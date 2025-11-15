using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BlindBoxAnimation : MonoBehaviour
{
    [Header("UI Elements (assign in Inspector)")]
    public Image boxImage;        // closed box sprite
    public Image rewardImage;     // reward sprite (initially disabled)
    //public ParticleSystem openEffect;
    public GameObject openEffect;
    public TextMeshProUGUI rewardText;
    public Button viewRewardBtn;

    [Header("Settings")]
    public float shakeDuration = 0.5f;
    public float shakeStrength = 15f;
    public int shakeVibrato = 10;
    public float scaleStep = 0.2f;
    public float stepDur = 0.2f;
    public float revealDelay = 0.05f;
    public float textRevealDelay = 0.5f;

    [Header("Sound Effects")]
    public AudioClip blindBoxRollSFX;
    public AudioSource audioSource;
    public SFXPlayer sfxPlayer;

    // Internal
    RectTransform rect;          // for UI scaling/shaking
    Transform t;

    void Awake()
    {
        audioSource = GameObject.Find("SFXPlayer").GetComponent<AudioSource>();
        t = transform;
        rect = GetComponent<RectTransform>();

        // Defensive: hide reward initially
        if (rewardImage != null)
            rewardImage.gameObject.SetActive(false);
        if (rewardText != null)
            rewardText.gameObject.SetActive(false);
        sfxPlayer = FindAnyObjectByType<SFXPlayer>();
    }

    // Public entrypoint
    public void playOpenAnimation(Sprite rewardSprite, string rewardName)
    {

        // Set reward sprite early so reveal shows correct sprite
        if (rewardImage != null)
            rewardImage.sprite = rewardSprite;
        if (rewardText != null)
            rewardText.text = rewardName;

        // Ensure reward hidden and effects stopped
        if (rewardImage != null) rewardImage.gameObject.SetActive(false);
        if (rewardText != null) rewardText.gameObject.SetActive(false);
        openEffect.SetActive(false);

        // Build sequence
        Sequence seq = DOTween.Sequence();

        // 1) Shake: UI vs world
        if (rect != null)
        {
            // DOShakeAnchorPos gives visible shake for UI
            seq.Append(rect.DOShakeAnchorPos(shakeDuration, shakeStrength, shakeVibrato, 90, false, true));
        }
        else
        {
            seq.Append(t.DOShakeRotation(shakeDuration, shakeStrength, shakeVibrato, 90, false));
        }

        // 2) Scale up, down, back
        Vector3 origScale = t.localScale;
        seq.Append(t.DOScale(origScale * (1f + scaleStep), stepDur));
        seq.Append(t.DOScale(origScale * (1f - scaleStep / 2f), stepDur));
        seq.Append(t.DOScale(origScale, stepDur));

        // slight delay then reveal
        seq.AppendInterval(revealDelay);

        // 3) Reveal callback: hide box, show reward, play particles
        seq.AppendCallback(() =>
        {
            if (boxImage != null) boxImage.gameObject.SetActive(false);
            if (rewardImage != null) rewardImage.gameObject.SetActive(true);
            openEffect.SetActive(true);
            sfxPlayer.playBlindBoxRoll(audioSource, blindBoxRollSFX);
        });

        // Optional: add a small pop scale for reward
        if (rewardImage != null)
        {
            RectTransform rRect = rewardImage.GetComponent<RectTransform>();
            if (rRect != null)
            {
                seq.Append(rRect.DOScale(0f, 0f)); // ensure start at 0
                seq.Append(rRect.DOScale(1f, 0.25f).SetEase(Ease.OutBack));
            }
            else
            {
                // reward is world object
                seq.Append(rewardImage.transform.DOScale(0f, 0f));
                seq.Append(rewardImage.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack));
            }
        }

        seq.AppendInterval(textRevealDelay);
        seq.AppendCallback(() =>
        {
            if (rewardText != null)
            {
                rewardText.gameObject.SetActive(true);
                rewardText.alpha = 0f;
                rewardText.DOFade(1f, 0.5f).SetEase(Ease.OutSine); // smooth fade in
            }
        });

        seq.OnComplete(() =>
        {
            if (viewRewardBtn != null)
            {
                viewRewardBtn.interactable = true;
            }
        });

        if (viewRewardBtn != null)
        {
            viewRewardBtn.interactable = false;
        }
    }
}
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item3DViewer : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    GameObject itemPrefab;
    Camera cam;
    ItemSelect select;

    bool isDragging = false;

    [SerializeField] GameObject tutorial;

    TextMeshProUGUI tutorialText;
    Image tutorialArrow;

    Color initialColor;

    bool zoomed = false;

    Vector3 initialPos;
    Vector3 initialScale;
    Vector3 initialBtnPos;
    Vector2 lastMousePos;
    [SerializeField] Image zoomBG;
    [SerializeField] GameObject zoomBtn;

    [SerializeField] Sprite zoomIcon;
    [SerializeField] Sprite zoomOutIcon;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        select = cam.GetComponent<ItemSelect>();
        tutorialText = tutorial.GetComponent<TextMeshProUGUI>();
        tutorialArrow = tutorial.GetComponentInChildren<Image>();

        initialColor = tutorialText.color;

        initialPos = this.gameObject.transform.GetChild(0).localPosition;
        initialScale = this.gameObject.transform.GetChild(0).localScale;

        initialBtnPos = zoomBtn.transform.localPosition;
    }

    void Update()
    {
        if (select.isLooking && !isDragging)
        {
            tutorial.SetActive(true);
            StartCoroutine(fadeInTutorial(0.5f, tutorialText));
            StartCoroutine(fadeInTutorial(0.5f, tutorialArrow));
        }
        else if (!select.isLooking)
        {
            isDragging = false;
            StopAllCoroutines();
            tutorialText.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
            tutorialArrow.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //isDragging = true;
        //if(tutorial != null)
        //{
        //    tutorial.SetActive(false);
        //}
        //itemPrefab = select.itemPrefab;
        //if (itemPrefab != null)
        //{
        //    itemPrefab.transform.eulerAngles += new Vector3(eventData.delta.y, -eventData.delta.x);
        //}
        isDragging = true;

        if (tutorial != null)
            tutorial.SetActive(false);

        itemPrefab = select.itemPrefab;
        if (itemPrefab == null)
            return;

        // Smooth rotation based on actual mouse movement
        if (eventData.dragging)
        {
            Vector3 delta = eventData.position - lastMousePos;

            float rotX = delta.y * 0.3f;
            float rotY = -delta.x * 0.3f;

            // Rotate smoothly in world space
            itemPrefab.transform.Rotate(Vector3.up, rotY, Space.World);
            itemPrefab.transform.Rotate(Vector3.right, rotX, Space.World);
        }

        lastMousePos = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lastMousePos = eventData.position;
    }

    IEnumerator fadeInTutorial(float duration, TextMeshProUGUI targetText)
    {
        yield return new WaitForSeconds(3f);
        float timer = 0f;
        Color startColor = targetText.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            targetText.color = Color.Lerp(startColor, targetColor, timer / duration);
            yield return null; // Wait for the next frame
        }
        targetText.color = targetColor;
    }

    IEnumerator fadeInTutorial(float duration, Image targetImage)
    {
        yield return new WaitForSeconds(3f);
        float timer = 0f;
        Color startColor = targetImage.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            targetImage.color = Color.Lerp(startColor, targetColor, timer / duration);
            yield return null; // Wait for the next frame
        }
        targetImage.color = targetColor;
    }

    public void zoom()
    {
        if (!zoomed)
        {
            this.gameObject.transform.GetChild(0).DOLocalMove(new Vector3(16, 16, 0), 0.2f);
            this.gameObject.transform.GetChild(0).DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.2f);
            zoomBtn.transform.DOLocalMove(new Vector3(-1283f, 443.76f, 0f), 0.2f);
            zoomBtn.GetComponent<Image>().sprite = zoomOutIcon;
            zoomBG.gameObject.SetActive(true);
            zoomBG.DOFade(0.85f, 0.2f);
            zoomed = true;
        }
        else
        {
            this.gameObject.transform.GetChild(0).DOLocalMove(initialPos, 0.2f);
            this.gameObject.transform.GetChild(0).DOScale(initialScale, 0.2f);
            zoomBtn.transform.DOLocalMove(initialBtnPos, 0.2f);
            zoomBtn.GetComponent<Image>().sprite = zoomIcon;
            zoomBG.gameObject.SetActive(false);
            zoomBG.DOFade(0f, 0.2f);
            zoomed = false;
        }
    }
}

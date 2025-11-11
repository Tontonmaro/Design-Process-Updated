using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [Header("Audio Setup")]
    public AudioSource drySource; // normal audio
    public AudioSource wetSource; // reverbed audio
    public List<AudioClip> playlistDry = new List<AudioClip>();
    public List<AudioClip> playlistWet = new List<AudioClip>();

    [Header("UI")]
    public CanvasGroup songInfoCanvasGroup;
    public TextMeshProUGUI songTitleText;

    [Header("Settings")]
    public float fadeTime = 1f;
    public float displayTime = 3f;
    public bool shuffle = true;

    private int currentIndex = 0;
    private List<int> shuffledIndices = new List<int>();
    private Coroutine songInfoCoroutine;
    private Coroutine playing;

    public bool tutorial = false;
    public ItemSelect select;

    void Start()
    {
        if (drySource == null || wetSource == null)
        {
            Debug.LogError("MusicPlayer: Assign both dry and wet AudioSources!");
            return;
        }

        if (playlistDry.Count == 0 || playlistWet.Count == 0)
        {
            Debug.LogWarning("MusicPlayer: One or both playlists are empty!");
            return;
        }

        // Ensure both lists have same count
        if (playlistDry.Count != playlistWet.Count)
        {
            Debug.LogWarning("MusicPlayer: Dry and Wet playlists must have same number of songs!");
        }

        // Generate shuffled indices
        shuffledIndices.Clear();
        for (int i = 0; i < playlistDry.Count; i++)
            shuffledIndices.Add(i);
        if (shuffle)
            ShuffleIndices();

        drySource.loop = false;
        wetSource.loop = false;
    }

    void Update()
    {
        if (tutorial)
        {
            if (playing == null)
            {
                playing = StartCoroutine(PlayPlaylist());
                tutorial = false;
            }
        }

        // Toggle mute with M
        if (Input.GetKeyDown(KeyCode.M) && !select.inMenu)
        {
            drySource.mute = !drySource.mute;
            wetSource.mute = !wetSource.mute;
        }

        // Switch between reverb and dry when menu opens/closes
        if (select.inMenu)
        {
            // In menu = no reverb
            drySource.DOFade(1f, 0.5f);
            wetSource.DOFade(0f, 0.5f);
        }
        else
        {
            // Out of menu = reverb on
            drySource.DOFade(0f, 0.5f);
            wetSource.DOFade(1f, 0.5f);
        }
    }

    public IEnumerator PlayPlaylist()
    {
        while (true)
        {
            int songIndex = shuffledIndices[currentIndex];
            AudioClip dryClip = playlistDry[songIndex];
            AudioClip wetClip = playlistWet[songIndex];

            drySource.clip = dryClip;
            wetSource.clip = wetClip;

            drySource.time = 0f;
            wetSource.time = 0f;

            //drySource.Play();
            //wetSource.Play();
            double startTime = AudioSettings.dspTime + 0.1; // start both 0.1s in the future
            drySource.PlayScheduled(startTime);
            wetSource.PlayScheduled(startTime);

            ShowSongInfo(dryClip.name);

            // Wait for song to end
            while (drySource.isPlaying)
                yield return null;

            // Move to next
            currentIndex++;
            if (currentIndex >= playlistDry.Count)
            {
                if (shuffle)
                    ShuffleIndices();
                currentIndex = 0;
            }
        }
    }

    void ShuffleIndices()
    {
        for (int i = 0; i < shuffledIndices.Count; i++)
        {
            int temp = shuffledIndices[i];
            int randomIndex = Random.Range(i, shuffledIndices.Count);
            shuffledIndices[i] = shuffledIndices[randomIndex];
            shuffledIndices[randomIndex] = temp;
        }
    }

    void ShowSongInfo(string songName)
    {
        if (songTitleText == null || songInfoCanvasGroup == null)
            return;

        if (songInfoCoroutine != null)
            StopCoroutine(songInfoCoroutine);

        songInfoCoroutine = StartCoroutine(FadeSongInfo(songName));
    }

    IEnumerator FadeSongInfo(string songName)
    {
        songTitleText.text = songName;
        songInfoCanvasGroup.alpha = 0f;

        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            songInfoCanvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeTime);
            yield return null;
        }

        yield return new WaitForSeconds(displayTime);

        t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            songInfoCanvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeTime);
            yield return null;
        }
    }
}

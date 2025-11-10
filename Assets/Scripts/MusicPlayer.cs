using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [Header("Audio Setup")]
    public AudioSource audioSource;
    public AudioReverbFilter reverbFilter;
    public List<AudioClip> playlist = new List<AudioClip>();

    [Header("UI")]
    public CanvasGroup songInfoCanvasGroup;
    public TextMeshProUGUI songTitleText;

    [Header("Settings")]
    public float fadeTime = 1f;
    public float displayTime = 3f;
    public bool shuffle = true; // toggle shuffle on/off

    private int currentIndex = 0;
    private List<AudioClip> shuffledList = new List<AudioClip>();
    private Coroutine songInfoCoroutine;

    public bool tutorial = false;
    public ItemSelect select;

    private Coroutine playing;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        if (reverbFilter == null)
        {
            reverbFilter = GetComponent<AudioReverbFilter>();
        }

        if (playlist.Count == 0)
        {
            Debug.LogWarning("MusicPlayer: Playlist is empty!");
            return;
        }

        // Prepare playlist order
        shuffledList = new List<AudioClip>(playlist);
        if (shuffle)
            ShuffleList(shuffledList);
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
        if (Input.GetKeyDown(KeyCode.M) && !select.inMenu)
        {
            if (audioSource.mute == false)
            {
                audioSource.mute = true;
            }
            else
            {
                audioSource.mute = false;
            }
        }

        if (select.inMenu)
        {
            DOTween.To(() => reverbFilter.reverbLevel, x => reverbFilter.reverbLevel = x, 0f, 0.5f);
        }
        else
        {
            DOTween.To(() => reverbFilter.reverbLevel, x => reverbFilter.reverbLevel = x, 1172f, 0.5f);
        }
    }

    public IEnumerator PlayPlaylist()
    {
        while (true)
        {
            // Play next song
            AudioClip nextClip = shuffledList[currentIndex];
            audioSource.clip = nextClip;
            audioSource.Play();

            // Show now playing info
            ShowSongInfo(nextClip.name);

            // Wait for current clip to finish playing
            while (audioSource.isPlaying)
                yield return null;

            // Move to next track
            currentIndex++;

            // If we reached end, reshuffle and start again
            if (currentIndex >= shuffledList.Count)
            {
                if (shuffle)
                    ShuffleList(shuffledList);
                currentIndex = 0;
            }
        }
    }

    void ShuffleList(List<AudioClip> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            AudioClip temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    void ShowSongInfo(string songName)
    {
        if (songTitleText == null || songInfoCanvasGroup == null)
            return;

        // Stop previous song info fade, without stopping PlayPlaylist
        if (songInfoCoroutine != null)
            StopCoroutine(songInfoCoroutine);

        songInfoCoroutine = StartCoroutine(FadeSongInfo(songName));
    }

    IEnumerator FadeSongInfo(string songName)
    {
        songTitleText.text = songName;
        songInfoCanvasGroup.alpha = 0f;

        // Fade in
        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            songInfoCanvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeTime);
            yield return null;
        }

        // Stay visible
        yield return new WaitForSeconds(displayTime);

        // Fade out
        t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            songInfoCanvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeTime);
            yield return null;
        }
    }
}
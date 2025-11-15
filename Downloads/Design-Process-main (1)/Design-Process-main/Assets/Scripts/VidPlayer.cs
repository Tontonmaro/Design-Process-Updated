using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VidPlayer : MonoBehaviour
{
    [SerializeField] string videoFileName;
    [SerializeField] ItemSelect select;
    VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        playVideo();
    }

    private void Update()
    {
        if (select != null)
        {
            if (select.inMenu)
            {
                videoPlayer.Pause();
            }
            else
            {
                videoPlayer.Play();
            }
        }
    }
    public void playVideo()
    { 
        if (videoPlayer)
        {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
            videoPlayer.url = videoPath;
            videoPlayer.Play();
        }
    }
}

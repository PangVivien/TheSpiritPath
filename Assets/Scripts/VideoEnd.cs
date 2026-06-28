using UnityEngine;
using UnityEngine.Video;

public class VideoEnd : MonoBehaviour
{
    public GameObject openingImage;   
    public GameObject fadePanel;

    private VideoPlayer vp;

    void Awake()
    {
        vp = GetComponent<VideoPlayer>();
        vp.loopPointReached += EndVideo;
    }

    void EndVideo(VideoPlayer player)
    {
        openingImage.SetActive(false);
        fadePanel.SetActive(true);
    }
}

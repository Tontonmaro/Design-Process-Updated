using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class RewardUI : MonoBehaviour
{
    public UnityEngine.UI.Image rewardImage;
    public TextMeshProUGUI rewardNameText;
    public UnityEngine.UI.Button rewardButton;

    private BlindBoxData.BlindBoxReward rewardData;

    [HideInInspector]public RewardViewer viewer;

    public void setup(BlindBoxData.BlindBoxReward reward, RewardViewer viewerRef)
    {
        rewardData = reward;
        viewer = viewerRef;

        rewardButton.onClick.RemoveAllListeners();
        rewardButton.onClick.AddListener(() =>
        {
            if (viewer != null)
            {
                viewer.ShowReward(rewardData);
            }
            else
            {
                Debug.LogError($"RewardViewer reference missing on {name}");
            }
        });
    }
}

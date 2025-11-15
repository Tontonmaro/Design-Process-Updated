using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Success : MonoBehaviour
{
    [SerializeField] CanvasGroup rewardsPanel;
    public BlindBoxManager blindBoxManager;
    public void proceedToRewards()
    {
        rewardsPanel.gameObject.SetActive(true);
        rewardsPanel.DOFade(1f, 0.2f)
            .OnComplete(() => this.gameObject.GetComponent<CanvasGroup>().alpha = 0f)
            .OnComplete(() => this.gameObject.SetActive(false));
        blindBoxManager.openAllBoxes();
    }
}

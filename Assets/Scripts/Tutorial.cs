using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tutorial : MonoBehaviour
{
    [SerializeField] CanvasGroup tutorialPanel;
    [SerializeField] GameObject tutorialPanelObj;

    public bool inTutorial = true;
    public void closeTut()
    {
        tutorialPanel.DOFade(0f, 0.2f)
            .OnComplete(() => tutorialPanelObj.SetActive(false));
        Cursor.lockState = CursorLockMode.Locked;
        inTutorial = false;
    }
}

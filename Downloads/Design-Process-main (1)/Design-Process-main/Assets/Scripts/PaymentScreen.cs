using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaymentScreen : MonoBehaviour
{
    [SerializeField] CanvasGroup successPanel;
    private void OnEnable()
    {
        StartCoroutine(loading());
    }

    IEnumerator loading()
    {
        yield return new WaitForSeconds(5f);
        successPanel.gameObject.SetActive(true);
        successPanel.DOFade(1f, 0.2f)
            .OnComplete(() => this.gameObject.GetComponent<CanvasGroup>().alpha = 0f)
            .OnComplete(() => this.gameObject.SetActive(false));
    }
}

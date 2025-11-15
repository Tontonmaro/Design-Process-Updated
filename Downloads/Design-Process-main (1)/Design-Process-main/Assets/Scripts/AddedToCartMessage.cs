using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddedToCartMessage : MonoBehaviour
{
    public CanvasGroup msg;

    private void OnEnable()
    {
        StartCoroutine(fadeMsg());
    }

    IEnumerator fadeMsg()
    {
        msg.DOFade(1f, 0.2f);
        yield return new WaitForSeconds(2f);
        msg.DOFade(0f, 0.2f)
        .OnComplete(() => this.gameObject.SetActive(false));
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingCart : MonoBehaviour
{
    public List<GameObject> cartItems = new List<GameObject>();

    [SerializeField] CanvasGroup msg;

    IEnumerator fadeMsg()
    {
        msg.DOFade(1f, 0.2f);
        yield return new WaitForSeconds(2f);
        msg.DOFade(0f, 0.2f);
    }

    public void addedToCartMsg()
    {
        StartCoroutine(fadeMsg());
    }
}

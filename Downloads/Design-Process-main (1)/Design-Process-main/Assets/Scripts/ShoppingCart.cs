using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingCart : MonoBehaviour
{
    public List<GameObject> cartItems = new List<GameObject>();

    [SerializeField] CanvasGroup msg;

    public void clearMsg()
    {
        StopAllCoroutines();
        msg.alpha = 0f;
    }

    public void addedToCartMsg()
    {
        msg.gameObject.SetActive(true);
    }
}

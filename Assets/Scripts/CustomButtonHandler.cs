using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CustomButtonHandler : MonoBehaviour, IPointerDownHandler
{
    ItemSelect select;
    [SerializeField] Tutorial tut;
    private void Awake()
    {
        select = Camera.main.GetComponent<ItemSelect>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            if (button.GetComponentInChildren<TextMeshProUGUI>() != null)
            {
                if (button.GetComponentInChildren<TextMeshProUGUI>().text == "Add to Cart")
                {
                    select.addToCart();
                    button.interactable = false;
                    StartCoroutine(enableButtonAfterDelay(button));
                }
                else if(button.GetComponentInChildren<TextMeshProUGUI>().text == "Close Tutorial")
                {
                    tut.closeTut();
                }
                else if (button.GetComponentInChildren<TextMeshProUGUI>().text == "Close Info Panel")
                {
                    select.exit();
                }
                else if (button.GetComponentInChildren<TextMeshProUGUI>().text == "Back to exhibition")
                {
                    select.exitCart();
                }
                else if (button.GetComponentInChildren<TextMeshProUGUI>().text == "Buy Now")
                {
                    select.buyNow();
                    button.interactable = false;
                    StartCoroutine(enableButtonAfterDelay(button));
                }
            }
        }
    }
    IEnumerator enableButtonAfterDelay(Button button)
    {
        yield return new WaitForSeconds(0.5f);
        button.interactable = true;
    }
}

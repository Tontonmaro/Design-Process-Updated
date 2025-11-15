using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderSummary : MonoBehaviour
{
    public GameObject listingPrefab;
    public GameObject listingParent;

    public List<GameObject> listings = new List<GameObject>();

    [SerializeField] ItemSelect select;
    [SerializeField] Checkout checkout;
    [SerializeField] ShoppingCart cart;
    [SerializeField] BuyNowCart buyNowCart;
    [SerializeField] CanvasGroup checkoutPanel;
    ItemDetails details;

    [SerializeField] TextMeshProUGUI error;

    [SerializeField] TextMeshProUGUI subtotalPriceText;
    [SerializeField] TextMeshProUGUI subtotalQtyText;
    [SerializeField] TextMeshProUGUI totalPriceText;
    [SerializeField] TextMeshProUGUI shippingPriceText;

    public bool spawned = false;
    public void spawnListings(GameObject prefab)
    {
        if (prefab != null)
        {
            details = prefab.GetComponent<ItemDetails>();
        }
        GameObject newListing = Instantiate(listingPrefab, listingParent.transform);
        CartItemUI cartUI = newListing.GetComponent<CartItemUI>();
        cartUI.Setup(details, select);
        listings.Add(newListing);
        newListing.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = details.name;
        newListing.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = details.subtitle;
        newListing.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = details.chosenSize;
        newListing.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = details.quantity.ToString();
        newListing.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "$" + details.chosenPrice.ToString("F2");
        newListing.transform.GetChild(5).GetComponent<Image>().sprite = details.thumbnail;
    }

    public void destroyListings()
    {
        for(int i = 0; i < listings.Count; i++)
        {
            Destroy(listings[i]);
        }
        spawned = false;
    }

    public void proceedToCheckout()
    {
        int totalQty = 0;
        if (!select.boughtNow)
        {
            if (cart.cartItems.Count > 0)
            {
                error.alpha = 0;
                checkoutPanel.gameObject.SetActive(true);
                checkoutPanel.DOFade(1f, 0.2f)
                    .OnComplete(() => this.gameObject.GetComponent<CanvasGroup>().alpha = 0f)
                    .OnComplete(() => this.gameObject.SetActive(false));
                for (int i = 0; i < cart.cartItems.Count; i++)
                {
                    checkout.spawnListings(cart.cartItems[i]);
                    totalQty += cart.cartItems[i].GetComponent<ItemDetails>().quantity;
                }
                select.refreshPrice(subtotalPriceText);
                select.refreshTotalPrice(totalPriceText);
                select.refreshPriceShipping(shippingPriceText);
                subtotalQtyText.text = "Subtotal (" + totalQty + " items):";
            }
            else
            {
                error.DOFade(1f, 0.2f);
            }
        }
        else
        {
            if (buyNowCart.buyNowItems.Count > 0)
            {
                error.alpha = 0;
                checkoutPanel.gameObject.SetActive(true);
                checkoutPanel.DOFade(1f, 0.2f)
                    .OnComplete(() => this.gameObject.GetComponent<CanvasGroup>().alpha = 0f)
                    .OnComplete(() => this.gameObject.SetActive(false));
                for (int i = 0; i < buyNowCart.buyNowItems.Count; i++)
                {
                    checkout.spawnListings(buyNowCart.buyNowItems[i]);
                    totalQty += buyNowCart.buyNowItems[i].GetComponent<ItemDetails>().quantity;
                }
                select.refreshPrice(subtotalPriceText);
                select.refreshTotalPrice(totalPriceText);
                select.refreshPriceShipping(shippingPriceText);
                subtotalQtyText.text = "Subtotal (" + totalQty + " items):";
            }
            else
            {
                error.DOFade(1f, 0.2f);
            }
        }
    }

    public void returnToSummary()
    {
        checkout.clearErrors();
        checkoutPanel.DOFade(1f, 0.2f)
            .OnComplete(() => checkoutPanel.gameObject.SetActive(false));
        this.gameObject.SetActive(true);
        this.gameObject.GetComponent<CanvasGroup>().DOFade(1f, 0.2f)
            .OnComplete(() => checkout.destroyListings());
    }
}

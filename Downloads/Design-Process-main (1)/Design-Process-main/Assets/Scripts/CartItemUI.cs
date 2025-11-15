using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CartItemUI : MonoBehaviour
{
    public TextMeshProUGUI quantityText;
    public Button increaseButton;
    public Button decreaseButton;
    public Button deleteButton;

    private ItemDetails linkedItem;
    private ItemSelect itemSelect;
    private ShoppingCart cart;
    private BuyNowCart buyNowCart;

    public void Setup(ItemDetails item, ItemSelect select)
    {
        linkedItem = item;
        itemSelect = select;

        cart = GameObject.FindWithTag("Cart").GetComponent<ShoppingCart>();
        buyNowCart = GameObject.FindWithTag("BuyNowCart").GetComponent<BuyNowCart>();

        UpdateUI();
        
        increaseButton.onClick.AddListener(IncreaseQuantity);
        decreaseButton.onClick.AddListener(DecreaseQuantity);
        deleteButton.onClick.AddListener(DeleteItem);
    }

    void IncreaseQuantity()
    {
        linkedItem.quantity++;
        linkedItem.quantity = Mathf.Min(linkedItem.quantity, 20);
        UpdateUI();
    }

    void DecreaseQuantity()
    {
        linkedItem.quantity = Mathf.Max(1, linkedItem.quantity - 1);
        UpdateUI();
    }

    void DeleteItem()
    {
        if (cart.cartItems.Contains(linkedItem.gameObject))
        {
            cart.cartItems.Remove(linkedItem.gameObject);
        }
        if(buyNowCart.buyNowItems.Contains(linkedItem.gameObject))
        {
            buyNowCart.buyNowItems.Remove(linkedItem.gameObject);
        }

        Destroy(linkedItem.gameObject);
        Destroy(gameObject);
        itemSelect.refreshPrice(itemSelect.totalPriceText);
    }

    void UpdateUI()
    {
        quantityText.text = linkedItem.quantity.ToString();
        itemSelect.refreshPrice(itemSelect.totalPriceText);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeQuantity : MonoBehaviour
{
    ItemDetails itemDetails;
    GameObject itemPrefab;
    Camera cam;
    ItemSelect itemSelect;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        itemSelect = cam.GetComponent<ItemSelect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (itemSelect.isLooking)
        {
            itemPrefab = itemSelect.itemPrefab;
            itemDetails = itemPrefab.GetComponent<ItemDetails>();
        }
    }

    public void increaseQuantity()
    {
        if(itemDetails != null && itemDetails.quantity < 10)
        {
            itemDetails.quantity++;
        }
    }

    public void decreaseQuantity()
    {
        if (itemDetails != null && itemDetails.quantity > 0)
        {
            itemDetails.quantity--;
        }
    }
}

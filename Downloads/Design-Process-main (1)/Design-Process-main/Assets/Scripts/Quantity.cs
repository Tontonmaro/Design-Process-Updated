using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Quantity : MonoBehaviour
{
    TMP_InputField quantity;
    Camera cam;
    ItemSelect select;
    GameObject itemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        quantity = GetComponent<TMP_InputField>();
        cam = Camera.main;
        select = cam.GetComponent<ItemSelect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (select.isLooking)
        {
            itemPrefab = select.itemPrefab;
            if (itemPrefab != null)
            {
                quantity.text = itemPrefab.GetComponent<ItemDetails>().quantity.ToString();
            }
        }
    }
}

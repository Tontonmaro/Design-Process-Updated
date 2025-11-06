using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Checkout : MonoBehaviour
{
    public GameObject listingPrefab;
    public GameObject listingParent;

    public List<GameObject> listings = new List<GameObject>();

    ItemDetails details;
    public void spawnListings(GameObject prefab)
    {
        if (prefab != null)
        {
            details = prefab.GetComponent<ItemDetails>();
        }
        GameObject newListing = Instantiate(listingPrefab, listingParent.transform);
        CartItemUI cartUI = newListing.GetComponent<CartItemUI>();
        listings.Add(newListing);
        newListing.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = details.name;
        newListing.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = details.subtitle;
        newListing.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = details.chosenSize;
        newListing.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "$" + details.chosenPrice.ToString("F2");
        newListing.transform.GetChild(4).GetComponent<Image>().sprite = details.thumbnail;
        newListing.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = details.quantity.ToString();
    }

    public void destroyListings()
    {
        for (int i = 0; i < listings.Count; i++)
        {
            Destroy(listings[i]);
        }
    }
}

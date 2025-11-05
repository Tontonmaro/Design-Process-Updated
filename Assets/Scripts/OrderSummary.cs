using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderSummary : MonoBehaviour
{
    public GameObject listingPrefab;
    public GameObject listingParent;

    public List<GameObject> listings = new List<GameObject>();

    [SerializeField] ItemSelect select;
    ItemDetails details;
    public void spawnListings(GameObject prefab)
    {
        details = prefab.GetComponent<ItemDetails>();
        for (int i = 0; i < details.sizes.Length; i++)
        {
            GameObject newListing = Instantiate(listingPrefab, listingParent.transform);
            listings.Add(newListing);
            newListing.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Name";
            newListing.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Subtitle";
            newListing.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Size";
            newListing.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Quantity";
            newListing.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Price";
            //newListing.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "Name";
        }
    }
}

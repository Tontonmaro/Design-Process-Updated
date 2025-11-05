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

    public bool spawned = false;
    public void spawnListings(GameObject prefab)
    {
        details = prefab.GetComponent<ItemDetails>();
        GameObject newListing = Instantiate(listingPrefab, listingParent.transform);
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
}

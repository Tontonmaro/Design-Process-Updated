using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Text.RegularExpressions;

public class Checkout : MonoBehaviour
{
    public GameObject listingPrefab;
    public GameObject listingParent;

    public GameObject errorMsg;

    public List<GameObject> listings = new List<GameObject>();

    ItemDetails details;

    [SerializeField] CanvasGroup paymentPanel;
    [SerializeField] PaymentDetailsHandler paymentDetails;
    [SerializeField] CountryDropdown countryDropdown;
    [SerializeField] TextMeshProUGUI[] errors;
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

    public void proceedToPayment()
    {
        bool hasError = false;
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(paymentDetails.emailAddress.text, emailPattern))
        {
            error(errors[0]);
            hasError = true;
        }
        if (paymentDetails.country.options[paymentDetails.country.value].text == "<i><color=#00000080>Country/Region</color></i>")
        {
            error(errors[1]);
            hasError = true;
        }
        if (string.IsNullOrEmpty(paymentDetails.firstName.text))
        {
            error(errors[2]);
            hasError = true;
        }
        if (string.IsNullOrEmpty(paymentDetails.lastName.text))
        {
            error(errors[3]);
            hasError = true;
        }
        if (string.IsNullOrEmpty(paymentDetails.address.text))
        {
            error(errors[4]);
            hasError = true;
        }
        if (string.IsNullOrEmpty(paymentDetails.apartment.text))
        {
            error(errors[5]);
            hasError = true;
        }
        if (!Regex.IsMatch(paymentDetails.postalCode.text, @"^\d{6}$"))
        {
            error(errors[6]);
            hasError = true;
        }
        if (!paymentDetails.toggleGrp.AnyTogglesOn())
        {
            error(errors[7]);
            hasError = true;
        }
        if (hasError)
        {
            StartCoroutine(mainError(errorMsg));
            return;
        }
        clearErrors();
        destroyListings();
        countryDropdown.SetupDropDown();
        paymentDetails.resetInputFields();
        paymentPanel.gameObject.SetActive(true);
        paymentPanel.DOFade(1f, 0.2f)
            .OnComplete(() => this.gameObject.GetComponent<CanvasGroup>().alpha = 0f)
            .OnComplete(() => this.gameObject.SetActive(false));
    }

    void error(TextMeshProUGUI errorMsg)
    {
        for(int i = 0; i < errors.Length; i++)
        {
            errors[i].alpha = 0f;
        }
        errorMsg.DOFade(1f, 0.2f);
    }

    public void clearErrors()
    {
        for (int i = 0; i < errors.Length; i++)
        {
            errors[i].alpha = 0f;
        }
    }

    IEnumerator mainError(GameObject errorMsg)
    {
        errorMsg.SetActive(true);
        errorMsg.GetComponent<CanvasGroup>().DOFade(1f, 0.2f);
        yield return new WaitForSeconds(1f);
        errorMsg.GetComponent<CanvasGroup>().DOFade(0f, 1f)
            .OnComplete(() =>errorMsg.gameObject.SetActive(false));
    }
}

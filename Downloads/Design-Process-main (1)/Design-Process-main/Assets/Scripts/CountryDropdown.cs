using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class CountryDropdown : MonoBehaviour
{
    public TMP_Dropdown countryDropdown; // Assign this in the Inspector
    private bool placeholderRemoved = false;

    void Start()
    {
        if (countryDropdown == null)
        {
            countryDropdown = GetComponent<TMP_Dropdown>();
        }

        SetupDropDown();
        countryDropdown.onValueChanged.AddListener(OnCountryChanged);
    }

    private void OnCountryChanged(int index)
    {
        // Ignore further calls after removing placeholder
        if (placeholderRemoved)
            return;

        if (index > 0)
        {
            // Remove placeholder (first item)
            countryDropdown.options.RemoveAt(0);
            placeholderRemoved = true;

            // Adjust selected index since list shrank
            countryDropdown.value = index - 1;
            countryDropdown.RefreshShownValue();
        }
    }

    public void SetupDropDown()
    {
        List<string> countries = new List<string>
        {
            "<i><color=#00000080>Country/Region</color></i>",
            "Argentina",
            "Australia",
            "Brazil",
            "Canada",
            "China",
            "Egypt",
            "France",
            "Germany",
            "India",
            "Indonesia",
            "Italy",
            "Japan",
            "Malaysia",
            "Mexico",
            "Netherlands",
            "New Zealand",
            "Norway",
            "Philippines",
            "Russia",
            "Singapore",
            "South Africa",
            "South Korea",
            "Spain",
            "Sweden",
            "Switzerland",
            "Thailand",
            "United Arab Emirates",
            "United Kingdom",
            "United States",
            "Vietnam"
            // Add more countries as needed
        };

        // Clear existing options
        countryDropdown.ClearOptions();

        // Add the country list to the dropdown
        countryDropdown.AddOptions(countries);

        countryDropdown.value = 0;
        placeholderRemoved = false;
    }
}

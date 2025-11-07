using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaymentDetailsHandler : MonoBehaviour
{
    public TMP_InputField emailAddress;
    public TMP_InputField country;
    public TMP_InputField firstName;
    public TMP_InputField lastName;
    public TMP_InputField address;
    public TMP_InputField apartment;
    public TMP_InputField postalCode;
    public TMP_InputField[] fields;
    public ToggleGroup toggleGrp;

    private void Awake()
    {
        fields = new TMP_InputField[]
        {
            emailAddress, country, firstName, lastName, address, apartment, postalCode
        };  
    }
    private void Update()
    {
        if (toggleGrp.AnyTogglesOn())
        {
            toggleGrp.allowSwitchOff = false;
        }
    }
    public void resetInputFields()
    {
        for(int i = 0; i < fields.Length; i++)
        {
            fields[i].text = string.Empty;
        }
    }
}

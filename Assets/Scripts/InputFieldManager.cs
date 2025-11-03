using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputFieldManager : MonoBehaviour
{
    [SerializeField] TMP_InputField email;
    [SerializeField] TMP_InputField password;

    // Start is called before the first frame update
    void Start()
    {
        if (email != null && email.placeholder != null)
        {
            TextMeshProUGUI placeholderText = email.placeholder as TextMeshProUGUI;
            if (placeholderText != null)
            {
                placeholderText.text = "firstnamelastname@email.com";
            }
        }

        if (password != null && password.placeholder != null)
        {
            TextMeshProUGUI placeholderText = password.placeholder as TextMeshProUGUI;
            if (placeholderText != null)
            {
                placeholderText.text = "************";
            }
        }
    }
}

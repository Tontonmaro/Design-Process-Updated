using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField emailOrPhone;
    public TMP_InputField password;
    private const string phoneNumberRegexPattern = @"^\d{8}$";

    public TextMeshProUGUI emailError;
    public TextMeshProUGUI passwordError;

    public CanvasGroup loginCanvas;
    public CanvasGroup successCanvas;

    bool hasError;

    public void Login()
    {
        hasError = false;
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        if (!(Regex.IsMatch(emailOrPhone.text, emailPattern) || Regex.IsMatch(emailOrPhone.text, phoneNumberRegexPattern)))
        {
            hasError = true;
            error(emailError);
        }
        if (string.IsNullOrEmpty(password.text))
        {
            hasError = true;
            error(passwordError);
        }
        if (hasError)
        {
            return;
        }
        emailError.alpha = 0;
        passwordError.alpha = 0;
        loginCanvas.DOFade(0f, 0.2f)
            .OnComplete(() => loginCanvas.gameObject.SetActive(false));
        successCanvas.gameObject.SetActive(true);
        successCanvas.DOFade(1f, 0.2f);

    }

    void error(TextMeshProUGUI errorMsg)
    {
        emailError.alpha = 0;
        passwordError.alpha = 0;
        errorMsg.DOFade(1f, 0.2f);
    }
}

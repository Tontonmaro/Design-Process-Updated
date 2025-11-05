using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SizeSelector : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject buttonParent;

    public List<GameObject> buttons = new List<GameObject>();

    [SerializeField] ItemSelect select;
    [SerializeField] GameObject priceText;
    ItemDetails details;
    public void spawnButtons(GameObject prefab)
    {
        details = prefab.GetComponent<ItemDetails>();
        for (int i = 0; i < details.sizes.Length; i++)
        {
            int index = i;
            GameObject newButton = Instantiate(buttonPrefab, buttonParent.transform);
            buttons.Add(newButton);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = details.sizes[index];
            newButton.GetComponent<Button>().onClick.AddListener(() => selectSize(newButton.GetComponent<Button>(), details.priceOptions[index]));
        }
    }

    void selectSize(Button button, float price)
    {
        if (details != null)
        {
            details.chosenSize = button.GetComponentInChildren<TextMeshProUGUI>().text;
            details.chosenPrice = price;
            priceText.GetComponent<TextMeshProUGUI>().text = "$" + price.ToString("F2");
        }
        for (int i = 0; i < buttons.Count; i++)
        {
            Image buttonImage = buttons[i].GetComponent<Image>();
            if (details.chosenSize == buttons[i].GetComponentInChildren<TextMeshProUGUI>().text)
            {
                buttonImage.color = new Color(0.4292453f, 0.645949f, 1f, 1f);
            }
            else
            {
                buttonImage.color = Color.white;
            }
        }
    }
}

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
    ItemDetails details;
    public void spawnButtons(GameObject prefab)
    {
        details = prefab.GetComponent<ItemDetails>();
        for (int i = 0; i < details.sizes.Length; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, buttonParent.transform);
            buttons.Add(newButton);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = details.sizes[i];
            newButton.GetComponent<Button>().onClick.AddListener(() => selectSize(newButton.GetComponent<Button>()));
        }
    }

    void selectSize(Button button)
    {
        if (details != null)
        {
            details.chosenSize = button.GetComponentInChildren<TextMeshProUGUI>().text;
        }
        for (int i = 0; i < buttons.Count; i++)
        {
            Image buttonImage = buttons[i].GetComponent<Image>();
            if (details.chosenSize == buttons[i].GetComponentInChildren<TextMeshProUGUI>().text)
            {
                buttonImage.color = new Color(1f, 0.5f, 0f, 1f);
            }
            else
            {
                buttonImage.color = Color.white;
            }
        }
    }
}

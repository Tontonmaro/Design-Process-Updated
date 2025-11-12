using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSelect : MonoBehaviour
{
    public bool isLooking = false;

    Transform cameraPosTransform;
    Transform objTransform;

    private Vector3 initialMousePosition;
    private Quaternion initialRotation;
    public float rotationSpeed = 0.5f;

    Camera cam;

    Vector3 initialPos;

    IEnumerator cameraMovement;

    public CanvasGroup infoPanel;
    public CanvasGroup rewardsPanel;
    public GameObject title;
    public GameObject subtitle;
    public GameObject price;

    public CanvasGroup cartPanel;

    [HideInInspector] public GameObject itemPrefab;
    [HideInInspector] public GameObject cartItemPrefab;

    [SerializeField] SizeSelector sizeSelector;
    [SerializeField] ShoppingCart cart;
    [SerializeField] BuyNowCart buyNowCart;
    [SerializeField] Tutorial tut;
    [SerializeField] public OrderSummary summary;
    [SerializeField] BlindBoxManager blindBoxManager;
    [SerializeField] SFXPlayer sfxPlayer;

    [SerializeField] CanvasGroup error;

    public TextMeshProUGUI totalPriceText;
    public TextMeshProUGUI emptyError;

    public bool boughtNow = false;
    public bool inMenu;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.localPosition;
        cam = Camera.main;

        infoPanel.gameObject.SetActive(false);
        inMenu = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !inMenu)
        {
            enterCart();
        }
        if(Input.GetKeyDown(KeyCode.T) && !inMenu)
        {
            tut.openTut();
        }
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (isLooking == false)
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider != null && hitInfo.collider.tag == "Item")
                {
                    objTransform = hitInfo.collider.transform;
                    GameObject item = hitInfo.collider.gameObject;
                    if (hitInfo.distance <= 5f)
                    {
                        item.GetComponent<Outline>().enabled = true;
                        if (Input.GetMouseButtonDown(0) && !inMenu)
                        {
                            Cursor.lockState = CursorLockMode.Locked;
                            Cursor.lockState = CursorLockMode.None;
                            isLooking = true;
                            inMenu = true;
                            itemPrefab = Instantiate(item, new Vector3 (100, 100, 103), Quaternion.identity);

                            infoPanel.gameObject.SetActive(true);
                            infoPanel.DOFade(1f, 0.2f);
                            cart.clearMsg();
                            sizeSelector.spawnButtons(itemPrefab);

                            ItemDetails details = item.GetComponent<ItemDetails>();
                            title.GetComponent<TextMeshProUGUI>().text = details.name;
                            subtitle.GetComponent<TextMeshProUGUI>().text = details.subtitle;
                            price.GetComponent<TextMeshProUGUI>().text = ("$" + details.chosenPrice.ToString("F2"));
                        }
                    }
                    else
                    {
                        item.GetComponent<Outline>().enabled = false;
                    }
                }
                else if(hitInfo.collider != null && hitInfo.collider.tag == "Checkout" && !inMenu)
                {
                    objTransform = hitInfo.collider.transform;
                    GameObject item = hitInfo.collider.gameObject;
                    if (hitInfo.distance <= 5f)
                    {
                        item.GetComponent<Outline>().enabled = true;
                        if (Input.GetMouseButtonDown(0) && !summary.spawned)
                        {
                            enterCart();
                        }
                    }
                    else
                    {
                        item.GetComponent<Outline>().enabled = false;
                    }
                }
                else
                {
                    if (objTransform != null)
                    {
                        objTransform.gameObject.GetComponent<Outline>().enabled = false;
                    }
                }
            }
        }
    }

    //IEnumerator MoveCamera()
    //{
    //    float time = 0;
    //    while (time < 1)
    //    {
    //        cam.transform.position = Vector3.Lerp(cam.transform.position, cameraPosTransform.position, time);
    //        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.identity, time);
    //        time += Time.deltaTime;
    //        yield return null;
    //    }
    //}

    //IEnumerator RotateBack()
    //{
    //    float time = 0;
    //    while (time < 1)
    //    {
    //        objTransform.rotation = Quaternion.Lerp(objTransform.rotation, Quaternion.identity, time);
    //        time += Time.deltaTime;
    //        yield return null;
    //    }
    //}

    //IEnumerator ReturnCamera()
    //{
    //    float time = 0;
    //    while (time < 1)
    //    {
    //        cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, initialPos, time);
    //        time += Time.deltaTime;
    //        yield return null;
    //    }
    //}

    public void exit()
    {
        if (isLooking)
        {
            isLooking = false;
            //StopCoroutine(cameraMovement);
            //StartCoroutine(RotateBack());
            //StartCoroutine(ReturnCamera());
            infoPanel.alpha = 0f;
            infoPanel.gameObject.SetActive(false);

            Destroy(itemPrefab);
            for (int i = 0; i < sizeSelector.buttons.Count; i++)
            {
                if (sizeSelector.buttons[i] != null)
                {
                    Destroy(sizeSelector.buttons[i]);
                }
            }

            sizeSelector.buttons.Clear();
            error.DOFade(0f, 0.2f);
            Cursor.lockState = CursorLockMode.Locked;
            inMenu = false;
        }
    }

    public void addToCart()
    {
        if (itemPrefab != null)
        {
            if (itemPrefab.GetComponent<ItemDetails>().chosenSize != "")
            {
                foreach (GameObject itemObj in cart.cartItems)
                {
                    ItemDetails existingDetails = itemObj.GetComponent<ItemDetails>();
                    ItemDetails newDetails = itemPrefab.GetComponent<ItemDetails>();
                    if (existingDetails.name == newDetails.name && existingDetails.chosenSize == newDetails.chosenSize)
                    {
                        existingDetails.quantity += newDetails.quantity;
                        cart.addedToCartMsg();
                        exit();
                        return;
                    }
                }
                cart.cartItems.Add(Instantiate(itemPrefab, new Vector3(200, 200, 200), Quaternion.identity));
                cart.addedToCartMsg();
                exit();
            }
            else
            {
                error.DOFade(1f, 0.2f);
            }
        }
    }

    public void enterCart()
    {
        summary.spawned = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.None;
        cartPanel.gameObject.SetActive(true);
        cartPanel.DOFade(1f, 0.2f);
        for (int i = 0; i < cart.cartItems.Count; i++)
        {
            summary.spawnListings(cart.cartItems[i]);
            refreshPrice(totalPriceText);
        }
        if(cart.cartItems.Count == 0)
        {
            refreshPrice(totalPriceText);
        }
        inMenu = true;
    }

    public void exitCart()
    {
        summary.destroyListings();
        emptyError.alpha = 0;
        cartPanel.DOFade(0f, 0.2f)
            .OnComplete(() => cartPanel.gameObject.SetActive(false));
        buyNowCart.buyNowItems.Clear();
        boughtNow = false;
        isLooking = false;
        Cursor.lockState = CursorLockMode.Locked;
        inMenu = false;
    }

    public void returnToExhibition()
    {
        summary.destroyListings();
        blindBoxManager.clearRewards();
        emptyError.alpha = 0;
        rewardsPanel.DOFade(0f, 0.2f)
            .OnComplete(() => rewardsPanel.gameObject.SetActive(false));
        foreach (GameObject itemObj in cart.cartItems)
        {
            Destroy(itemObj);
        }
        cart.cartItems.Clear();
        foreach (GameObject itemObj in buyNowCart.buyNowItems)
        {
            Destroy(itemObj);
        }
        buyNowCart.buyNowItems.Clear();
        boughtNow = false;
        isLooking = false;
        summary.spawned = false;
        sfxPlayer.sfxPlayed = false;
        Cursor.lockState = CursorLockMode.Locked;
        inMenu = false;
    }

    public void buyNow()
    {
        boughtNow = true;
        if (itemPrefab != null)
        {
            if (itemPrefab.GetComponent<ItemDetails>().chosenSize != "")
            {
                foreach (GameObject itemObj in cart.cartItems)
                {
                    ItemDetails existingDetails = itemObj.GetComponent<ItemDetails>();
                    ItemDetails newDetails = itemPrefab.GetComponent<ItemDetails>();
                    if (existingDetails.name == newDetails.name && existingDetails.chosenSize == newDetails.chosenSize)
                    {
                        existingDetails.quantity += newDetails.quantity;
                        exit();
                        return;
                    }
                }
                buyNowCart.buyNowItems.Add(Instantiate(itemPrefab, new Vector3(200, 200, 200), Quaternion.identity));
                exit();
                isLooking = true;
                summary.spawned = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.lockState = CursorLockMode.None;
                cartPanel.gameObject.SetActive(true);
                cartPanel.DOFade(1f, 0.2f);
                for (int i = 0; i < buyNowCart.buyNowItems.Count; i++)
                {
                    summary.spawnListings(buyNowCart.buyNowItems[i]);
                    refreshPrice(totalPriceText);
                }
                inMenu = true;
            }
            else
            {
                error.DOFade(1f, 0.2f);
            }
        }
    }

    public void refreshPrice(TextMeshProUGUI text)
    {
        float total = 0f;
        if (!boughtNow)
        {
            foreach (GameObject itemObj in cart.cartItems)
            {
                if (itemObj != null)
                {
                    ItemDetails d = itemObj.GetComponent<ItemDetails>();
                    total += d.chosenPrice * d.quantity;
                }
            }
        }
        else
        {
            foreach (GameObject itemObj in buyNowCart.buyNowItems)
            {
                if (itemObj != null)
                {
                    ItemDetails d = itemObj.GetComponent<ItemDetails>();
                    total += d.chosenPrice * d.quantity;
                }
            }
        }
        text.text = "$" + total.ToString("F2");   
    }

    public void refreshPriceShipping(TextMeshProUGUI text)
    {
        float total = 0f;
        if (!boughtNow)
        {
            foreach (GameObject itemObj in cart.cartItems)
            {
                if (itemObj != null)
                {
                    ItemDetails d = itemObj.GetComponent<ItemDetails>();
                    total += 2.95f * d.quantity;
                }
            }
        }
        else
        {
            foreach (GameObject itemObj in buyNowCart.buyNowItems)
            {
                if (itemObj != null)
                {
                    ItemDetails d = itemObj.GetComponent<ItemDetails>();
                    total += 2.95f * d.quantity;
                }
            }
        }
        text.text = "$" + total.ToString("F2");
    }

    public void refreshTotalPrice(TextMeshProUGUI text)
    {
        float total = 0f;
        if (!boughtNow)
        {
            foreach (GameObject itemObj in cart.cartItems)
            {
                if (itemObj != null)
                {
                    ItemDetails d = itemObj.GetComponent<ItemDetails>();
                    total += ((d.chosenPrice * d.quantity) + (2.95f * d.quantity));
                }
            }
        }
        else
        {
            foreach (GameObject itemObj in buyNowCart.buyNowItems)
            {
                if (itemObj != null)
                {
                    ItemDetails d = itemObj.GetComponent<ItemDetails>();
                    total += ((d.chosenPrice * d.quantity) + (2.95f * d.quantity));
                }
            }
        }
        text.text = "$" + total.ToString("F2");
    }
}

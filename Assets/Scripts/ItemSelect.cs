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
    public GameObject title;
    public GameObject subtitle;
    public GameObject price;

    [HideInInspector] public GameObject itemPrefab;
    [HideInInspector] public GameObject cartItemPrefab;

    [SerializeField] SizeSelector sizeSelector;
    [SerializeField] ShoppingCart cart;

    [SerializeField] CanvasGroup error;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.localPosition;
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
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
                        if (Input.GetMouseButtonDown(0))
                        {
                            Cursor.lockState = CursorLockMode.None;
                            isLooking = true;
                            cameraPosTransform = objTransform.parent.GetChild(objTransform.GetSiblingIndex() + 1);
                            cameraMovement = MoveCamera();
                            StartCoroutine(cameraMovement);

                            itemPrefab = Instantiate(item, new Vector3 (100, 100, 103), Quaternion.identity);

                            infoPanel.DOFade(1f, 0.2f);

                            sizeSelector.spawnButtons(itemPrefab);

                            ItemDetails details = item.GetComponent<ItemDetails>();
                            title.GetComponent<TextMeshProUGUI>().text = details.name;
                            subtitle.GetComponent<TextMeshProUGUI>().text = details.subtitle;
                            price.GetComponent<TextMeshProUGUI>().text = details.price;
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

    IEnumerator MoveCamera()
    {
        float time = 0;
        while (time < 1)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, cameraPosTransform.position, time);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.identity, time);
            time += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator RotateBack()
    {
        float time = 0;
        while (time < 1)
        {
            objTransform.rotation = Quaternion.Lerp(objTransform.rotation, Quaternion.identity, time);
            time += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator ReturnCamera()
    {
        float time = 0;
        while (time < 1)
        {
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, initialPos, time);
            time += Time.deltaTime;
            yield return null;
        }
    }

    public void exit()
    {
        if (isLooking)
        {
            isLooking = false;
            StopCoroutine(cameraMovement);
            StartCoroutine(RotateBack());
            StartCoroutine(ReturnCamera());

            infoPanel.DOFade(0f, 0.2f);

            Destroy(itemPrefab);
            for (int i = 0; i < sizeSelector.buttons.Count; i++)
            {
                if (sizeSelector.buttons[i] != null)
                {
                    Destroy(sizeSelector.buttons[i]);
                }
            }

            sizeSelector.buttons.Clear();

            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void addToCart()
    {
        if (itemPrefab != null)
        {
            if (itemPrefab.GetComponent<ItemDetails>().chosenSize != "")
            {
                cart.cartItems.Add(Instantiate(itemPrefab, new Vector3(200, 200, 200), Quaternion.identity));
                cart.addedToCartMsg();
                error.DOFade(0f, 0.2f);
                exit();
            }
            else
            {
                error.DOFade(1f, 0.2f);
            }
        }
    }
}

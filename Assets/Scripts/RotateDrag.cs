using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDrag : MonoBehaviour
{
    private Vector3 initialMousePosition;
    private Quaternion initialRotation;
    public float rotationSpeed = 0.5f;

    bool isLooking;
    // Start is called before the first frame update
    void Start()
    {
        isLooking = Camera.main.GetComponent<ItemSelect>().isLooking;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if (isLooking)
        {
            storeInitialMousePos();
        }
    }

    void OnMouseDrag()
    {
        if (isLooking)
        {
            clickAndDragRotation();
        }
    }

    void storeInitialMousePos()
    {
        initialMousePosition = Input.mousePosition;
        initialRotation = transform.rotation;
        Debug.Log("hii");
    }

    void clickAndDragRotation()
    {
        Vector3 mouseDelta = Input.mousePosition - initialMousePosition;

        float rotationX = mouseDelta.y * rotationSpeed;
        float rotationY = -mouseDelta.x * rotationSpeed;

        transform.rotation = initialRotation * Quaternion.Euler(rotationX, rotationY, 0);

        Debug.Log("hi");
    }
}

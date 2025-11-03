using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;

    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;

    private float xRotation = 0f;

    ItemSelect itemSelect;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        itemSelect = Camera.main.GetComponent<ItemSelect>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (itemSelect.isLooking != true)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            transform.Rotate(Vector3.up * mouseX);

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;

            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
    }
}

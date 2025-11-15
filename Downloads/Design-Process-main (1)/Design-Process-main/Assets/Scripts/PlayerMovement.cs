using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.5f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 2f;
    public Transform playerCamera;
    public float cameraClamp = 90f;

    [Header("Inactivity Settings")]
    public float inactivityTime = 3f; // seconds before hints appear
    public CanvasGroup hintCanvasGroup; // UI hints to fade in
    public float fadeDuration = 0.5f;

    private Vector3 lastPosition;
    private float idleTimer = 0f;
    private bool hintsVisible = false;

    [Header("References")]
    public ItemSelect itemSelect;
    public Tutorial tutorial;

    // Internal
    private CharacterController controller;
    private float xRotation = 0f;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (playerCamera == null)
            playerCamera = Camera.main.transform;

        if (itemSelect == null)
            itemSelect = Camera.main.GetComponent<ItemSelect>();
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        CheckInactivity();
    }

    private void HandleMouseLook()
    {
        if (itemSelect.isLooking || tutorial.inTutorial || itemSelect.summary.spawned)
            return;

        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        if (Mathf.Abs(mouseX) < 0.001f) mouseX = 0f;
        if (Mathf.Abs(mouseY) < 0.001f) mouseY = 0f;

        // Rotate player horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera vertically
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -cameraClamp, cameraClamp);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void HandleMovement()
    {
        if (itemSelect.isLooking || tutorial.inTutorial || itemSelect.summary.spawned)
            return;

        // Get input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        // Apply speed
        float speed = moveSpeed;
        //if (Input.GetKey(KeyCode.LeftShift))
        //    speed *= sprintMultiplier;

        // Ground check & gravity
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f; // small stick force

        // Jump
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Move
        controller.Move((move * speed + velocity) * Time.deltaTime);
    }

    private void CheckInactivity()
    {
        Vector3 horizontalMovement = transform.position;
        horizontalMovement.y = 0f; // ignore vertical movement

        if (Vector3.Distance(horizontalMovement, lastPosition) > 0.01f)
        {
            // Player moved
            idleTimer = 0f;
            if (hintsVisible)
                FadeOutHints();
        }
        else
        {
            // Player idle
            idleTimer += Time.deltaTime;
            if (idleTimer >= inactivityTime && !hintsVisible)
                FadeInHints();
        }

        lastPosition = horizontalMovement;
    }

    private void FadeInHints()
    {
        if (hintCanvasGroup == null) return;

        hintsVisible = true;
        hintCanvasGroup.DOKill(); // stop any ongoing tweens
        hintCanvasGroup.DOFade(1f, fadeDuration);
    }

    private void FadeOutHints()
    {
        if (hintCanvasGroup == null) return;

        hintsVisible = false;
        hintCanvasGroup.DOKill(); // stop any ongoing tweens
        hintCanvasGroup.DOFade(0f, fadeDuration);
    }
}
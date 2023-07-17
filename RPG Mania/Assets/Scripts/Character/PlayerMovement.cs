using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed = 5f;
    public float sprint = 1.8f;

    private InputActions actions;
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isSprinting;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        actions = new InputActions();
    }

    private void OnEnable()
    {
        actions.Gameplay.Enable();

        actions.Gameplay.Move.performed += context => moveInput = context.ReadValue<Vector2>();
        actions.Gameplay.Move.canceled += context => moveInput = Vector2.zero;
        actions.Gameplay.Sprint.performed += context => isSprinting = true;
        actions.Gameplay.Sprint.canceled += context => isSprinting = false;
    }

    private void OnDisable() {
        actions.Gameplay.Move.performed -= context => moveInput = context.ReadValue<Vector2>();
        actions.Gameplay.Move.canceled -= context => moveInput = Vector2.zero;
        actions.Gameplay.Sprint.performed -= context => isSprinting = true;
        actions.Gameplay.Sprint.canceled -= context => isSprinting = false;

        actions.Gameplay.Disable();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        float speedToUse = isSprinting ? speed * sprint : speed;

        rb.velocity = moveDirection * speedToUse;
    }
}
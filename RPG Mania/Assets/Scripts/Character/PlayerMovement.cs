using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    public float speed = 5f;
    public float sprint = 1.8f;

    private InputActions actions;
    private Rigidbody rb;
    private Animator anim;
    private SpriteRenderer sr;
    private Vector2 moveInput;
    private bool isSprinting;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        actions = new InputActions();
    }

    private void OnEnable()
    {
        actions.Gameplay.Enable();

        actions.Gameplay.Move.performed += MoveCharacter;
        actions.Gameplay.Move.canceled += StopCharacter;
        actions.Gameplay.Sprint.performed += context => isSprinting = true;
        actions.Gameplay.Sprint.canceled += context => isSprinting = false;
    }

    private void OnDisable() {
        actions.Gameplay.Move.performed -= MoveCharacter;
        actions.Gameplay.Move.canceled -= StopCharacter;
        actions.Gameplay.Sprint.performed -= context => isSprinting = true;
        actions.Gameplay.Sprint.canceled -= context => isSprinting = false;

        actions.Gameplay.Disable();

        rb.velocity = Vector3.zero;
    }

    private void MoveCharacter(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        anim.SetBool("Moving", true);

        sr.flipX = moveInput.x > 0;
    }
    private void StopCharacter(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
        anim.SetBool("Moving", false);
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
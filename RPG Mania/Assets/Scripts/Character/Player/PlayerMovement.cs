using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {
    public float speed = 5f;
    public float sprint = 1.8f;

    private InputActions actions;
    private Rigidbody rb;
    private Animator anim;
    private SpriteRenderer sr;
    private Vector2 moveInput;
    private bool isSprinting;

    private AudioSource audioSource;
    [SerializeField] private AudioClip walkSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        actions = new InputActions();
    }

    private void OnEnable()
    {
        actions.Gameplay.Enable();

        actions.Gameplay.Move.performed += MoveCharacter;
        actions.Gameplay.Move.canceled += StopCharacter;
        actions.Gameplay.Sprint.performed += context => isSprinting = true;
        actions.Gameplay.Sprint.canceled += context => isSprinting = false;

        PauseManager.pauseEvent += TogglePause;
    }

    private void OnDisable() {
        actions.Gameplay.Move.performed -= MoveCharacter;
        actions.Gameplay.Move.canceled -= StopCharacter;
        actions.Gameplay.Sprint.performed -= context => isSprinting = true;
        actions.Gameplay.Sprint.canceled -= context => isSprinting = false;

        PauseManager.pauseEvent -= TogglePause;

        actions.Gameplay.Disable();

        CancelMovement();
        
        rb.velocity = Vector3.zero;
    }

    private void MoveCharacter(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        anim.SetBool("Moving", true);

        if (moveInput.x > 0) sr.flipX = false;

        if (moveInput.x < 0) sr.flipX = true;
    }
    private void StopCharacter(InputAction.CallbackContext context)
    {
        CancelMovement();
    }

    private void CancelMovement()
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

    public void PlayMoveSound()
    {
        audioSource.clip = walkSound;
        audioSource.time = 1.88f;
        audioSource.Play();
    }

    private void TogglePause(bool pause)
    {
        if (pause)
            actions.Gameplay.Disable();
        else 
            actions.Gameplay.Enable();
    }
}
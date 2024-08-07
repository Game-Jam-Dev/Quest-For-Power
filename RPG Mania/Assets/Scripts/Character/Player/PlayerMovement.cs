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

    private string currentState;
    //Animation States
    const string PLAYER_IDLE = "Exploration_Idle";
    const string PLAYER_WALK_RIGHT = "Walk_Right";
    const string PLAYER_WALK_LEFT = "Walk_Left";
    const string PLAYER_WALK_UP = "Walk_Up";
    const string PLAYER_WALK_DOWN = "Walk_Down";

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
        
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        anim.Play(newState);

        currentState = newState;
    }

    private void MoveCharacter(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        //anim.SetBool("Moving", true);

        if (currentState == PLAYER_IDLE || moveInput.y == 0)
        {
            if (moveInput.x >= 0)
            {
                ChangeAnimationState(PLAYER_WALK_RIGHT);
            }

            if (moveInput.x < 0)
            {
                ChangeAnimationState(PLAYER_WALK_LEFT);
            }
        }

        if (currentState == PLAYER_IDLE || moveInput.x == 0)
        { 
            if (moveInput.y >= 0) 
            {
                ChangeAnimationState(PLAYER_WALK_UP);
            }

            if (moveInput.y <= 0)
            {
                ChangeAnimationState(PLAYER_WALK_DOWN);
            }
        }        
    }
    private void StopCharacter(InputAction.CallbackContext context)
    {
        CancelMovement();
    }

    private void CancelMovement()
    {
        rb.velocity = Vector3.zero;
        moveInput = Vector2.zero;
        ChangeAnimationState(PLAYER_IDLE);
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, moveInput.y, 0).normalized;

        float speedToUse = isSprinting ? speed * sprint : speed;
        Vector3 vel = new Vector3(moveDirection.x * speedToUse, moveDirection.y * speedToUse, 0);
        rb.velocity =  vel;
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
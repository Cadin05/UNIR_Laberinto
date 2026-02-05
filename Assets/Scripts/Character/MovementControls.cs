using UnityEngine;

public class MovementControls : MonoBehaviour
{
    public float movementSpeed = 10f;
    bool disableMovement = false;

    public float mouseSensitivity = 10f;
    float xRotation = 0f;

    [SerializeField] float audioStepInterval = 1f;
    float audioStepCounter = 1f;

    [SerializeField] float interactionRange = 2f;
    [SerializeField] LayerMask interactionLayer;
    public GameObject interactUI;

    BasicAttack basicAttack;

    public GameObject deathScreenUI;

    CharacterController cc;
    Animator animator;
    Camera cam;
    public Transform cmCam;
    AudioSource audioSource;
    [SerializeField] AudioClip[] stepClips;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        cam = Camera.main;

        basicAttack = GetComponent<BasicAttack>();
    }

    private void Start()
    {
        audioStepCounter = audioStepInterval;
        deathScreenUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (!disableMovement)
        {
            CharacterMovement();
        }
        else { animator.SetBool("Walking", false); }
        
        Interact();

        if (Input.GetButtonDown("Fire1"))
        {
            basicAttack.Attack();
        }
    }

    private void Interact()
    {
        interactUI.SetActive(false);

        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, out RaycastHit hit, interactionRange, interactionLayer))
        {
            Debug.Log("Hit a switch");
            IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();

            interactUI.SetActive(true);
            interactUI.GetComponent<RectTransform>().position = cam.WorldToScreenPoint(hit.collider.transform.position);

            if (Input.GetButtonDown("Interact"))
            {
                interactable.Interact();
            }
        }
    }

    private void CharacterMovement()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        transform.Rotate(Vector3.up, mouseX * mouseSensitivity);

        Vector3 movementDir = transform.TransformDirection(Vector3.forward) * yInput + transform.TransformDirection(Vector3.right) * xInput;

        xRotation -= mouseY * mouseSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        cmCam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        cc.Move(movementDir * Time.deltaTime * movementSpeed);

        // Handle walk animation
        if (movementDir.magnitude >= 0.1f)
        { animator.SetBool("Walking", true); }
        else
        { animator.SetBool("Walking", false); }

        PlayStepSounds();
    }

    private void PlayStepSounds()
    {
        if (cc.velocity.magnitude > 0.1f)
        {
            if (audioStepCounter <= 0)
            {
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.clip = stepClips[Random.Range(0, stepClips.Length)];
                audioSource.Play();
                audioStepCounter = audioStepInterval;
            }
            audioStepCounter -= Time.deltaTime;
        }
    }

    private void Die()
    {
        disableMovement = true;
        deathScreenUI.SetActive(true);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Goal"))
        {
            hit.gameObject.GetComponent<GoalBehaviour>().TouchedByPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lethal"))
        {
            Die();
        }
    }
}

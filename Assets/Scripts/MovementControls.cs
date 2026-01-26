using UnityEngine;

public class MovementControls : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public float movementSpeed = 10f;
    bool disableMovement = false;

    [SerializeField] float audioStepInterval = 1f;
    float audioStepCounter = 1f;

    [SerializeField] float interactionRange = 2f;
    [SerializeField] LayerMask interactionLayer;
    public GameObject interactUI;

    public GameObject deathScreenUI;

    CharacterController cc;
    Camera cam;
    AudioSource audioSource;
    [SerializeField] AudioClip[] stepClips;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        cam = Camera.main;
    }

    private void Start()
    {
        audioStepCounter = audioStepInterval;
        deathScreenUI.SetActive(false);
    }

    private void Update()
    {
        if (!disableMovement)
        {
            CharacterMovement();
        }
        Interact();
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
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        transform.Rotate(Vector3.up, xInput * Time.deltaTime * rotationSpeed);

        Vector3 movementDir = transform.TransformDirection(Vector3.forward) * yInput;

        cc.Move(movementDir * Time.deltaTime * movementSpeed);

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

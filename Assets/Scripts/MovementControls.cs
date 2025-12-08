using UnityEngine;

public class MovementControls : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public float movementSpeed = 10f;

    [SerializeField] float audioStepInterval = 1f;
    float audioStepCounter = 1f;

    CharacterController cc;
    AudioSource audioSource;
    [SerializeField] AudioClip[] stepClips;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioStepCounter = audioStepInterval;
    }

    private void Update()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        transform.Rotate(Vector3.up, xInput * Time.deltaTime * rotationSpeed);

        Vector3 movementDir = transform.TransformDirection(Vector3.forward) * yInput;

        cc.Move(movementDir * Time.deltaTime * movementSpeed);

        if (cc.velocity.magnitude > 0.1f) // SONIDO DE PASOS AL ANDAR
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Goal"))
        {
            hit.gameObject.GetComponent<GoalBehaviour>().TouchedByPlayer();
        }
    }
}

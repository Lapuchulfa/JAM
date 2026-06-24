using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 22f;
    public float acceleration = 100f;
    [Range(0f, 1f)]
    public float airControl = 0.5f;
    public float rotationTime = 0.15f;
    public float modelYawOffset = -90f;

    [Header("Salto")]
    public float jumpForce = 18f;
    [HideInInspector] public float fallMultiplier = 6f;
    [HideInInspector] public float lowJumpMultiplier = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.6f;
    public LayerMask groundMask;

    [Header("Sonidos")]
    public AudioClip jumpUpClip;
    public AudioClip jumpLandingClip;
    public AudioClip boingClip;
    public AudioClip[] footstepClips;
    [Range(0.1f, 1f)] public float footstepVolume = 0.5f;

    [Header("Animaciones")]
    public float landingAnimDuration = 0.3f;

    private Rigidbody rb;
    private Transform cameraTransform;
    private Animator animator;
    private AudioSource audioSource;
    private bool isGrounded;
    private bool wasGrounded;
    private bool jumpRequested;
    private bool isInAir;
    private bool wasInAir;
    private float turnCalVelocity;
    private Vector3 inputDirection;
    private float lastFootstepTime;
    private float footstepInterval = 0.3f;

    void Awake()
    {
        // Forzar valores de gravedad aquí para que nunca quede el valor viejo del Inspector
        fallMultiplier = 6f;
        lowJumpMultiplier = 3f;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D: sin desfase por posición 3D

        // Gravedad manual total: desactiva la de Unity para controlarla nosotros
        rb.useGravity = false;

        if (Camera.main != null)
            cameraTransform = Camera.main.transform;

        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Update()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Transición de caída a aterrizaje
        if (!wasGrounded && isGrounded)
        {
            PlayClip(jumpLandingClip);
            if (animator != null)
            {
                animator.SetTrigger("Land");
                animator.SetBool("isGrounded", true);
            }
        }
        else if (animator != null)
            animator.SetBool("isGrounded", isGrounded);

        // Detección de estado en aire
        wasInAir = isInAir;
        isInAir = !isGrounded;

        if (!wasInAir && isInAir && animator != null)
        {
            animator.SetTrigger("Jump");
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical   = Input.GetAxisRaw("Vertical");
        inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // Salto con protección mejorada
        if (Input.GetButtonDown("Jump") && isGrounded && !jumpRequested)
        {
            jumpRequested = true;
            PlayClip(jumpUpClip);
        }

        // Sonidos de pasos
        if (isGrounded && inputDirection.sqrMagnitude >= 0.01f)
        {
            lastFootstepTime += Time.deltaTime;
            if (lastFootstepTime >= footstepInterval)
            {
                PlayFootstep();
                lastFootstepTime = 0f;
            }
        }
        else
        {
            lastFootstepTime = 0f;
        }
    }

    void FixedUpdate()
    {
        if (jumpRequested)
        {
            jumpRequested = false;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }

        // Movimiento horizontal relativo a la cámara
        Vector3 targetVelocity = Vector3.zero;
        float currentSpeed = 0f;

        if (inputDirection.sqrMagnitude >= 0.01f && cameraTransform != null)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle + modelYawOffset, ref turnCalVelocity, rotationTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            targetVelocity = moveDirection * moveSpeed;
            currentSpeed = moveSpeed;
        }

        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        float control = isGrounded ? 1f : airControl;
        Vector3 newVelocity = Vector3.MoveTowards(horizontalVelocity, targetVelocity, acceleration * control * Time.fixedDeltaTime);
        rb.AddForce(newVelocity - horizontalVelocity, ForceMode.VelocityChange);

        // Gravedad manual: multiplicador total (no extra), caída pesada y subida corta
        if (rb.linearVelocity.y < 0f)
            rb.AddForce(Physics.gravity * fallMultiplier, ForceMode.Acceleration);
        else
            rb.AddForce(Physics.gravity * (rb.linearVelocity.y > 0f ? lowJumpMultiplier : 1f), ForceMode.Acceleration);

        if (animator != null)
            animator.SetFloat("Speed", currentSpeed);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boing"))
            PlayClip(boingClip);
    }

    void PlayFootstep()
    {
        if (footstepClips == null || footstepClips.Length == 0) return;
        if (audioSource == null) return;

        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
        audioSource.PlayOneShot(clip, footstepVolume);
    }

    public void PlayClip(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }

    public void PlayClip(AudioClip clip, float volume)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip, volume);
    }
}

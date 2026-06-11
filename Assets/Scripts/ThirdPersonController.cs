using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 6f;       // Reducido un poco para mayor control inicial
    public float rotationTime = 0.15f;  // Tiempo que tarda en orientarse (más bajo = más rápido)

    [Header("Salto")]
    public float jumpForce = 5f;
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    private Rigidbody rb;
    private Transform cameraTransform;
    private Animator animator;
    private bool isGrounded;
    private float turnCalVelocity;     // Variable interna para el suavizado de rotación

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        // Mantener al personaje vertical y evitar que ruede por colisiones
        rb.freezeRotation = true;
    }

    void Update()
    {
        // 1. Detección de suelo
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (animator != null)
        {
            animator.SetBool("isGrounded", isGrounded);
        }

        // 2. Input de Salto (Se lee en Update para no perder la pulsación de la tecla)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Reseteamos la velocidad vertical antes del salto para que siempre suba con la misma fuerza
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // 3. Inputs de movimiento (WASD)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        float currentSpeed = 0f;

        if (direction.magnitude >= 0.1f)
        {
            // Calcular el ángulo hacia el que debe mirar el personaje según la cámara
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

            // Suavizar la rotación para que no gire de golpe
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalVelocity, rotationTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Calcular la dirección final del movimiento físico
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Aplicar velocidad al Rigidbody respetando la gravedad actual (rb.linearVelocity.y)
            rb.linearVelocity = new Vector3(moveDirection.normalized.x * moveSpeed, rb.linearVelocity.y, moveDirection.normalized.z * moveSpeed);

            currentSpeed = moveSpeed;
        }
        else
        {
            // Frenado inmediato horizontal al soltar las teclas para evitar deslizamientos estilo jabón
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }

        // 4. Enviar velocidad al Animator para las transiciones de Idle/Walk
        if (animator != null)
        {
            animator.SetFloat("Speed", currentSpeed);
        }
    }
}
using UnityEngine;

public class PlayerJuiciness : MonoBehaviour
{
    [Header("Squash & Stretch")]
    public float squashAmount = 0.7f;
    public float stretchAmount = 1.15f;
    public float squashDuration = 0.12f;
    public float stretchDuration = 0.08f;

    [Header("Bob al caminar")]
    public float bobSpeed = 8f;
    public float bobAmount = 0.02f;

    private Vector3 originalScale;
    private Vector3 originalPosition;
    private float squashTimer;
    private float stretchTimer;
    private Rigidbody rb;
    private bool wasGrounded;
    private ThirdPersonController controller;
    private float bobTimer;

    void Start()
    {
        originalScale = transform.localScale;
        originalPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<ThirdPersonController>();
    }

    void Update()
    {
        if (rb == null) return;

        bool isGrounded = rb.linearVelocity.y <= 0.1f;

        // Squash al aterrizar
        if (isGrounded && !wasGrounded && rb.linearVelocity.y < -5f)
        {
            squashTimer = squashDuration;
            stretchTimer = 0;
        }

        // Stretch al saltar
        if (!isGrounded && wasGrounded)
        {
            stretchTimer = stretchDuration;
            squashTimer = 0;
        }

        // Aplicar squash/stretch
        float scale = 1f;
        if (squashTimer > 0)
        {
            squashTimer -= Time.deltaTime;
            float squashProgress = 1f - (squashTimer / squashDuration);
            scale = Mathf.Lerp(squashAmount, 1f, squashProgress);
        }
        else if (stretchTimer > 0)
        {
            stretchTimer -= Time.deltaTime;
            float stretchProgress = 1f - (stretchTimer / stretchDuration);
            scale = Mathf.Lerp(stretchAmount, 1f, stretchProgress);
        }
        else if (isGrounded && controller != null)
        {
            // Bob suave al caminar (más pronunciado al correr)
            float moveSpeed = rb.linearVelocity.magnitude;
            if (moveSpeed > 0.5f)
            {
                bobTimer += Time.deltaTime * bobSpeed;
                float speedFactor = moveSpeed / 18f;
                float bobIntensity = bobAmount * (1f + speedFactor); // Más bob al correr
                scale = 1f + Mathf.Sin(bobTimer) * bobIntensity;
            }
            else
            {
                bobTimer = 0;
            }
        }

        transform.localScale = new Vector3(originalScale.x * scale, originalScale.y * scale, originalScale.z * scale);
        wasGrounded = isGrounded;
    }
}

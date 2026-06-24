using UnityEngine;

public class PlayerJuiciness : MonoBehaviour
{
    [Header("Squash & Stretch")]
    public float squashAmount = 0.8f;
    public float stretchAmount = 1.2f;
    public float squashDuration = 0.1f;

    private Vector3 originalScale;
    private float squashTimer;
    private Rigidbody rb;
    private bool wasGrounded;

    void Start()
    {
        originalScale = transform.localScale;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb == null) return;

        bool isGrounded = rb.linearVelocity.y <= 0.1f;

        // Squash al aterrizar
        if (isGrounded && !wasGrounded && rb.linearVelocity.y < -5f)
        {
            squashTimer = squashDuration;
        }

        // Aplicar squash
        if (squashTimer > 0)
        {
            squashTimer -= Time.deltaTime;
            float squashProgress = 1f - (squashTimer / squashDuration);
            float scale = Mathf.Lerp(squashAmount, 1f, squashProgress);
            transform.localScale = new Vector3(originalScale.x * scale, originalScale.y * scale, originalScale.z * scale);
        }
        else
        {
            transform.localScale = originalScale;
        }

        wasGrounded = isGrounded;
    }
}

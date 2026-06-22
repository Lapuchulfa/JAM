using UnityEngine;

public class PlayerMovementOptimizer : MonoBehaviour
{
    [Header("Optimizaciones de Movimiento")]
    [Range(0.01f, 0.5f)] public float coyoteTime = 0.1f;
    [Range(0f, 0.3f)] public float jumpBuffer = 0.05f;

    [Header("Feedback Visual")]
    [Range(0.8f, 1.2f)] public float jumpScaleBoost = 1.05f;
    [Range(0.05f, 0.3f)] public float jumpScaleDuration = 0.1f;

    private Rigidbody rb;
    private ThirdPersonController controller;
    private float lastGroundedTime;
    private float jumpBufferTime;
    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<ThirdPersonController>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Mantener track de cuándo fue la última vez en tierra
        if (IsGrounded())
            lastGroundedTime = Time.time;

        // Buffer de salto (permite presionar salto un poco antes de aterrizar)
        if (Input.GetButtonDown("Jump"))
            jumpBufferTime = Time.time;
    }

    void FixedUpdate()
    {
        // Coyote time: permite saltar aunque haya dejado la plataforma hace poco
        if (Time.time - lastGroundedTime < coyoteTime && Time.time - jumpBufferTime < jumpBuffer)
        {
            ExecuteBufferedJump();
        }
    }

    void ExecuteBufferedJump()
    {
        if (rb != null)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * (controller != null ? controller.jumpForce : 12f), ForceMode.VelocityChange);
            jumpBufferTime = -1f; // Consumir el buffer

            // Feedback visual de salto
            StartCoroutine(JumpScaleFeedback());
        }
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(controller.groundCheck.position, controller.groundDistance, controller.groundMask);
    }

    System.Collections.IEnumerator JumpScaleFeedback()
    {
        float elapsed = 0f;

        // Expandir al saltar
        while (elapsed < jumpScaleDuration)
        {
            elapsed += Time.deltaTime;
            float scale = Mathf.Lerp(1f, jumpScaleBoost, elapsed / jumpScaleDuration);
            transform.localScale = originalScale * scale;
            yield return null;
        }

        // Volver al tamaño normal
        elapsed = 0f;
        while (elapsed < jumpScaleDuration * 0.5f)
        {
            elapsed += Time.deltaTime;
            float scale = Mathf.Lerp(jumpScaleBoost, 1f, elapsed / (jumpScaleDuration * 0.5f));
            transform.localScale = originalScale * scale;
            yield return null;
        }

        transform.localScale = originalScale;
    }
}

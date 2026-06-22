using System.Collections;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Limites")]
    public float killY = -15f;
    public float fallBelowCheckpoint = 60f;

    [Header("Juiciness")]
    public AudioClip respawnClip;
    public AudioClip checkpointClip;
    [Range(0.05f, 0.3f)] public float shakeAmount = 0.12f;
    [Range(0.1f, 0.5f)]  public float shakeDuration = 0.2f;
    [Range(0.05f, 0.15f)] public float respawnPauseDuration = 0.1f;

    private Vector3 respawnPoint;
    private Rigidbody rb;
    private ThirdPersonController controller;
    private Vector3 originalScale;
    private bool isRespawning;
    private Coroutine scalePunchRoutine;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<ThirdPersonController>();
        originalScale = transform.localScale;
        respawnPoint = transform.position;
    }

    public void SetCheckpoint(Vector3 point)
    {
        respawnPoint = point;
        // Feedback visual: pequeño destello de escala al registrar checkpoint
        if (scalePunchRoutine != null) StopCoroutine(scalePunchRoutine);
        scalePunchRoutine = StartCoroutine(ScalePunch(1.15f, 0.1f));

        // Sonido de checkpoint
        ThirdPersonController ctrl = GetComponent<ThirdPersonController>();
        if (checkpointClip != null && ctrl != null)
            ctrl.PlayClip(checkpointClip, 0.6f);
    }

    void Update()
    {
        if (isRespawning) return;

        bool fellOff = transform.position.y < killY ||
                       transform.position.y < respawnPoint.y - fallBelowCheckpoint;

        if (fellOff || Input.GetKeyDown(KeyCode.R))
            StartCoroutine(DoRespawn());
    }

    IEnumerator DoRespawn()
    {
        isRespawning = true;

        // 1. Squish hacia abajo antes de teletransportar
        yield return StartCoroutine(ScalePunch(0.4f, 0.08f));

        // Pausa de respawn para efecto dramático
        yield return new WaitForSeconds(respawnPauseDuration);

        // 2. Teletransporte y reseteo de velocidad
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.position = respawnPoint;
        transform.position = respawnPoint;

        // 3. Sonido
        if (respawnClip != null && controller != null)
            controller.PlayClip(respawnClip);

        // 4. Scale pop: crece rápido y vuelve al tamaño normal
        yield return StartCoroutine(ScalePop());

        // 5. Shake de cámara
        StartCoroutine(ShakeCamera());

        isRespawning = false;
    }

    IEnumerator ScalePunch(float targetScale, float duration)
    {
        float t = 0f;
        Vector3 start = transform.localScale;
        Vector3 target = originalScale * targetScale;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.localScale = Vector3.Lerp(start, target, t);
            yield return null;
        }
        // Volver al tamaño original
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.localScale = Vector3.Lerp(target, originalScale, t);
            yield return null;
        }
        transform.localScale = originalScale;
    }

    IEnumerator ScalePop()
    {
        // Aparece pequeño y crece con overshoot
        float[] keys = { 0f, 1.4f, 0.85f, 1.1f, 1f };
        float step = 0.07f;
        foreach (float s in keys)
        {
            float t = 0f;
            Vector3 from = transform.localScale;
            Vector3 to = originalScale * s;
            while (t < 1f)
            {
                t += Time.deltaTime / step;
                transform.localScale = Vector3.Lerp(from, to, t);
                yield return null;
            }
        }
        transform.localScale = originalScale;
    }

    IEnumerator ShakeCamera()
    {
        if (Camera.main == null) yield break;
        Transform cam = Camera.main.transform;
        Vector3 origin = cam.localPosition;
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float strength = Mathf.Lerp(shakeAmount, 0f, elapsed / shakeDuration);
            cam.localPosition = origin + Random.insideUnitSphere * strength;
            elapsed += Time.deltaTime;
            yield return null;
        }
        cam.localPosition = origin;
    }
}

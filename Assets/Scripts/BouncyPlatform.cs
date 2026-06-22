using UnityEngine;

public class BouncyPlatform : MonoBehaviour
{
    [Header("Sonidos")]
    public AudioClip boingClip;
    [Range(0.3f, 1f)] public float boingVolume = 0.8f;

    [Header("Mecánica")]
    public float bouncePower = 3.5f;
    public float bounceScale = 1.1f;
    public float bounceDuration = 0.15f;

    void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (rb == null || rb.isKinematic) return;

        if (collision.transform.position.y > transform.position.y)
        {
            ThirdPersonController ctrl = collision.collider.GetComponentInParent<ThirdPersonController>();
            float launchVelocity = ctrl != null ? ctrl.jumpForce * bouncePower : 42f;

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, launchVelocity, rb.linearVelocity.z);

            // Sonido con volumen controlado
            if (boingClip != null)
            {
                Vector3 soundPos = collision.contacts.Length > 0 ? collision.contacts[0].point : transform.position;
                AudioSource.PlayClipAtPoint(boingClip, soundPos, boingVolume);
            }

            // Feedback visual de rebote
            StartCoroutine(BounceFeedback());
        }
    }

    System.Collections.IEnumerator BounceFeedback()
    {
        Vector3 originalScale = transform.localScale;
        float t = 0f;

        // Compresión
        while (t < 0.5f)
        {
            t += Time.deltaTime / bounceDuration;
            float scale = Mathf.Lerp(1f, 0.9f, t * 2f);
            transform.localScale = originalScale * scale;
            yield return null;
        }

        // Expansión
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / bounceDuration;
            float scale = Mathf.Lerp(0.9f, 1f, t);
            transform.localScale = originalScale * scale;
            yield return null;
        }

        transform.localScale = originalScale;
    }
}

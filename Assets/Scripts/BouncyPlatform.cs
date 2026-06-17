using UnityEngine;

// Plataforma roja: lanza al jugador con el doble de altura de salto (velocidad × √2).
public class BouncyPlatform : MonoBehaviour
{
    public AudioClip boingClip;

    void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (rb == null || rb.isKinematic) return;

        if (collision.transform.position.y > transform.position.y)
        {
            ThirdPersonController ctrl = collision.collider.GetComponentInParent<ThirdPersonController>();
            // √2 ≈ 1.4142 duplica la altura (h ∝ v²)
            // Con gravedad manual ×6 al caer, necesita más impulso para sentirse como "doble altura"
            float launchVelocity = ctrl != null ? ctrl.jumpForce * 3.5f : 42f;

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, launchVelocity, rb.linearVelocity.z);

            if (boingClip != null)
                AudioSource.PlayClipAtPoint(boingClip, collision.contacts[0].point);
        }
    }
}

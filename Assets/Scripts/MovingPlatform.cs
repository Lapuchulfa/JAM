using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingPlatform : MonoBehaviour
{
    public Vector3 moveOffset = new Vector3(10f, 0f, 0f);
    public float speed = 5f;
    public float waitTime = 0.5f;

    private Rigidbody rb;
    private Vector3 startPos;
    private float progress;
    private int direction = 1;
    private float waitTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        startPos = rb.position;
    }

    void FixedUpdate()
    {
        if (waitTimer > 0f) { waitTimer -= Time.fixedDeltaTime; return; }

        float distance = moveOffset.magnitude;
        if (distance < 0.01f) return;

        progress += direction * (speed / distance) * Time.fixedDeltaTime;

        if (progress >= 1f) { progress = 1f; direction = -1; waitTimer = waitTime; }
        else if (progress <= 0f) { progress = 0f; direction =  1; waitTimer = waitTime; }

        Vector3 next = startPos + moveOffset * Mathf.SmoothStep(0f, 1f, progress);
        rb.MovePosition(next);
    }

    // Cuando el jugador cae encima, se convierte en hijo de la plataforma.
    // Unity mueve al hijo junto con el padre automáticamente, sin conflictos de física.
    void OnCollisionEnter(Collision collision)
    {
        if (!IsOnTop(collision)) return;
        collision.transform.SetParent(transform);
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.transform.parent == transform)
            collision.transform.SetParent(null);
    }

    bool IsOnTop(Collision collision)
    {
        foreach (ContactPoint cp in collision.contacts)
            if (cp.normal.y > 0.5f) return true;
        return false;
    }
}

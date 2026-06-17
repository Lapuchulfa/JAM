using UnityEngine;

public class CheckpointPlatform : MonoBehaviour
{
    public float spawnHeight = 1.5f;

    private float cooldown = 0f;
    private const float COOLDOWN_TIME = 1f;

    void Update()
    {
        if (cooldown > 0f) cooldown -= Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (cooldown > 0f) return;

        PlayerRespawn player = collision.collider.GetComponentInParent<PlayerRespawn>();
        if (player == null) return;

        Vector3 checkpoint = transform.position + Vector3.up * (transform.localScale.y * 0.5f + spawnHeight);
        player.SetCheckpoint(checkpoint);
        cooldown = COOLDOWN_TIME;
    }
}

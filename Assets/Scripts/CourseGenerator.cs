using System.Collections.Generic;
using UnityEngine;

// Genera segmentos de plataformas de manera procedural e infinita hacia -Z.
// Coloca este objeto donde debe aparecer la ENTRADA del primer segmento.
public class CourseGenerator : MonoBehaviour
{
    public GameObject[] segmentPrefabs;  // Los 3 segmentos (o más)
    public Transform player;             // El MC
    public float spawnAhead = 250f;      // Genera segmentos cuando el jugador está a esta distancia del final
    public float destroyBehind = 350f;   // Destruye segmentos que quedan tan atrás

    private Vector3 nextSpawnPos;
    private readonly List<GameObject> segments = new List<GameObject>();
    private int lastIndex = -1;

    void Start()
    {
        nextSpawnPos = transform.position;
        // Dos segmentos iniciales para que siempre haya camino visible
        SpawnNext();
        SpawnNext();
    }

    void Update()
    {
        if (player == null || segmentPrefabs == null || segmentPrefabs.Length == 0) return;

        // El recorrido avanza hacia -Z: generar mientras el final esté cerca del jugador
        int safety = 0;
        while (nextSpawnPos.z > player.position.z - spawnAhead && safety++ < 10)
        {
            SpawnNext();
        }

        // Limpiar segmentos muy atrás (el respawn siempre apunta al segmento actual, nunca a uno destruido)
        for (int i = segments.Count - 1; i >= 0; i--)
        {
            if (segments[i] == null)
            {
                segments.RemoveAt(i);
            }
            else if (segments[i].transform.position.z > player.position.z + destroyBehind)
            {
                Destroy(segments[i]);
                segments.RemoveAt(i);
            }
        }
    }

    void SpawnNext()
    {
        // Elegir un segmento al azar evitando repetir el anterior
        int index = Random.Range(0, segmentPrefabs.Length);
        if (segmentPrefabs.Length > 1 && index == lastIndex)
        {
            index = (index + 1) % segmentPrefabs.Length;
        }
        lastIndex = index;

        GameObject seg = Instantiate(segmentPrefabs[index], nextSpawnPos, Quaternion.identity);
        segments.Add(seg);

        CourseSegment info = seg.GetComponent<CourseSegment>();
        nextSpawnPos += info != null ? info.exitOffset : new Vector3(0f, 0f, -120f);
    }
}

using UnityEngine;

// Va en la raíz de cada prefab de segmento: define dónde empieza el siguiente segmento.
public class CourseSegment : MonoBehaviour
{
    public Vector3 exitOffset = new Vector3(0f, 30f, -120f); // Desde la entrada de este segmento hasta la entrada del siguiente
}

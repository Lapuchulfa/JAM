using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Referencias")]
    public Transform target; // Arrastra aquí a tu personaje "MC"

    [Header("Configuración")]
    public float mouseSensitivity = 200f;
    public float distance = 5f; // Distancia detrás del personaje
    public float height = 2f;     // Altura respecto al personaje

    private float mouseX;
    private float mouseY;

    void Start()
    {
        // Bloquear el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Capturar el movimiento del mouse
        mouseX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Limitar la rotación vertical para que no de la vuelta completa
        mouseY = Mathf.Clamp(mouseY, -20f, 60f);

        // 2. Calcular la rotación y posición de la cámara
        Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0f);
        Vector3 targetPosition = target.position - (rotation * Vector3.forward * distance) + (Vector3.up * height);

        // 3. Aplicar a la cámara
        transform.rotation = rotation;
        transform.position = targetPosition;
    }
}
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Rychlost a plynulost")]
    public float moveSpeed = 20f;
    public float smoothness = 8f;

    [Header("Posun držením koleèka myši (HoI4 Drag)")]
    public float dragSpeed = 25f;

    [Header("Limity pohybu po mapì (Clamp)")]
    public float minX = -30f;
    public float maxX = 30f;
    public float minZ = -20f;
    public float maxZ = 20f;

    [Header("Nastavení zoomování")]
    public float zoomSpeed = 15f;
    public float minHeight = 5f;
    public float maxHeight = 35f;

    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        // 1. Posun klávesami WASD / šipkami
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 moveInput = new Vector3(x, 0, z).normalized;
        targetPosition += moveInput * moveSpeed * Time.deltaTime;

        // 2. Posun držením koleèka myši (Prostøední tlaèítko - index 2)
        if (Input.GetMouseButton(2))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Pøizpùsobení citlivosti podle výšky kamery (pøi vìtším oddálení se mapa hýbe odpovídající rychlostí)
            float heightMultiplier = targetPosition.y / minHeight;

            Vector3 dragMove = new Vector3(-mouseX, 0, -mouseY) * dragSpeed * heightMultiplier * Time.deltaTime;
            targetPosition += dragMove;
        }

        // Limity pohybu v hranicích mapy
        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ);

        // 3. Zoomování koleèkem myši
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            targetPosition.y -= scroll * zoomSpeed * 10f;
            targetPosition.y = Mathf.Clamp(targetPosition.y, minHeight, maxHeight);
        }

        // Plynulá interpolace pohybu kamery
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothness);
    }
}
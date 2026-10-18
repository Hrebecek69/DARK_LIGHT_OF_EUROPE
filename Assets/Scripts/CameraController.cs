using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float zoomSpeed = 10f;
    public float minHeight = 5f;
    public float maxHeight = 30f;

    void Update()
    {
        // Pohyb klávesami WASD nebo šipkami
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(x, 0, z) * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);

        // Zoomování koleèkem myši
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 pos = transform.position;
        pos.y -= scroll * zoomSpeed * 100f * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minHeight, maxHeight);
        transform.position = pos;
    }
}
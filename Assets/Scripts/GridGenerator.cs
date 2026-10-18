using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int width = 15;
    public int height = 10;
    public GameObject tilePrefab;

    private float xOffset = 1.732f;
    private float zOffset = 1.5f;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                float xPos = x * xOffset;
                if (z % 2 == 1)
                {
                    xPos += xOffset / 2f;
                }

                Vector3 position = new Vector3(xPos, 0, z * zOffset);
                Instantiate(tilePrefab, position, Quaternion.identity, transform);
            }
        }
    }
}
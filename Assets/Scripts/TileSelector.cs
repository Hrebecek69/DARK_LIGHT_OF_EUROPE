using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public Material hoverMaterial;   // Barva pøi najetí myší
    public Material clickedMaterial; // Barva po kliknutí

    private Transform lastHoveredTile;
    private Material originalMaterial;

    void Update()
    {
        // Vystøelení paprsku z kamery na pozici kurzoru myši
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Transform currentTile = hit.transform;

            // Pokud míøíme na novou dlaždici
            if (lastHoveredTile != currentTile)
            {
                ResetLastTile();

                // Uložíme si pùvodní materiál dlaždice
                lastHoveredTile = currentTile;
                MeshRenderer renderer = currentTile.GetComponent<MeshRenderer>();

                if (renderer != null)
                {
                    originalMaterial = renderer.material;
                    if (hoverMaterial != null)
                    {
                        renderer.material = hoverMaterial; // Zmìna barvy pøi najetí
                    }
                }
            }

            // Kliknutí levým tlaèítkem myši
            if (Input.GetMouseButtonDown(0))
            {
                MeshRenderer renderer = currentTile.GetComponent<MeshRenderer>();
                if (renderer != null && clickedMaterial != null)
                {
                    originalMaterial = clickedMaterial; // Trvalá zmìna barvy
                    renderer.material = clickedMaterial;
                }
            }
        }
        else
        {
            ResetLastTile();
        }
    }

    void ResetLastTile()
    {
        if (lastHoveredTile != null)
        {
            MeshRenderer renderer = lastHoveredTile.GetComponent<MeshRenderer>();
            if (renderer != null && originalMaterial != null)
            {
                renderer.material = originalMaterial;
            }
            lastHoveredTile = null;
        }
    }
}
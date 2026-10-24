using UnityEngine;

public class TileSelector : MonoBehaviour
{
    [Header("Materiály pro kurzor a výbìr")]
    public Material hoverMaterial;   // Barva pøi najetí myší
    public Material clickedMaterial; // Barva vybrané dlaždice

    private Transform hoverTile;
    private Material hoverOriginalMat;

    private Transform selectedTile;
    private Material selectedOriginalMat;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hitSomething = Physics.Raycast(ray, out hit);

        // 1. Odkliknutí pravým tlaèítkem myši nebo klávesou ESC
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            DeselectCurrentTile();
        }

        if (hitSomething)
        {
            Transform currentTile = hit.transform;

            // --- HOVER EFEKT (najetí myší) ---
            if (hoverTile != currentTile)
            {
                ResetHover();

                // Zvýrazòujeme pouze dlaždici, která není zrovna vybraná
                if (currentTile != selectedTile)
                {
                    hoverTile = currentTile;
                    MeshRenderer renderer = currentTile.GetComponent<MeshRenderer>();
                    if (renderer != null)
                    {
                        hoverOriginalMat = renderer.material;
                        if (hoverMaterial != null)
                        {
                            renderer.material = hoverMaterial;
                        }
                    }
                }
            }

            // --- KLIKNUTÍ LEVÝM TLAÈÍTKEM (Výbìr / Odkliknutí) ---
            if (Input.GetMouseButtonDown(0))
            {
                MeshRenderer renderer = currentTile.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    // A) Kliknutí na UŽ VYBRANOU dlaždici -> ODKLIKNE JI
                    if (selectedTile == currentTile)
                    {
                        DeselectCurrentTile();
                    }
                    // B) Kliknutí na NOVOU dlaždici -> Odklikne starou a vybere novou
                    else
                    {
                        DeselectCurrentTile();

                        selectedTile = currentTile;
                        selectedOriginalMat = (hoverTile == currentTile && hoverOriginalMat != null)
                            ? hoverOriginalMat
                            : renderer.material;

                        if (clickedMaterial != null)
                        {
                            renderer.material = clickedMaterial;
                        }

                        hoverTile = null;
                    }
                }
            }
        }
        else
        {
            ResetHover();

            // 2. Kliknutí do prázdného prostoru mimo mapu odklikne výbìr
            if (Input.GetMouseButtonDown(0))
            {
                DeselectCurrentTile();
            }
        }
    }

    void ResetHover()
    {
        if (hoverTile != null)
        {
            if (hoverTile != selectedTile)
            {
                MeshRenderer renderer = hoverTile.GetComponent<MeshRenderer>();
                if (renderer != null && hoverOriginalMat != null)
                {
                    renderer.material = hoverOriginalMat;
                }
            }
            hoverTile = null;
            hoverOriginalMat = null;
        }
    }

    // Metoda pro navrácení vybrané dlaždice do pùvodního stavu
    public void DeselectCurrentTile()
    {
        if (selectedTile != null)
        {
            MeshRenderer renderer = selectedTile.GetComponent<MeshRenderer>();
            if (renderer != null && selectedOriginalMat != null)
            {
                renderer.material = selectedOriginalMat;
            }
            selectedTile = null;
            selectedOriginalMat = null;
        }
    }
}
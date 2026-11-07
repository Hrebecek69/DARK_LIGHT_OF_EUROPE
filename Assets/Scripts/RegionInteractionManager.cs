
using UnityEngine;

public class RegionInteractionManager : MonoBehaviour
{
    [Header("Kamera")]
    public Camera mapCamera;

    [Header("Barvy")]
    public Color hoverColor = new Color(0.2f, 0.5f, 0.9f);
    public Color selectedRegionColor = Color.yellow;
    public Color selectedCountryColor = new Color(1f, 0.7f, 0.1f);

    [Header("Nastavení Raycastu")]
    public float maxDistance = 1000f;

    private RegionData[] allRegions;

    private RegionData hoveredRegion;
    private RegionData selectedRegion;
    private CountryData selectedCountry;

    void Start()
    {
        if (mapCamera == null)
            mapCamera = Camera.main;

        allRegions = FindObjectsByType<RegionData>(FindObjectsSortMode.None);

        UpdateAllColors();

        Debug.Log("Region Interaction Manager pøipraven. Regionù: " + allRegions.Length);
    }

    void Update()
    {
        if (mapCamera == null)
            return;

        RegionData currentRegion = GetRegionUnderMouse();

        if (currentRegion != hoveredRegion)
        {
            hoveredRegion = currentRegion;
            UpdateAllColors();
        }

        // LEVÉ TLAÈÍTKO = výbìr provincie
        if (Input.GetMouseButtonDown(0))
        {
            selectedRegion = currentRegion;
            selectedCountry = null;

            UpdateAllColors();

            if (selectedRegion != null)
                Debug.Log("Vybraná provincie: " + selectedRegion.regionName);
            else
                Debug.Log("Výbìr zrušen.");
        }

        // PRAVÉ TLAÈÍTKO = výbìr celého státu
        if (Input.GetMouseButtonDown(1))
        {
            selectedRegion = null;

            if (currentRegion != null)
            {
                selectedCountry = currentRegion.ownerCountry;

                if (selectedCountry != null)
                    Debug.Log("Vybraný stát: " + selectedCountry.countryName);
                else
                    Debug.LogWarning("Tento region nemá pøiøazený stát!");
            }
            else
            {
                selectedCountry = null;
                Debug.Log("Výbìr státu zrušen.");
            }

            UpdateAllColors();
        }
    }

    RegionData GetRegionUnderMouse()
    {
        Ray ray = mapCamera.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);

        RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance);

        Debug.Log("POÈET ZÁSAHÙ: " + hits.Length);

        foreach (RaycastHit h in hits)
        {
            Debug.Log(
                "ZASAŽENÝ OBJEKT: " + h.collider.name +
                " | POZICE: " + h.point +
                " | VZDÁLENOST: " + h.distance
            );

            RegionData region = h.collider.GetComponentInParent<RegionData>();

            if (region != null)
            {
                Debug.Log("NALEZEN REGION: " + region.regionName);
                return region;
            }
        }

        return null;
    }

    void UpdateAllColors()
    {
        if (allRegions == null)
            return;

        foreach (RegionData region in allRegions)
        {
            if (region == null)
                continue;

            if (region == selectedRegion)
                region.SetColor(selectedRegionColor);

            else if (selectedCountry != null &&
                     region.ownerCountry == selectedCountry)
                region.SetColor(selectedCountryColor);

            else if (region == hoveredRegion)
                region.SetColor(hoverColor);

            else
                region.SetColor(region.GetNormalColor());
        }
    }
}

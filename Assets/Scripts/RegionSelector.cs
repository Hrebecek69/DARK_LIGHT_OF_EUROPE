using UnityEngine;
using System.Collections.Generic;

public class RegionSelector : MonoBehaviour
{
    [Header("Identifikace")]
    public string regionName = "Sasko - Hlavní";
    public string stateGroup = "Sasko"; // Slouí k propojení exkláv

    [Header("Barevné fáze")]
    public Color normalColor = new Color(0f, 0.19f, 0.33f);
    public Color hoverColor = new Color(0.2f, 0.5f, 0.8f);
    public Color selectedColor = new Color(0.9f, 0.7f, 0.1f); // lutá/Zlatá pro vıbìr

    [Header("Rychlost pøechodu barev")]
    public float colorChangeSpeed = 8f;

    private SpriteRenderer spriteRenderer;
    private Color targetColor;

    // Statickı seznam pro hromadné úpravy všech krajù na mapì
    private static List<RegionSelector> allRegions = new List<RegionSelector>();

    // PAMÌ: Která skupina (stát) je momentálnì trvale zakliknutá
    private static string activeSelectedGroup = "";

    void Awake()
    {
        allRegions.Add(this);
    }

    void OnDestroy()
    {
        allRegions.Remove(this);
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = normalColor;
            targetColor = normalColor;
        }
    }

    void Update()
    {
        // Plynulı pøechod barvy
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, Time.deltaTime * colorChangeSpeed);
        }

        // ZRUŠENÍ VİBÌRU: Kliknutí pravım tlaèítkem myši (index 1)
        if (Input.GetMouseButtonDown(1) && activeSelectedGroup != "")
        {
            DeselectAll();
        }
    }

    void OnMouseEnter()
    {
        // Zvırazní se POUZE tehdy, pokud tento kraj zrovna není trvale vybranı
        if (stateGroup != activeSelectedGroup)
        {
            SetGroupTargetColor(stateGroup, hoverColor);
        }
    }

    void OnMouseExit()
    {
        // Vrátí se do normálu POUZE tehdy, pokud tento kraj zrovna není trvale vybranı
        if (stateGroup != activeSelectedGroup)
        {
            SetGroupTargetColor(stateGroup, normalColor);
        }
    }

    void OnMouseDown()
    {
        // Pokud u byl vybranı jinı stát, nejprve ho odznaèíme
        if (activeSelectedGroup != "" && activeSelectedGroup != stateGroup)
        {
            DeselectAll();
        }

        // Nastavíme tento stát jako trvale vybranı a obarvíme ho lutì
        activeSelectedGroup = stateGroup;
        SetGroupTargetColor(stateGroup, selectedColor);

        Debug.Log($"Trvale vybrán kraj: {regionName} ze skupiny: {stateGroup}");
    }

    // Pomocná metoda pro obarvení všech èástí jednoho státu
    private static void SetGroupTargetColor(string groupName, Color newColor)
    {
        foreach (var region in allRegions)
        {
            if (region.stateGroup == groupName)
            {
                region.targetColor = newColor;
            }
        }
    }

    // Pomocná metoda pro zrušení lutého vıbìru (vyvoláno pravım tlaèítkem)
    private static void DeselectAll()
    {
        foreach (var region in allRegions)
        {
            if (region.stateGroup == activeSelectedGroup)
            {
                region.targetColor = region.normalColor;
            }
        }
        activeSelectedGroup = "";
        Debug.Log("Vıbìr kraje byl zrušen.");
    }
}
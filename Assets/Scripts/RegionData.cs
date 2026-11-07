
using UnityEngine;

public class RegionData : MonoBehaviour
{
    [Header("Nastavení regionu")]
    public string regionName;
    public CountryData ownerCountry;

    private SpriteRenderer spriteRenderer;
    private Color targetColor;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (ownerCountry != null)
        {
            targetColor = ownerCountry.countryColor;
        }
        else
        {
            targetColor = Color.white;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = targetColor;
        }
    }

    void Update()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.Lerp(
                spriteRenderer.color,
                targetColor,
                Time.deltaTime * 8f
            );
        }
    }

    public void SetColor(Color newColor)
    {
        targetColor = newColor;
    }

    public Color GetNormalColor()
    {
        if (ownerCountry != null)
        {
            return ownerCountry.countryColor;
        }

        return Color.white;
    }
}

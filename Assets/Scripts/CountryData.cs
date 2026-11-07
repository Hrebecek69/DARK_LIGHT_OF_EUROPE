using UnityEngine;

[CreateAssetMenu(fileName = "NovyStat", menuName = "DarkLight/Stat")]
public class CountryData : ScriptableObject
{
    [Header("Základní informace")]
    public string countryName;
    public Color countryColor; 
}
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Settings")]
    public string DisplayName;
    public int Score;
    public bool DoesEnergize;
    [Header("Textures")]
    public Sprite Sprite;
}

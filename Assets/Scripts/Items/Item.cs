using UnityEngine;

public class Item : MonoBehaviour, IPickable
{
    [Header("References")]
    public SpriteRenderer spriteRenderer;
    
    [HideInInspector] public LevelManager levelManager;
    public int Score;
    public bool DoesEnergize;

    public int Pickup()
    {
        levelManager.SpawnedItems.Remove(this);
        Destroy(gameObject);
        return Score;
    }
}

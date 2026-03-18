using UnityEngine;

public class Item : MonoBehaviour, IPickable
{
    [Header("References")]
    public SpriteRenderer spriteRenderer;
    
    [HideInInspector] public LevelManager levelManager;

    [HideInInspector] public int Score;

    public int Pickup()
    {
        levelManager.SpawnedItems.Remove(this);
        Destroy(gameObject);
        return Score;
    }
}

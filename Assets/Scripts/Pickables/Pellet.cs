using UnityEngine;

public class Pellet : MonoBehaviour, IPickable
{
    public int PickScore;
    public void Pickup()
    {
        Destroy(gameObject);
    }
}

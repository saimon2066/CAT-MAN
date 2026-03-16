using UnityEngine;

public class Pellet : MonoBehaviour, IPickable
{
    public int PickScore {get; set;} = 100;

    public void Pickup()
    {
        Destroy(gameObject);
    }
}

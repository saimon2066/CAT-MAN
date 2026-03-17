using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerManager player;

    private void OnTriggerEnter2D(Collider2D collision)
    {   
        GameObject other = collision.gameObject;
        if (other.TryGetComponent(out IPickable pickable))
        {
            pickable.Pickup();
            player.Score += pickable.PickScore;
        }
        else if (other.TryGetComponent(out IGhost ghost))
        {
            if (player.Energized)
            {
                ghost.Die();
            }
            else
            {
                // player.Die();
            }
        }
    }
}

using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerCollision : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerManager playerManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {   
        GameObject other = collision.gameObject;

        if (other.TryGetComponent(out IPickable pickable))
        {
            int score = pickable.Pickup();
            playerManager.UpdateScore(score);
        }
        else if (other.TryGetComponent(out IGhost ghost))
        {
            Debug.Log("ghost");
            if (playerManager.Energized)
            {
                ghost.SetEaten();
            }
            else
            {
                Debug.Log("player death");
                playerManager.Die();
            }
        }
    }
}

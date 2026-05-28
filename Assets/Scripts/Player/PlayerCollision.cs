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
            var (Score, DoesEnergize) = pickable.Pickup();

            playerManager.AddScore(Score);
            if (DoesEnergize)
            {
                playerManager.Energize();
            }
        }
        else if (other.TryGetComponent(out IGhost ghost))
        {
            if (ghost.ReturnState() != GhostManager.GhostState.Eaten)
            {
                if (ghost.ReturnState() == GhostManager.GhostState.Frightened)
                {
                    ghost.Die();
                    Debug.Log(200 * playerManager.GhostScoreMultiplier);
                    playerManager.AddScore(200 * playerManager.GhostScoreMultiplier);
                    playerManager.GhostScoreMultiplier *= 2;
                }
                else
                {
                    playerManager.Die();
                }
            }
        }
    }
}

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
            var (Score, Energized) = pickable.Pickup();
            playerManager.UpdateScore(Score);
            playerManager.UpdateEnergized(Energized);
        }
        else if (other.TryGetComponent(out IGhost ghost))
        {
            if (playerManager.Energized)
            {
                Debug.Log("kill ghost");
                ghost.UpdateState(GhostManager.GhostState.Eaten);
                playerManager.UpdateScore(200);
            }
            else
            {
                Debug.Log($"Death {playerManager.Energized}");
            }
        }
    }
}

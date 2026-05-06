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
            if (playerManager.Energized)
            {
                ghost.Die();
            }
            else
            {
                playerManager.Die();
            }
        }
    }
}

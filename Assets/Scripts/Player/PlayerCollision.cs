using UnityEngine;

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
    }
}

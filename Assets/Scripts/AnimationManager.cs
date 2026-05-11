using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Movement movement;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private LevelManager levelManager;

    private void OnEnable()
    {
        movement.MovementDirectionChanged += OnMovementDirectionChanged;
        playerManager.PlayerDeath += OnPlayerDeath;
        levelManager.LevelRespawnEveryone += OnLevelRespawnEveryone;
    }
    private void OnDisable()
    {
        movement.MovementDirectionChanged -= OnMovementDirectionChanged;
        playerManager.PlayerDeath -= OnPlayerDeath;
        levelManager.LevelRespawnEveryone -= OnLevelRespawnEveryone;
    }

    private void OnMovementDirectionChanged(Vector2Int dir)
    {
        animator.SetBool("Dead", false);

        if (dir == Vector2Int.up)
        {
            animator.SetInteger("Direction", 0);
        }
        else if (dir == Vector2Int.right)
        {
            animator.SetInteger("Direction", 1);
        }
        else if (dir == Vector2Int.left)
        {
            animator.SetInteger("Direction", 2);
        }
        else if (dir == Vector2Int.down)
        {
            animator.SetInteger("Direction", 3);
        }
    }

    private void OnPlayerDeath(int lives)
    {
        animator.SetBool("Dead", true);
    }

    private void OnLevelRespawnEveryone()
    {
        animator.SetBool("Dead", false);
    }
}

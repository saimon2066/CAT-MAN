using UnityEngine;

public class PlayerAnimationManager : CharacterAnimationBase
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private PlayerManager playerManager;

    public override void OnEnable()
    {
        base.OnEnable();
        levelManager.LevelRespawnEveryone += OnLevelRespawnEveryone;
        playerManager.PlayerDeath += OnPlayerDeath;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        levelManager.LevelRespawnEveryone -= OnLevelRespawnEveryone;
        playerManager.PlayerDeath -= OnPlayerDeath;
    }

    public override void OnMovementDirectionChanged(Vector2Int dir)
    {
        animator.SetBool("Dead", false);
        base.OnMovementDirectionChanged(dir);
    }

    private void OnLevelRespawnEveryone()
    {
        animator.SetBool("Dead", false);
    }

    private void OnPlayerDeath(int lives)
    {
        animator.SetBool("Dead", true);
    }
}
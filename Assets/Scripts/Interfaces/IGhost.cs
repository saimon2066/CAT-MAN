public interface IGhost
{
    public void SetState(GhostManager.GhostState state, bool isForced);
    public void SetPaused(bool paused, GhostManager.GhostState ignoreState = GhostManager.GhostState.None);
    public void Die();
}

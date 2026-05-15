public interface IGhost
{
    public void SetState(GhostManager.GhostState state, bool isForced, GhostManager.GhostState ignoreState = GhostManager.GhostState.None);
    public void SetPaused(bool paused, GhostManager.GhostState ignoreState = GhostManager.GhostState.None);
    public void Die();
    public GhostManager.GhostState ReturnState(); 
}

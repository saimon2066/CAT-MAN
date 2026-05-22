public interface IGhost
{
    public void SetState(GhostManager.GhostState state, bool isForced, GhostManager.GhostState[] ignoreStates = null);
    public void SetPaused(bool paused, GhostManager.GhostState[] ignoreStates = null);
    public void Die();
    public GhostManager.GhostState ReturnState(); 
}

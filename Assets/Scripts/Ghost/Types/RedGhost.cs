using UnityEngine;

public class RedGhost : MonoBehaviour, IGhost
{
    [Header("Settings")]
    [SerializeField] private Transform scatterTarget;
    [Header("References")]
    [SerializeField] private Movement player;
    [SerializeField] private GhostAI ai;

    private GhostManager.GhostState _state;

    private void Update() // dont use update
    {
        if (_state == GhostManager.GhostState.Chase)
        {
            ai.SetDestination(player.CurrentTile);
        }
        else if (_state == GhostManager.GhostState.Chase)
        {
            ai.SetDestination(Vector3Int.RoundToInt(scatterTarget.position));
        }        
    }
    public void UpdateState(GhostManager.GhostState state)
    {
        _state = state;
    }
}

using UnityEngine;

public class PinkGhost : MonoBehaviour, IGhost
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
            Vector3Int dest;
            if (player.Direction == new Vector2Int(0, 1))
            {
                dest = player.CurrentTile + new Vector3Int(-2, 2, 0);
            }
            else
            {
                dest = player.CurrentTile + (Vector3Int)player.Direction * 2;
            }
            ai.SetDestination(dest);
        }
        else if (_state == GhostManager.GhostState.Scatter)
        {
            ai.SetDestination(Vector3Int.RoundToInt(scatterTarget.position));
        }
    }
    public void UpdateState(GhostManager.GhostState state)
    {
        _state = state;
    }
}

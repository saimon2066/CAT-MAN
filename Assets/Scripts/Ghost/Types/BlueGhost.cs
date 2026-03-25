using UnityEngine;

public class BlueGhost : MonoBehaviour, IGhost
{
    [Header("Settings")]
    [SerializeField] private Transform scatterTarget;
    [Header("References")]
    [SerializeField] private Movement player;
    [SerializeField] private Movement red;
    [SerializeField] private GhostAI ai;

    private GhostManager.GhostState _state;

    private void Update() // dont use update
    {
        if (_state == GhostManager.GhostState.Chase)
        {
            Vector3Int dest;
            if (player.Direction == new Vector2Int(0, 1))
            {
                Vector3Int point = player.CurrentTile + new Vector3Int(-1, 1, 0);
                dest = point - (red.CurrentTile - point);
            }
            else
            {
                Vector3Int point = player.CurrentTile + (Vector3Int)player.Direction;
                dest = point - (red.CurrentTile - point);
            }
            ai.SetDestination(dest);
        }
        else
        {
            ai.SetDestination(Vector3Int.RoundToInt(scatterTarget.position));
        }
    }
    public void UpdateState(GhostManager.GhostState state)
    {
        _state = state;
    }
}

using UnityEngine;

public class PinkGhost : MonoBehaviour, IGhost
{
    [Header("Settings")]
    [SerializeField] private bool showDebug;
    [SerializeField] private Transform scatterTarget;
    [Header("References")]
    [SerializeField] private Movement player;
    [SerializeField] private GhostAI ai;

    private GhostManager.GhostState _state;

    private void Update()
    {
        if (_state == GhostManager.GhostState.Frightened)
        {
            ai.SetRandomization(true);
        }
        else
        {
            ai.SetRandomization(false);
                
            Vector3Int dest;
            if (_state == GhostManager.GhostState.Chase)
            {
                if (player.Direction == new Vector2Int(0, 1))
                {
                    dest = player.CurrentTile + new Vector3Int(-2, 2, 0);
                }
                else
                {
                    dest = player.CurrentTile + (Vector3Int)player.Direction * 2;
                }
            }
            else if (_state == GhostManager.GhostState.Scatter)
            {
                dest = Vector3Int.RoundToInt(scatterTarget.position);
            }
            else
            {
                dest = Vector3Int.zero;
            }

            ai.SetDestination(dest);

            if (showDebug)
            {
                Debug.DrawLine(ai.Movement.wallsTilemap.GetCellCenterWorld(ai.Movement.CurrentTile), ai.Movement.wallsTilemap.GetCellCenterWorld(dest), Color.hotPink, Time.deltaTime);
            }   
        }
    }
    public void UpdateState(GhostManager.GhostState state)
    {
        _state = state;
    }
}

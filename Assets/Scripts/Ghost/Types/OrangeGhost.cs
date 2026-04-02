using UnityEngine;

public class OrangeGhost : MonoBehaviour, IGhost
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
                if (Vector3Int.Distance(player.CurrentTile, ai.Movement.CurrentTile) < 8)
                {
                    dest = Vector3Int.RoundToInt(scatterTarget.position);
                }
                else
                {
                    dest = player.CurrentTile;
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
                Debug.DrawLine(ai.Movement.wallsTilemap.GetCellCenterWorld(ai.Movement.CurrentTile), ai.Movement.wallsTilemap.GetCellCenterWorld(dest), Color.orange, Time.deltaTime);
            }   
        }
    }

    public void UpdateState(GhostManager.GhostState state)
    {
        _state = state;
    }
}


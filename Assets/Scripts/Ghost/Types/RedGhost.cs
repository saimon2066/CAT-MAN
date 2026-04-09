using Unity.VisualScripting;
using UnityEngine;

public class RedGhost : MonoBehaviour, IGhost
{
    [Header("Settings")]
    [SerializeField] private bool showDebug;
    [SerializeField] private Transform scatterTarget;
    [Header("References")]
    [SerializeField] private Movement player;
    [SerializeField] private GhostAI ai;

    private GhostManager.GhostState _state;
    private bool _eaten;

    private void Update()
    {
        if (_eaten)
        {
            if (ai.Movement.CurrentTile == Vector3Int.zero)
            {
                _eaten = false;
                ai.ClearLastDirection();
            }
            else
            {
                ai.SetDestination(Vector3Int.zero);
            }

            if (showDebug)
            {
                Debug.DrawLine(ai.Movement.wallsTilemap.GetCellCenterWorld(ai.Movement.CurrentTile), ai.Movement.wallsTilemap.GetCellCenterWorld(Vector3Int.zero), Color.red, Time.deltaTime);
            }
        }
        else
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
                    dest = player.CurrentTile;
                }
                else //if (_state == GhostManager.GhostState.Scatter)
                {
                    dest = Vector3Int.RoundToInt(scatterTarget.position);
                }      
                ai.SetDestination(dest);  

                if (showDebug)
                {
                    Debug.DrawLine(ai.Movement.wallsTilemap.GetCellCenterWorld(ai.Movement.CurrentTile), ai.Movement.wallsTilemap.GetCellCenterWorld(dest), Color.red, Time.deltaTime);
                }
            }
        }
    }

    public void UpdateState(GhostManager.GhostState state)
    {
        if (state == GhostManager.GhostState.Eaten)
        {
            _eaten = true;
        }
        else
        {
            _state = state;
        }
    }
}

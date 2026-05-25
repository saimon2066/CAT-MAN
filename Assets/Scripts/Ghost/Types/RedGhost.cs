using UnityEngine;

public class RedGhost : GhostBase
{
    [SerializeField] private Movement player;

    private void Update()
    {
        Vector3Int? dest = null;
        switch (_state)
        {
            case GhostManager.GhostState.Chase: 
                _isRespawning = false;
                ai.Movement.Speed = baseSpeed;
                collarRenderer.color = color;
                dest = player.CurrentTile;
                break;

            case GhostManager.GhostState.Scatter:
                _isRespawning = false;
                ai.Movement.Speed = baseSpeed;
                collarRenderer.color = color;
                dest = ai.Movement.wallsTilemap.WorldToCell(scatterTarget.position);
                break;

            case GhostManager.GhostState.Frightened:
                _isRespawning = false;
                ai.Movement.Speed = frightenedSpeed;
                collarRenderer.color = Color.blue;
                dest = null;
                break;

            case GhostManager.GhostState.Eaten:
                ai.Movement.Speed = eatenSpeed;
                collarRenderer.color = Color.white;
                dest = ai.Movement.wallsTilemap.WorldToCell(eatenTarget.position);
                if (!_isRespawning)
                {
                    ai.DoorCell = new(0, 100, 0);
                    if (ai.Movement.CurrentTile == dest)
                    {
                        _isRespawning = true;   
                        SetState(GhostManager.GhostState.Leaving, true);              
                    }
                }
                break;

            case GhostManager.GhostState.Leaving:
                ai.Movement.Speed = baseSpeed;
                collarRenderer.color = Color.white;
                dest = ai.Movement.wallsTilemap.WorldToCell(leavingTarget.position);
                if (ai.Movement.CurrentTile == dest)
                {
                    ai.DoorCell = new(0, 1, 0);
                    SetPaused(false);
                    _isRespawning = false;
                }
                break;

            case GhostManager.GhostState.None:
                return;
        }

        ai.SetRandomization(_state == GhostManager.GhostState.Frightened);

        if (dest != null)
        {
            ai.SetDestination((Vector3Int)dest);

            if (showDebug)
            {
                Debug.DrawLine(ai.Movement.CurrentTile, (Vector3Int)dest, color, Time.deltaTime);                
            }
        }
    }
}

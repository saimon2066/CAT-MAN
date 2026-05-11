using System.Collections;
using UnityEngine;

public class RedGhost : MonoBehaviour, IGhost
{
    [Header("Settings")]
    [SerializeField] private Color color;
    [SerializeField] private bool showDebug;
    [SerializeField] private Transform scatterTarget;
    [SerializeField] private Transform eatenTarget;
    [SerializeField] private float cooldown;
    [SerializeField] private float baseSpeed, eatenSpeed, frightenedSpeed;
    [Header("References")]
    [SerializeField] private Movement player;
    [SerializeField] private GhostAI ai;

    private SpriteRenderer _spriteRenderer;

    private GhostManager.GhostState _state;
    private GhostManager.GhostState _futureState;

    private bool _statesPaused;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();   
        _spriteRenderer.color = color;   
    }
    private void Update()
    {
        Vector3Int? dest = null;
        switch (_state)
        {
            case GhostManager.GhostState.Chase: 
                ai.Movement.Speed = baseSpeed;
                _spriteRenderer.color = color;
                dest = player.CurrentTile;
                break;

            case GhostManager.GhostState.Scatter:
                ai.Movement.Speed = baseSpeed;
                _spriteRenderer.color = color;
                dest = ai.Movement.wallsTilemap.WorldToCell(scatterTarget.position);
                break;

            case GhostManager.GhostState.Frightened:
                ai.Movement.Speed = frightenedSpeed;
                _spriteRenderer.color = Color.blue;
                dest = null;
                break;

            case GhostManager.GhostState.Eaten:
                ai.Movement.Speed = eatenSpeed;
                _spriteRenderer.color = Color.white;
                dest = ai.Movement.wallsTilemap.WorldToCell(eatenTarget.position);
                ai.DoorCell = new(0, 100, 0);
                if (ai.Movement.CurrentTile == dest)
                {
                    StartCoroutine(RespawnCorot());
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

    public void SetState(GhostManager.GhostState state, bool isForced)
    {
        if (isForced)
        {
            ai.Movement.FlipDirection();
            _state = state;
        }   
        else
        {
            _futureState = state;
            if (!_statesPaused)
            {
                ai.Movement.FlipDirection();
                _state = state;
            }  
        }
    }
    public void SetPaused(bool pause, GhostManager.GhostState ignoreState = GhostManager.GhostState.None)
    {
        if (_state != ignoreState)
        {
            _statesPaused = pause;
            if (!_statesPaused)
            {
                SetState(_futureState, false);
            }   
        }
    }
    public void Die()
    {
        if (_state == GhostManager.GhostState.Frightened)
        {
            SetState(GhostManager.GhostState.Eaten, true);
            SetPaused(true);
        }
    }
    public GhostManager.GhostState ReturnState()
    {
        return _state;
    }

    private WaitForSeconds _wait2Sec = new(2);
    private IEnumerator RespawnCorot()
    {
        ai.DoorCell = new(0, 1, 0);
        yield return new WaitForSeconds(cooldown);
        ai.DoorCell = new(0, 100, 0);
        SetPaused(false);
        yield return _wait2Sec;
        ai.DoorCell = new(0, 1, 0);
    }
}

using System.Linq;
using UnityEngine;

public class GhostBase : MonoBehaviour, IGhost
{
    [Header("Settings")]
    [SerializeField] protected Color color;
    [SerializeField] protected Transform scatterTarget;
    [SerializeField] protected Transform eatenTarget;
    [SerializeField] protected Transform leavingTarget;
    [SerializeField] protected float baseSpeed, eatenSpeed, frightenedSpeed;
    [SerializeField] protected bool showDebug;
    [Header("References")]
    [SerializeField] protected GhostAI ai;
    [SerializeField] protected SpriteRenderer collarRenderer;

    protected GhostManager.GhostState _state;
    protected GhostManager.GhostState _futureState;
    protected bool _statesPaused;
    protected bool _isRespawning;

    public virtual void Start()
    { 
        collarRenderer.color = color;   
    }

    public void SetState(GhostManager.GhostState state, bool isForced, GhostManager.GhostState[] ignoreStates = null)
    {
        if (ignoreStates == null || !ignoreStates.Contains(_state))
        {
            if (isForced)
            {
                _state = state;
                ai.Movement.FlipDirection();
            }   
            else
            {
                if (!_statesPaused)
                {
                    _state = state;
                    ai.Movement.FlipDirection();
                }  
                else
                {
                    _futureState = state;
                }
            }
        }   
    }
    public void SetPaused(bool paused, GhostManager.GhostState[] ignoreStates = null)
    {
        if (ignoreStates == null || !ignoreStates.Contains(_state))
        {
            _statesPaused = paused;
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
}

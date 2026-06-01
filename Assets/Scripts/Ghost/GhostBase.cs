using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GhostBase : MonoBehaviour, IGhost
{
    [Header("Settings")]
    [SerializeField] protected Color color;
    [SerializeField] private Color collarColor;
    [SerializeField] protected Transform scatterTarget;
    [SerializeField] protected Transform eatenTarget;
    [SerializeField] protected Transform leavingTarget;
    [SerializeField] protected float baseSpeed, eatenSpeed, frightenedSpeed;
    [SerializeField] protected bool showDebug;
    [Header("References")]
    [SerializeField] protected GhostAI ai;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] protected SpriteRenderer collarRenderer;
    [SerializeField] protected SpriteRenderer outlineRenderer;

    protected GhostManager.GhostState _state;
    protected GhostManager.GhostState _futureState;
    protected bool _statesPaused;
    protected bool _isRespawning;

    protected Coroutine _frightenedCorot = null;

    private void OnEnable()
    {
        levelManager.LevelChanged += OnLevelChanged;
    }
    private void OnDisable()
    {
        levelManager.LevelChanged -= OnLevelChanged;
    }

    public virtual void Start()
    { 
        collarRenderer.color = collarColor;   
    }

    public void SetState(GhostManager.GhostState state, bool isForced, GhostManager.GhostState[] ignoreStates = null)
    {
        if (ignoreStates == null || !ignoreStates.Contains(_state))
        {
            if (isForced)
            {
                if (_state != GhostManager.GhostState.Leaving) ai.Movement.FlipDirection();
                _state = state;
            }   
            else
            {
                if (!_statesPaused)
                {
                    if (_state != GhostManager.GhostState.Leaving) ai.Movement.FlipDirection();
                    _state = state;
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

    private void OnLevelChanged(int level)
    {
        ai.Movement.ResetDirection(Vector2Int.up);
    }

    protected IEnumerator FrightenedColor()
    {
        outlineRenderer.color = Color.blue;
        yield return new WaitForSeconds(4f);
        outlineRenderer.color = Color.white;
        yield return new WaitForSeconds(0.5f);
        outlineRenderer.color = Color.blue;
        yield return new WaitForSeconds(0.5f);
        outlineRenderer.color = Color.white;
        yield return new WaitForSeconds(0.5f);
        outlineRenderer.color = Color.blue;
        yield return new WaitForSeconds(0.5f);
        outlineRenderer.color = Color.white;
    }
}

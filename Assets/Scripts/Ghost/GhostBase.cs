using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class GhostBase : MonoBehaviour, IGhost
{
    [Header("Settings")]
    [SerializeField] protected Color color;
    [SerializeField] protected Color collarColor;
    [SerializeField] protected Transform scatterTarget;
    [SerializeField] protected Transform eatenTarget;
    [SerializeField] protected Transform leavingTarget;
    [SerializeField] protected float baseSpeed, eatenSpeed, frightenedSpeed;
    [SerializeField] protected bool showDebug;
    [Header("References")]
    [SerializeField] private GameObject debugObject;
    [SerializeField] protected GhostAI ai;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] protected SpriteRenderer collarRenderer;
    [SerializeField] protected SpriteRenderer outlineRenderer;
    [SerializeField] private LineRenderer lineRenderer;

    protected GhostManager.GhostState _state;
    protected GhostManager.GhostState _futureState;
    protected bool _statesPaused;
    protected bool _isRespawning;
    protected bool _flashing;

    protected Coroutine _frightenedCorot = null;

    public virtual void OnEnable()
    {
        levelManager.LevelChanged += OnLevelChanged;
    }
    public virtual void OnDisable()
    {
        levelManager.LevelChanged -= OnLevelChanged;
    }

    public virtual void Start()
    { 
        collarRenderer.color = collarColor;   

        lineRenderer.positionCount = 2;
        lineRenderer.startColor = color;
        lineRenderer.endColor = collarColor;
        lineRenderer.startWidth = 0.15f;
        lineRenderer.endWidth = 0.05f;
    }
    public virtual void Update()
    {
        Vector3Int? dest = null;
        switch (_state)
        {
            case GhostManager.GhostState.Chase:
                _flashing = false;
                if (_frightenedCorot != null)
                {
                    StopCoroutine(_frightenedCorot);
                    _frightenedCorot = null;
                } 
                ai.Movement.blacklistedCell = new(0, 1, 0);
                _isRespawning = false;
                ai.Movement.Speed = baseSpeed;
                outlineRenderer.color = color;
                dest = GetDestination();
                break;

            case GhostManager.GhostState.Scatter:
                _flashing = false;
                if (_frightenedCorot != null)
                {
                    StopCoroutine(_frightenedCorot);
                    _frightenedCorot = null;
                }                 
                ai.Movement.blacklistedCell = new(0, 1, 0);
                _isRespawning = false;
                ai.Movement.Speed = baseSpeed;
                outlineRenderer.color = color;
                dest = ai.Movement.wallsTilemap.WorldToCell(scatterTarget.position);
                break;

            case GhostManager.GhostState.Frightened:
                _isRespawning = false;
                ai.Movement.Speed = frightenedSpeed;
                _frightenedCorot ??= StartCoroutine(FrightenedColor());
                dest = null;
                break;

            case GhostManager.GhostState.Eaten:
                _flashing = false;
                if (_frightenedCorot != null)
                {
                    StopCoroutine(_frightenedCorot);
                    _frightenedCorot = null;
                } 
                ai.Movement.Speed = eatenSpeed;
                outlineRenderer.color = Color.white;
                dest = ai.Movement.wallsTilemap.WorldToCell(eatenTarget.position);
                if (!_isRespawning)
                {
                ai.Movement.blacklistedCell = new(0, 100, 0);
                    if (ai.Movement.CurrentTile == dest)
                    {
                        _isRespawning = true;   
                        SetState(GhostManager.GhostState.Leaving, true);              
                    }
                }
                break;

            case GhostManager.GhostState.Leaving:
                _flashing = false;
                if (_frightenedCorot != null)
                {
                    StopCoroutine(_frightenedCorot);
                    _frightenedCorot = null;
                } 
                ai.Movement.blacklistedCell = new(0, 100, 0);
                ai.Movement.Speed = baseSpeed;
                outlineRenderer.color = Color.white;
                dest = ai.Movement.wallsTilemap.WorldToCell(leavingTarget.position);
                if (ai.Movement.CurrentTile == dest)
                {
                    ai.Movement.blacklistedCell = new(0, 1, 0);
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
                Vector3 pos = ai.Movement.wallsTilemap.GetCellCenterWorld((Vector3Int)dest);

                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, ai.Movement.wallsTilemap.GetCellCenterWorld(ai.Movement.CurrentTile));
                lineRenderer.SetPosition(1, pos);

                debugObject.SetActive(true);
                debugObject.transform.position = pos;
            }
            else 
            {
                lineRenderer.enabled = false; 
                debugObject.SetActive(false);
            }
        }
        else 
        {
            lineRenderer.enabled = false; 
            debugObject.SetActive(false);
        }
    }

    public abstract Vector3Int? GetDestination();

    public void SetState(GhostManager.GhostState state, bool isForced, GhostManager.GhostState[] ignoreStates = null)
    {
        if (_state == GhostManager.GhostState.Frightened && state == GhostManager.GhostState.Frightened)
        {
            if (_frightenedCorot != null)
            {
                StopCoroutine(_frightenedCorot);
                _frightenedCorot = null;
            }
            _flashing = false;
            return;
        }

        if (ignoreStates == null || !ignoreStates.Contains(_state))
        {
            if (state == GhostManager.GhostState.Frightened)
            {
                if (_frightenedCorot != null)
                {
                    StopCoroutine(_frightenedCorot);
                    _frightenedCorot = null;
                }
                _flashing = false;
            }

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
        _flashing = true;
        outlineRenderer.color = Color.white;
        yield return new WaitForSeconds(0.5f);
        outlineRenderer.color = Color.blue;
        yield return new WaitForSeconds(0.5f);
        outlineRenderer.color = Color.white;
        yield return new WaitForSeconds(0.5f);
        outlineRenderer.color = Color.blue;
        yield return new WaitForSeconds(0.5f);
        outlineRenderer.color = Color.white;
        _flashing = false;
        _frightenedCorot = null;
    }
}

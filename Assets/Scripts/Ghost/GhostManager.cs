using System.Collections;
using UnityEngine;

public class Phase
{
    public GhostManager.GhostState State;
    public float Time;
    public Phase(GhostManager.GhostState state, float time)
    {
        State = state;
        Time = time;
    }
}

public class GhostManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private MonoBehaviour[] ghosts;
    [Header("References")]
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private PlayerManager playerManager;

    private Coroutine _phaseCorot;
    private bool _isPaused;

    public enum GhostState
    {
        Chase, Scatter, Frightened, Eaten
    }

    private GhostState _currentState;

    private readonly Phase[][] _phaseTable =
    {
        new Phase[] // Level 1
        {
            new(GhostState.Scatter, 7f),
            new(GhostState.Chase, 20f),
            new(GhostState.Scatter, 7f),
            new(GhostState.Chase, 20f),
            new(GhostState.Scatter, 5f),
            new(GhostState.Chase, 20f),
            new(GhostState.Scatter, 5f),
            new(GhostState.Chase, float.MaxValue)
        },
        new Phase[] // Level 2-4
        {
            new(GhostState.Scatter, 7f),
            new(GhostState.Chase, 20f),
            new(GhostState.Scatter, 7f),
            new(GhostState.Chase, 20f),
            new(GhostState.Scatter, 5f),
            new(GhostState.Chase, 1000f),
            new(GhostState.Scatter, 0.01f),
            new(GhostState.Chase, float.MaxValue)
        },
        new Phase[] // Level 5+
        {
            new(GhostState.Scatter, 5f),
            new(GhostState.Chase, 20f),
            new(GhostState.Scatter, 5f),
            new(GhostState.Chase, 20f),
            new(GhostState.Scatter, 5f),
            new(GhostState.Chase, 1033.14f),
            new(GhostState.Scatter, 0.01f),
            new(GhostState.Chase, float.MaxValue)
        }
    };

    private void OnEnable()
    {
        levelManager.LevelChanged += OnLevelChanged;
        playerManager.PlayerEnergizedChanged += OnPlayerEnergizedChanged;
    }
    private void OnDisable()
    {
        levelManager.LevelChanged -= OnLevelChanged;
        playerManager.PlayerEnergizedChanged -= OnPlayerEnergizedChanged;
    }

    private void OnLevelChanged(int level)
    {
        if (_phaseCorot != null)
        {
            StopCoroutine(_phaseCorot);
        }

        _phaseCorot = StartCoroutine(PhaseCorot(level));
    }
    private void OnPlayerEnergizedChanged(bool energized)
    {
        SetPause(energized);

        if (energized)
        {
            SetGhostsState(GhostState.Frightened);
        }
        else
        {
            SetGhostsState(_currentState);
        }
    }

    private void SetGhostsState(GhostState state)
    {
        foreach (MonoBehaviour mono in ghosts)
        {
            if (mono.TryGetComponent(out IGhost ghost))
            {
                ghost.UpdateState(state);
            }
        }
    }

    public void SetPause(bool isPaused)
    {
        _isPaused = isPaused;
    }

    private IEnumerator PhaseCorot(int level)
    {
        if (level <= 1)
        {
            foreach (Phase phase in _phaseTable[0])
            {
                float elapsed = 0f;
                while (elapsed < phase.Time)
                {
                    if (!_isPaused)
                    {
                        elapsed += Time.deltaTime;

                        _currentState = phase.State;
                        SetGhostsState(phase.State);
                    }

                    yield return null;
                }
            }
        }
        else if (level <= 4)
        {
            foreach (Phase phase in _phaseTable[1])
            {
                float elapsed = 0f;
                while (elapsed < phase.Time)
                {
                    if (!_isPaused)
                    {
                        elapsed += Time.deltaTime;

                        _currentState = phase.State;
                        SetGhostsState(phase.State);
                    }

                    yield return null;
                }
            }
        }
        else
        {
            foreach (Phase phase in _phaseTable[3])
            {
                float elapsed = 0f;
                while (elapsed < phase.Time)
                {
                    if (!_isPaused)
                    {
                        elapsed += Time.deltaTime;

                        _currentState = phase.State;
                        SetGhostsState(phase.State);
                    }

                    yield return null;
                }
            }
        }
    }
}

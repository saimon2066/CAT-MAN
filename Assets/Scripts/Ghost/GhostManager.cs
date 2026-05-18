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

    public enum GhostState
    {
        Chase, Scatter, Frightened, Eaten, None
    }

    private readonly Phase[][] _phases =
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
        playerManager.PlayerEnergize += OnPlayerEnergize;
    }
    private void OnDisable()
    {
        levelManager.LevelChanged -= OnLevelChanged;
        playerManager.PlayerEnergize -= OnPlayerEnergize;
    }
    private void OnLevelChanged(int level)
    {
        SetGhostPaused(false);

        if (_phaseCorot != null)
        {
            StopCoroutine(_phaseCorot);
        }

        _phaseCorot = StartCoroutine(PhaseCorot(level));
    }
    private void OnPlayerEnergize(bool energize)
    {
        if (energize)
        {
            SetGhostStates(GhostState.Frightened, false, GhostState.Eaten);
            SetGhostPaused(true);
        }
        else
        {
            SetGhostPaused(false, GhostState.Eaten);
        }
    }

    private void SetGhostStates(GhostState state, bool isForced, GhostState ignoreState = GhostState.None)
    {
        foreach (MonoBehaviour mono in ghosts)
        {
            if (mono.TryGetComponent(out IGhost ghost))
            {
                ghost.SetState(state, isForced, ignoreState);
            }
        }
    }
    private void SetGhostPaused(bool pause, GhostState ignoreState = GhostState.None)
    {
        Debug.Log(pause);

        foreach (MonoBehaviour mono in ghosts)
        {
            if (mono.TryGetComponent(out IGhost ghost))
            {
                ghost.SetPaused(pause, ignoreState);
            }
        }
    }

    private IEnumerator PhaseCorot(int level)
    {
        if (level <= 1)
        {
            foreach (Phase phase in _phases[0])
            {
                SetGhostStates(phase.State, false);
                yield return new WaitForSeconds(phase.Time);
            }
        }
        else if (level <= 4)
        {
            foreach (Phase phase in _phases[1])
            {
                SetGhostStates(phase.State, false);
                yield return new WaitForSeconds(phase.Time);
            }
        }
        else
        {
            foreach (Phase phase in _phases[2])
            {
                SetGhostStates(phase.State, false);
                yield return new WaitForSeconds(phase.Time);
            }
        }
    }
}

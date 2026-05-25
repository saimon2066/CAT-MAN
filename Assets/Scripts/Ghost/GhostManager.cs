using System.Collections;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject[] ghosts;
    [Header("References")]
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private PlayerManager playerManager;

    private Coroutine _phaseCorot;

    public enum GhostState
    {
        Chase, Scatter, Frightened, Eaten, Leaving, None
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
        GhostState[] ignoreStates = {GhostState.Eaten, GhostState.Leaving};
        if (energize)
        {
            SetGhostStates(GhostState.Frightened, false, ignoreStates);
            SetGhostPaused(true);
        }
        else
        {
            SetGhostPaused(false, ignoreStates);
        }
    }

    private void SetGhostStates(GhostState state, bool isForced, GhostState[] ignoreStates = null)
    {
        foreach (GameObject obj in ghosts)
        {
            if (obj.TryGetComponent(out GhostBase ghost))
            {
                ghost.SetState(state, isForced, ignoreStates);
            }
        }
    }
    private void SetGhostPaused(bool pause, GhostState[] ignoreStates = null)
    {
        foreach (GameObject obj in ghosts)
        {
            if (obj.TryGetComponent(out GhostBase ghost))
            {
                ghost.SetPaused(pause, ignoreStates);
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

    private class Phase
    {
        public GhostState State;
        public float Time;
        public Phase(GhostState state, float time)
        {
            State = state;
            Time = time;
        }
    }
}

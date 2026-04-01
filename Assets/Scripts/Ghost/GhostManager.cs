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

    private Coroutine _phaseCorot;

    public enum GhostState
    {
        Chase, Scatter, Frightened, Eaten
    }

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
    }
    private void OnDisable()
    {
        levelManager.LevelChanged -= OnLevelChanged;
    }

    private void OnLevelChanged(int level)
    {
        if (_phaseCorot != null)
        {
            StopCoroutine(_phaseCorot);
        }

        _phaseCorot = StartCoroutine(PhaseCorot(level));
    }

    private IEnumerator PhaseCorot(int level)
    {
        if (level <= 1)
        {
            foreach (Phase phase in _phaseTable[0])
            {
                foreach (MonoBehaviour mono in ghosts)
                {
                    if (mono.TryGetComponent(out IGhost ghost))
                    {
                        ghost.UpdateState(phase.State);   
                    }
                }
                yield return new WaitForSeconds(phase.Time);
            }
        }
        else if (level <= 4)
        {
            foreach (Phase phase in _phaseTable[1])
            {
                foreach (MonoBehaviour mono in ghosts)
                {
                    if (mono.TryGetComponent(out IGhost ghost))
                    {
                        ghost.UpdateState(phase.State);   
                    }
                }
                yield return new WaitForSeconds(phase.Time);
            }
        }
        else
        {
            foreach (Phase phase in _phaseTable[2])
            {
                foreach (MonoBehaviour mono in ghosts)
                {
                    if (mono.TryGetComponent(out IGhost ghost))
                    {
                        ghost.UpdateState(phase.State);   
                    }
                }
                yield return new WaitForSeconds(phase.Time);
            }
        }
    }
}

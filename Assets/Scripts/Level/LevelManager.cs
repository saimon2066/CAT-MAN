using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerManager playerManager;

    private List<Pellet> _pellets = new();

    private void OnEnable()
    {
        playerManager.ScoreChanged += OnScoreChanged;
    }
    private void OnDisable()
    {
        playerManager.ScoreChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(int score)
    {
        if (_pellets.Count > 0)
        {
            Debug.Log("next level");
        }
    }
}

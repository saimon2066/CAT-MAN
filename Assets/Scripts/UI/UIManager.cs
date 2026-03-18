using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreGUI;
    [SerializeField] private TextMeshProUGUI levelGUI;
    [Header("References")]
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private PlayerManager playerManager;

    private void OnEnable()
    {
        playerManager.ScoreChanged += OnScoreChanged;
        levelManager.LevelChanged += OnLevelChanged;
    }
    private void OnDisable()
    {
        playerManager.ScoreChanged -= OnScoreChanged;
        levelManager.LevelChanged -= OnLevelChanged;
    }

    private void OnScoreChanged(int score)
    {
        scoreGUI.text = $"{score}";
    }
    private void OnLevelChanged(int level)
    {
        levelGUI.text = $"level: {level}";
    }
}

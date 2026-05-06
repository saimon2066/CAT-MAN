using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI scoreDisplay;
    [SerializeField] private TextMeshProUGUI livesDisplay;
    [SerializeField] private TextMeshProUGUI levelDisplay;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private PlayerManager playerManager;

    private void OnEnable()
    {
        playerManager.PlayerScoreChanged += OnPlayerScoreChanged;
        playerManager.PlayerDeath += OnPlayerDeath;
        levelManager.LevelChanged += OnLevelChanged;
    }
    private void OnDisable()
    {
        playerManager.PlayerScoreChanged -= OnPlayerScoreChanged;
        playerManager.PlayerDeath -= OnPlayerDeath;
        levelManager.LevelChanged -= OnLevelChanged;
    }

    private void OnPlayerScoreChanged(int score)
    {
        scoreDisplay.text = $"{score}";
    }
    private void OnPlayerDeath(int lives)
    {
        livesDisplay.text = $"lives: {lives}";
    }
    private void OnLevelChanged(int level)
    {
        levelDisplay.text = $"level: {level}";
    }
}

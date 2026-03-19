using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    public GameObject Player;
    public int Score;
    public bool Energized;

    public event Action<int> ScoreChanged;

    public void UpdateScore(int newScore)
    {
        Score += newScore;
        ScoreChanged?.Invoke(Score);
    }
    public void SetMovement(bool toggle)
    {
        playerMovement.IsPaused = !toggle;
    }
}

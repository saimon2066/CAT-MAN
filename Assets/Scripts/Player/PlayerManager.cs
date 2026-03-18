using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int Score;
    public bool Energized;

    public event Action<int> ScoreChanged;

    public void UpdateScore(int newScore)
    {
        Score += newScore;
        ScoreChanged?.Invoke(Score);
    }
}

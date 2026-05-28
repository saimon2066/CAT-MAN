using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int Lives = 3;
    public int Score;
    public bool Energized;

    public int GhostScoreMultiplier = 1;

    public event Action<int> PlayerScoreChanged;
    public event Action<int> PlayerDeath;
    public event Action<bool> PlayerEnergize;

    private Coroutine _energizeCorot;

    public void AddScore(int score)
    {
        Score += score;
        PlayerScoreChanged?.Invoke(Score);
    }
    public void Energize()
    {
        GhostScoreMultiplier = 1;
        if (_energizeCorot != null)
        {
            StopCoroutine(_energizeCorot);
        }
        _energizeCorot = StartCoroutine(EnergizeCorot());
    }
    public void Die()
    {
        Lives--;
        PlayerDeath?.Invoke(Lives);

        if (Lives == 0)
        {
            Lives = 3;
            Score = 0;
            Energized = false;

            PlayerScoreChanged?.Invoke(Score);
            PlayerDeath?.Invoke(Lives);
        }
    }

    private WaitForSeconds _wait6Sec = new(6);
    private IEnumerator EnergizeCorot()
    {
        Energized = true;
        PlayerEnergize?.Invoke(Energized);
        yield return _wait6Sec;
        Energized = false;
        PlayerEnergize?.Invoke(Energized);
    }
}

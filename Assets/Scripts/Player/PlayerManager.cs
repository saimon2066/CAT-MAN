using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int Lives = 3;
    public int Score;
    public bool Energized;

    public event Action<int> PlayerScoreChanged;
    public event Action<int> PlayerDeath;

    public void UpdateScore(int score)
    {
        Score += score;
        PlayerScoreChanged?.Invoke(Score);
    }
    public event Action<int> PlayerLivesChanged;
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
    public event Action<bool> PlayerEnergizedChanged;

    private Coroutine _energizedCorot;
    private static WaitForSeconds _wait6Seconds = new(6f);
    public void UpdateLives(int lives)
    {
        Lives += lives;
        PlayerLivesChanged?.Invoke(Lives);
    }
    public void UpdateEnergized(bool energized)
    {
        if (energized)
        {
            if (_energizedCorot != null)
            {
                StopCoroutine(_energizedCorot);
            }
            _energizedCorot = StartCoroutine(EnergizedCorot(energized));
        }
        /*else
        {
            if (_energizedCorot != null)
            {
                StopCoroutine(_energizedCorot);
            }

            Energized = energized;
        }*/
    }

    private IEnumerator EnergizedCorot(bool energized)
    {
        Energized = energized;
        PlayerEnergizedChanged?.Invoke(Energized);
        Debug.Log($"energized: {Energized}");

        yield return _wait6Seconds;

        Energized = false;
        PlayerEnergizedChanged?.Invoke(Energized);
        Debug.Log($"not energized anymore: {Energized}");
    }
}

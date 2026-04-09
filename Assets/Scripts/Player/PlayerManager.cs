using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int Score;
    public int Lives = 3;
    public bool Energized;

    public event Action<int> PlayerScoreChanged;
    public event Action<int> PlayerLivesChanged;
    public event Action<bool> PlayerEnergizedChanged;

    private Coroutine _energizedCorot;
    private static WaitForSeconds _wait6Seconds = new(6f);

    public void UpdateScore(int score)
    {
        Score += score;
        PlayerScoreChanged?.Invoke(Score);
    }
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

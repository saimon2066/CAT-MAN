using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreDisplay;
    [Header("References")]
    [SerializeField] private PlayerManager player;

    private void Update()
    {
        scoreDisplay.text = $"{player.Score}";
    }
}

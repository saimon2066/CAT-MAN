using UnityEngine;

public class RedGhost : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Movement player;
    [SerializeField] private GhostAI ai;

    private void Update() // dont use update
    {
        ai.SetDestination(player.CurrentTile);
    }
}

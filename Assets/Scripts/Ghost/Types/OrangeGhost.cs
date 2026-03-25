using UnityEngine;

public class OrangeGhost : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform scatterTarget;
    [Header("References")]
    [SerializeField] private Movement player;
    [SerializeField] private GhostAI ai;

    private void Update() // dont use update
    {
        if (Vector3Int.Distance(player.CurrentTile, ai.movement.CurrentTile) < 8)
        {
            ai.SetDestination(Vector3Int.RoundToInt(scatterTarget.position));
        }
        else
        {
            ai.SetDestination(player.CurrentTile);
        }
    }
}


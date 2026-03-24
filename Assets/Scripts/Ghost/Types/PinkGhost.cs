using UnityEngine;

public class PinkGhost : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Movement player;
    [SerializeField] private GhostAI ai;

    private void Update() // dont use update
    {
        Vector3Int dest;
        if (player.Direction == new Vector2Int(0, 1))
        {
            dest = player.CurrentTile + new Vector3Int(-2, 2, 0);
        }
        else
        {
            dest = player.CurrentTile + (Vector3Int)player.Direction * 2;
        }
        ai.SetDestination(dest);
    }
}

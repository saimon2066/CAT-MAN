using UnityEngine;

public class BlueGhost : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject debugCircle;
    [SerializeField] private GameObject debug2;
    [SerializeField] private Movement player;
    [SerializeField] private Movement red;
    [SerializeField] private GhostAI ai;

    private void Update() // dont use update
    {
        Vector3Int dest;
        if (player.Direction == new Vector2Int(0, 1))
        {
            Vector3Int point = player.CurrentTile + new Vector3Int(-1, 1, 0);
            dest = point - (red.CurrentTile - point);

            debug2.transform.position = point;
        }
        else
        {
            Vector3Int point = player.CurrentTile + (Vector3Int)player.Direction;
            dest = point - (red.CurrentTile - point);
            debug2.transform.position = point;
        }
        debugCircle.transform.position = player.wallsTilemap.GetCellCenterWorld(dest);
        ai.SetDestination(dest);
    }
}

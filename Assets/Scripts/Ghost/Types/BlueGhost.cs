using UnityEngine;

public class BlueGhost : GhostBase
{
    [SerializeField] private Movement player;
    [SerializeField] private Movement redGhost;

    public override Vector3Int? GetDestination()
    {
        if (player.Direction == new Vector2Int(0, 1))
        {
            Vector3Int point = player.CurrentTile + new Vector3Int(-1, 1, 0);
            return point - (redGhost.CurrentTile - point);
        }
        else
        {
            Vector3Int point = player.CurrentTile + (Vector3Int)player.Direction;
            return point - (redGhost.CurrentTile - point);
        }
    }
}
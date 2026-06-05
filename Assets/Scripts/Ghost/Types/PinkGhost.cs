using UnityEngine;

public class PinkGhost : GhostBase
{
    [SerializeField] private Movement player;

    public override Vector3Int? GetDestination()
    {
        if (player.Direction == new Vector2Int(0, 1))
        {
            return player.CurrentTile + new Vector3Int(-2, 2, 0);
        }
        else
        {
            return player.CurrentTile + (Vector3Int)player.Direction * 2;
        }    
    }
}
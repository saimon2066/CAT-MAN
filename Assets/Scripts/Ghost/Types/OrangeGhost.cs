using UnityEngine;

public class OrangeGhost : GhostBase
{
    [SerializeField] private Movement player;

    public override Vector3Int? GetDestination()
    {
        if (Vector3Int.Distance(player.CurrentTile, ai.Movement.CurrentTile) < 8)
        {
            return ai.Movement.wallsTilemap.WorldToCell(scatterTarget.position);
        }
        else
        {
            return player.CurrentTile;
        }
    }
}
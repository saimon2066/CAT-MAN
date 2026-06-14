using UnityEngine;

public class RedGhost : GhostBase
{
    [SerializeField] private Movement player;
    
    public override Vector3Int? GetDestination()
    {
        return player.CurrentTile;
    }
}

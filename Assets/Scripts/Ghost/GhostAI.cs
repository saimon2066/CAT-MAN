using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Movement movement;

    private Vector3Int _destination;
    private Vector2Int[] _directions = {new(0, 1), new(-1, 0), new(0, -1), new(1, 0)}; // Up, Left, Down, Right

    private Vector2Int _lastDirection;
    private Vector3Int _lastTile;

    public void Update()
    {
        if (movement.CloseToCenter && movement.CurrentTile != _lastTile)
        {
            _lastTile = movement.CurrentTile;
            RunAlgorithm();
        }
    }

    public void RunAlgorithm()
    {
        Dictionary<Vector2Int, float> possible = new();
        List<Vector2Int> badPossible = new();

        foreach (Vector2Int dir in _directions)
        {
            Vector3Int next = movement.CurrentTile + (Vector3Int)dir;
            if (!movement.wallsTilemap.HasTile(next))
            {
                if (dir != -_lastDirection)
                {
                    float distanceCurrent = Vector3.Distance(movement.CurrentTile, _destination);
                    float distanceNext = Vector3.Distance(next, _destination);
                    if (distanceNext <= distanceCurrent)
                    {
                        possible.Add(dir, distanceNext);
                    }
                    else
                    {
                        badPossible.Add(dir);
                    }   
                }                
            }
        }

        if (possible.Count > 0)
        {
            float min = possible.Values.Min();
            var allMinimum = possible.Where(dictionary => dictionary.Value == min).Select(dictionary => dictionary.Key);

            foreach (Vector2Int dir in _directions)
            {
                if (allMinimum.Contains(dir))
                {
                    movement.SetDirection(dir);
                    _lastDirection = dir;
                    break;
                }
            }
        }
        else
        {
            foreach (Vector2Int dir in _directions)
            {
                if (badPossible.Contains(dir))
                {
                    if (dir != -_lastDirection)
                    {
                        movement.SetDirection(dir);
                        _lastDirection = dir;
                        break;  
                    }
                }
            }
        }
    }

    public void SetDestination(Vector3Int destination)
    {
        Debug.Log(destination + " " + gameObject.name);
        _destination = destination;
    }
}
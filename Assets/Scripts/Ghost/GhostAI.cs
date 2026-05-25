using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostAI : MonoBehaviour
{
    [Header("References")]
    public Movement Movement;

    [HideInInspector] public Vector3Int DoorCell = new(0, 1, 0);

    private Vector3Int _destination;
    private Vector2Int[] _directions = {new(0, 1), new(-1, 0), new(0, -1), new(1, 0)}; // Up, Left, Down, Right

    private Vector3Int _lastTile;

    private bool _randomize;

    private void Update()
    {
        if (Movement.CloseToCenter && Movement.CurrentTile != _lastTile)
        {
            _lastTile = Movement.CurrentTile;
            RunAlgorithm();
        }
    }

    private void RunAlgorithm()
    {
        Dictionary<Vector2Int, float> possibleDirectionsAndDistance = GetPossibleDirectionsAndDistance();

        if (_randomize)
        {
            int rand = Random.Range(0, possibleDirectionsAndDistance.Count);
            Vector2Int dir = possibleDirectionsAndDistance.Keys.ElementAt(rand);
            Movement.SetDirection(dir);
        }
        else
        {
            Dictionary<Vector2Int, float> possibleDirectionsByDistance = new();

            foreach (var DirDist in possibleDirectionsAndDistance)
            {
                float distanceCurrent = Vector3.Distance(Movement.wallsTilemap.CellToWorld(Movement.CurrentTile), Movement.wallsTilemap.CellToWorld(_destination));
                if (DirDist.Value <= distanceCurrent)
                {
                    possibleDirectionsByDistance.Add(DirDist.Key, DirDist.Value);
                }
            }

            if (possibleDirectionsByDistance.Count > 0)
            {
                float min = possibleDirectionsByDistance.Values.Min(); // get minimum value (distnace) of the tile in the dictionary
                var allMinimum = possibleDirectionsByDistance.Where(dictionary => dictionary.Value == min).Select(dictionary => dictionary.Key); // get all keys in the dictionary where their values are equal to the minimum value

                foreach (Vector2Int dir in _directions)
                {
                    if (allMinimum.Contains(dir))
                    {
                        Movement.SetDirection(dir);
                        break;
                    }
                }
            }
            else
            {
                foreach (Vector2Int dir in _directions)
                {
                    if (possibleDirectionsAndDistance.Keys.Contains(dir))
                    {
                        Movement.SetDirection(dir);
                        break;  
                    }   
                }   
            }
        }
    }
    private Dictionary<Vector2Int, float> GetPossibleDirectionsAndDistance()
    {
        Dictionary<Vector2Int, float> possible = new();
        
        foreach (Vector2Int dir in _directions)
        {
            Vector3Int next = Movement.CurrentTile + (Vector3Int)dir;

            if (!Movement.wallsTilemap.HasTile(next) && dir != -Movement.Direction && next != DoorCell)
            {                
                float distance = Vector3.Distance(Movement.wallsTilemap.CellToWorld(next), Movement.wallsTilemap.CellToWorld(_destination));
                possible.Add(dir, distance);
            }
        }

        if (possible.Count == 0)
        {
            Vector2Int reverse = -Movement.Direction;
            Vector3Int next = Movement.CurrentTile + (Vector3Int)reverse;

            if (!Movement.wallsTilemap.HasTile(next) && next != DoorCell)
            {
                float distance = Vector3.Distance(Movement.wallsTilemap.CellToWorld(next), Movement.wallsTilemap.CellToWorld(_destination));
                possible.Add(reverse, distance);   
            }
        }

        return possible;
    }

    public void SetDestination(Vector3Int destination)
    {
        _destination = destination;
    }
    public void SetRandomization(bool randomize)
    {
        _randomize = randomize;
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostAI : MonoBehaviour
{
    [Header("References")]
    public Movement Movement;

    private Vector3Int _destination;
    private Vector2Int[] _directions = {new(0, 1), new(-1, 0), new(0, -1), new(1, 0)}; // Up, Left, Down, Right

    private Vector2Int _lastDirection;
    private Vector3Int _lastTile;

    private bool _randomize;

    public void Update()
    {
        if (Movement.CloseToCenter && Movement.CurrentTile != _lastTile)
        {
            _lastTile = Movement.CurrentTile;
            RunAlgorithm();
        }
    }

    public void RunAlgorithm()
    {
        Dictionary<Vector2Int, float> possibleDirectionsAndDistance = GetPossibleDirectionsAndDistance();

        if (_randomize)
        {
            int rand = Random.Range(0, possibleDirectionsAndDistance.Count - 1);
            Movement.SetDirection(possibleDirectionsAndDistance.Keys.ElementAt(rand));
            _lastDirection = possibleDirectionsAndDistance.Keys.ElementAt(rand);   
        }
        else
        {
            Dictionary<Vector2Int, float> possibleDirectionsByDistance = new();

            foreach (var DirDist in possibleDirectionsAndDistance)
            {
                float distanceCurrent = Vector3.Distance(Movement.CurrentTile, _destination);
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
                        _lastDirection = dir;
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
                        _lastDirection = dir;
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

            if (!Movement.wallsTilemap.HasTile(next))
            {
                float distance = Vector3.Distance(next, _destination);

                if (dir != -_lastDirection)
                {
                    possible.Add(dir, distance);
                }
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
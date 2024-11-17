using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public GameObject[] tiles;
    [SerializeField] private int width = 10;   // Width of map  
    [SerializeField] private int height = 10;  // Height of map
    
    private const int N = 1;
    private const int E = 2;
    private const int S = 4;
    private const int W = 8;
    private const float TileSize = 10;

    private readonly Dictionary<Vector2, int> _cellWalls = new();

    private readonly List<List<int>> _map = new();
    
    private void Start()
    {
        _cellWalls[new Vector2(0, -1)] = N;
        _cellWalls[new Vector2(1, 0)] = E;
        _cellWalls[new Vector2(0, 1)] = S;
        _cellWalls[new Vector2(-1, 0)] = W;

        MakeMaze();
    }

    private List<Vector2> CheckNeighbors(Vector2 cell, List<Vector2> unvisited) {
        // Returns a list of cell's unvisited neighbors
        var list = new List<Vector2>();

        foreach (var n in _cellWalls.Keys)
        {
            if (unvisited.IndexOf((cell + n)) != -1) { 
                list.Add(cell+ n);
            }
                    
        }
        return list;
    }
    
    private void MakeMaze()
    {
        var unvisited = new List<Vector2>();
        var stack = new List<Vector2>();

        // Fill the map with #15 tiles
        for (var i = 0; i < width; i++)
        {
            _map.Add(new List<int>());
            for (var j = 0; j < height; j++)
            {
                _map[i].Add(N | E | S | W);
                unvisited.Add(new Vector2(i, j));
            }

        }

        var current = new Vector2(0, 0);

        unvisited.Remove(current);

        while (unvisited.Count > 0) {
            var neighbors = CheckNeighbors(current, unvisited);

            if (neighbors.Count > 0)
            {
                var next = neighbors[Random.Range(0, neighbors.Count)];
                stack.Add(current);

                var dir = next - current;

                var currentWalls = _map[(int)current.x][(int)current.y] - _cellWalls[dir];

                var nextWalls = _map[(int)next.x][(int)next.y] - _cellWalls[-dir];

                _map[(int)current.x][(int)current.y] = currentWalls;

                _map[(int)next.x][(int)next.y] = nextWalls;

                current = next;
                unvisited.Remove(current);

            }
            else if (stack.Count > 0) { 
                current = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);
            
            }
        }

        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                var tile = Instantiate(tiles[_map[i][j]], gameObject.transform, true);

                tile.transform.Translate(new Vector3 (j*TileSize, 0, i * TileSize));
                tile.name += " " + i + ' ' + j; 
            }

        }
    }
}

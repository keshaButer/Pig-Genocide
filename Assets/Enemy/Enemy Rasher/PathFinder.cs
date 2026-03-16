using System.Collections.Generic;
using UnityEngine;

public static class PathFinder
{
    public static List<Vector2> FindPath(Vector2 startWorld, Vector2 targetWorld, ChunkedLevelGenerator generator)
    {
        Vector2Int start = generator.WorldCellToIndex(startWorld);
        Vector2Int target = generator.WorldCellToIndex(targetWorld);
        // Debug.Log($"Start world: {startWorld}, start index: {start}, is surface start: {generator.IsSurfaceCell(start + Vector2Int.down)}, is surface target: {generator.IsSurfaceCell(target + Vector2Int.down)}");

        // if (!generator.IsSurfaceCellAround(start) || !generator.IsSurfaceCellAround(target))
        // {
        //     Debug.Log("Start is not surface");
        //     return null;
        // }

        Node startNode = new Node(start);

        List<Node> openSetList = new List<Node>();
        Dictionary<Vector2Int, Node> openSetDictionary = new Dictionary<Vector2Int, Node>();
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();

        openSetList.Add(startNode);
        openSetDictionary[start] = startNode;

        while (openSetList.Count > 0)
        {
            Node currentNode = openSetList[0];
            for (int i = 1; i < openSetList.Count; i++)
            {
                if (openSetList[i].FCost < currentNode.FCost || 
                (openSetList[i].FCost == currentNode.FCost && openSetList[i].HCost < currentNode.HCost))
                {
                    currentNode = openSetList[i];
                }
            }

            if (currentNode.Position == target) 
            {
                return BuildPath(currentNode, generator);
            }

            openSetList.Remove(currentNode);
            openSetDictionary.Remove(currentNode.Position);
            closedSet.Add(currentNode.Position);

            foreach (Vector2Int neighborPos in GetNeighbors(currentNode.Position))
            {
                if (closedSet.Contains(neighborPos))
                    continue;

                if (!generator.IsSurfaceCellAround(neighborPos))
                {
                    Debug.Log("neighborPos is not around surface");
                    continue;
                }

                int newGCost = currentNode.GCost + 1;

                Node neighbor = openSetList.Find(n => n.Position == neighborPos);

                if (!openSetDictionary.ContainsKey(neighborPos))
                {
                    neighbor = new Node(neighborPos);
                    neighbor.GCost = newGCost;
                    neighbor.HCost = Distance(neighborPos, target);
                    neighbor.Parent = currentNode;
                    openSetList.Add(neighbor);
                    openSetDictionary[neighborPos] = neighbor;
                }
                else if (newGCost < neighbor.GCost)
                {
                    neighbor.GCost = newGCost;
                    neighbor.Parent = currentNode;
                }
            }
        }

        Debug.Log($"Path not found. openSetList.Count: {openSetList.Count}, closedSet.Count: {closedSet.Count}");
        return null;
    }

    static List<Vector2Int> GetNeighbors(Vector2Int pos)
    {
        return new List<Vector2Int>
        {
            new Vector2Int(pos.x + 1, pos.y),
            new Vector2Int(pos.x - 1, pos.y),
            new Vector2Int(pos.x, pos.y + 1),
            new Vector2Int(pos.x, pos.y - 1)
        };
    }

    static int Distance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    static List<Vector2> BuildPath(Node lastNode, ChunkedLevelGenerator generator)
    {
        Debug.Log("Building Path...");

        List<Vector2> result = new List<Vector2>();
        Node currentNode = lastNode;
        result.Add(generator.IndexCellToWorld(currentNode.Position));

        while (currentNode.Parent != null)
        {
            result.Add(generator.IndexCellToWorld(currentNode.Parent.Position));
            currentNode = currentNode.Parent;
        }
        result.Reverse();

        return result;
    }
}

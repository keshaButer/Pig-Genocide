using UnityEngine;

public class Node
{
    public Vector2Int Position;
    public Node Parent;
    public int GCost;
    public int HCost;

    public float FCost => GCost + HCost;
    
    public Node(Vector2Int position)
    {
        Position = position;
    }
}

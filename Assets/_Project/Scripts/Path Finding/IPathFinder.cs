using UnityEngine;
using System.Collections.Generic;

public interface IPathFinder
{
    List<Vector2> FindPath(Vector2 startWorld, Vector2 targetWorld, ILevelGenerator generator, int maxFallDepth);
}
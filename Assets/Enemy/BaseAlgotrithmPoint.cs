using System.Collections.Generic;
using UnityEngine;

public class BaseAlgotrithmPoint : MonoBehaviour
{
    public List<Transform> points;
    public bool IsUsing { get; private set; }

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
            points.Add(transform.GetChild(i));
    }
}

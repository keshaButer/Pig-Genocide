using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class PathFollower : MonoBehaviour
{
    public float Speed;
    [SerializeField] private float _reachDistance = 0.1f;

    public bool HasPath => _currentPath != null;
    public bool FinishedPath;

    private Rigidbody2D _rigidBody;
    private List<Vector2> _currentPath;
    private int _currentPoint;

    private void Awake() => _rigidBody = GetComponent<Rigidbody2D>();

    public void SetPath(List<Vector2> path)
    {
        FinishedPath = false;
        _currentPath = path;
        _currentPoint = 0;
    }

    // public Vector2 GetDirectionAlongPath()
    // {
    //     if (_currentPoint >= _currentPath.Count)
    //     {
    //         Stop();
    //         return Vector2.zero;
    //     }
    //
    //     Vector2 target = _currentPath[_currentPoint];
    //     if (Vector2.Distance(_rigidBody.position, target) < _reachDistance)
    //         _currentPoint++;
    //
    //     Vector2 direction = target - _rigidBody.position;
    //
    //     return direction.normalized;
    // }
    public void MoveAlongPath()
    {
        if (_currentPoint >= _currentPath.Count)
        {
            Stop();
            return;
        }

        Vector2 target = _currentPath[_currentPoint] + Vector2.down * 0.15f;
        Vector2 newPos = Vector2.MoveTowards(transform.position, target, Speed * Time.fixedDeltaTime);
        
        transform.position = newPos;

        if (Vector2.Distance(transform.position, target) < _reachDistance)
            _currentPoint++;
    }
    public void Stop()
    {
        FinishedPath = true;
        _currentPath = null;
    }
    private void OnDrawGizmos()
    {
        if (_currentPath == null || _currentPath.Count == 0)
            return;

        Gizmos.color = Color.blue;
        for (int i = 0; i < _currentPath.Count - 1; i++)
        {
            Vector3 start = new Vector3(_currentPath[i].x, _currentPath[i].y, 0);
            Vector3 end = new Vector3(_currentPath[i + 1].x, _currentPath[i + 1].y, 0);
            Gizmos.DrawLine(start, end);
            Gizmos.DrawSphere(start, 0.06f);
        }
        // Последняя точка
        Gizmos.DrawSphere(new Vector3(_currentPath[_currentPath.Count - 1].x, _currentPath[_currentPath.Count - 1].y, 0), 0.1f);
    }
}

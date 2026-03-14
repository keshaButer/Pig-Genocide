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

    public Vector2 GetDirectionAlongPath()
    {
        if (_currentPoint >= _currentPath.Count)
        {
            Stop();
            return Vector2.zero;
        }

        Vector2 target = _currentPath[_currentPoint];
        if (Vector2.Distance(_rigidBody.position, target) < _reachDistance)
            _currentPoint++;

        Vector2 direction = target - _rigidBody.position;

        return direction * Speed * Time.fixedDeltaTime * 10;
    }
    public void Stop()
    {
        FinishedPath = true;
        _currentPath = null;
    }
}

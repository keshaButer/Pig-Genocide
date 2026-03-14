using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class PathFollower : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _reachDistance = 0.1f;

    public bool HasPath => _currentPath != null;

    private Rigidbody2D _rigidBody;
    private List<Vector2> _currentPath;
    private int _currentPoint;

    private void Awake() => _rigidBody = GetComponent<Rigidbody2D>();

    public void SetPath(List<Vector2> path)
    {
        _currentPath = path;
        _currentPoint = 0;
    }

    public void MoveAlongPath()
    {
        if (_currentPath == null || _currentPoint >= _currentPath.Count)
            return;

        Vector2 target = _currentPath[_currentPoint];
        Vector2 newPos = Vector2.MoveTowards(_rigidBody.position, target, _speed * Time.fixedDeltaTime);
        _rigidBody.MovePosition(newPos);

        if (Vector2.Distance(_rigidBody.position, target) < _reachDistance)
            _currentPoint++;
    }

    public bool IsPathComplete()
    {
        return _currentPath == null || _currentPoint >= _currentPath.Count;
    }
    
    public void Stop() => _currentPath = null;
}

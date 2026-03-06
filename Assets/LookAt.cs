using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] Transform target;
    void Update()
    {
        Look(transform, target.position);
    }
    private void Look(Transform _weapon, Vector3 target)
    {
        Vector3 lookAt = _weapon.InverseTransformPoint(target);
        float angle = Mathf.Atan2(lookAt.y, lookAt.x) * Mathf.Rad2Deg;
        _weapon.Rotate(0, 0, angle);
    }
}

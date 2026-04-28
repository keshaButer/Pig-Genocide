using UnityEngine;

public interface IFollowing
{
    bool isChangeDirection { get; set; }
    void Expend(bool turn);
    int CalculateDirection(Vector3 position);
}
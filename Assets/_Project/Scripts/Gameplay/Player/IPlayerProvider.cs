using UnityEngine;

public interface IPlayerProvider
{
    GameObject Player { get; }
    event System.Action<GameObject> OnPlayerSpawned;
}

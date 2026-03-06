using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action EnemyDied, DashDownKick,
    Dash, PlayerTookDamage, PlayerDied, Parry, SatDown, StandUp, Explosion;

    public static void OnEnemyDied() => EnemyDied?.Invoke();
    public static void OnSatDown() => SatDown?.Invoke();
    public static void OnStandUp() => StandUp?.Invoke();
    public static void OnParry() => Parry?.Invoke();
    public static void OnDashDownKick() => DashDownKick?.Invoke();
    public static void OnPlayerTookDamage() => PlayerTookDamage?.Invoke();
    public static void OnPlayerDied() => PlayerDied?.Invoke();
    public static void OnDash() => Dash?.Invoke();
    public static void OnExplosion() => Explosion?.Invoke();

    public static void CleanEvents()
    {
        Parry = null;
        EnemyDied = null;
        DashDownKick = null;
        Dash = null;
        PlayerDied = null;
        PlayerTookDamage = null;
        SatDown = null;
        Explosion = null;
    }
}

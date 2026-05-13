using UnityEngine;

[CreateAssetMenu(fileName = "Sound Config", menuName = "Sound Manager/Sound Config")]
public class SoundManagerConfig : ScriptableObject
{
    [Header("Enemies")]
    public AudioClip EnemyDeath;

    [Header("Player")]
    public AudioClip Dash;
    public AudioClip PlayerDeath;
    public AudioClip Parry;
    //можно еще получение урона звук

    //Надо будет еще добавить при разрушении тайлов чтобы ChunkedLevelGenerator вызывал 
    //событие специального доп интерфейса что разрушен тайл, который будет в soundManager
    //прокидываться по DI
    //и также с бочками и вервками
}
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/InputPlayerMovementCfg", fileName = "InputPlayerMovementConfig")]
public class InputPlayerMovementConfig : ScriptableObject
{
    [field: Header("Dash keys")]
    [field: SerializeField] public KeyCode left { get; private set; } = KeyCode.A;
    [field: SerializeField] public KeyCode right { get; private set; } = KeyCode.D;
    [field: SerializeField] public KeyCode down { get; private set; } = KeyCode.S;
    [field: SerializeField] public KeyCode mainDash { get; private set; } = KeyCode.Space;
    [field: Header("Stels keys")]
    [field: SerializeField] public KeyCode crouch { get; private set; } = KeyCode.LeftControl;
    [field: SerializeField] public KeyCode knifeKill { get; private set; }
    [field: Header("Interaction")]
    [field: SerializeField] public KeyCode interactKey { get; private set; } = KeyCode.E;
    [field: Header("Parry bullets")]
    [field: SerializeField] public KeyCode parryKey { get; private set; } = KeyCode.Space;
}

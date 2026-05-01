using UnityEngine;

[CreateAssetMenu(menuName = "Configs/PropertiesPlayerMovement", fileName = "PropertiesPlayerMovement")]
public class PlayerMovementConfig : ScriptableObject
{
    [field: Header("Speed")]
    [field: SerializeField] public float speed { get; private set; }
    [field: SerializeField] public float crouchSpeed { get; private set; }
    [field: SerializeField] public float fallSpeed { get; private set; }
    [field: SerializeField] public float minFallSpeed { get; private set; }

    [field: Header("Gravity")]
    [field: SerializeField] public float gravityForce { get; private set; } = -9.81f;

    [field: Header("Walking")]
    [field: SerializeField] public float stepHeight { get; private set; }
    [field: SerializeField] public float stepCheckDistance { get; private set; }

    [field: Header("Layers")]
    [field: SerializeField] public LayerMask checkGroundMask { get; private set; }
    [field: SerializeField] public LayerMask checkDashMask { get; private set; }

    [field: Header("Jump")]
    [field: SerializeField] public float jupmHeight { get; private set; }
    [field: SerializeField] public float crouchJupmHeight { get; private set; }
    [field: SerializeField] public float checkCircleRadius { get; private set; }
    [field: SerializeField] public float delayFalseIsGrounded { get; private set; } = 1;
    [field: SerializeField] public float minJumpTime { get; private set; } = 0.5f;
    [field: SerializeField] public float minDelayStopFall { get; private set; } = 0.5f;
    
    [field: Header("Dash")]
    [field: SerializeField] public float timeDoubleClick { get; private set; }
    [field: SerializeField] public float dashDistance { get; private set; }
    [field: SerializeField] public float dashRangeLeftRay { get; private set; }
    [field: SerializeField] public float dashRangeDownRay { get; private set; }
    
    [field: Header("FreeFLy")]
    [field: SerializeField] public float delayResetRB { get; private set; } = 3f;
    [field: SerializeField] public float fallDeathSpeed { get; private set; }
    [field: SerializeField] public float fallDamage { get; private set; }

    [field: Header("Sound")]
    [field: SerializeField] public float delaySoundStep { get; private set; } = 0.5f;
    [field: SerializeField] public AudioClip soundStep { get; private set; }
    [field: SerializeField] public float radiusStep { get; private set; }
}

using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    public static PlayerWeaponHandler SingleTone;

    [SerializeField] float accelaration;
    private float angle;
    public bool wasLeft;
    private float timer;
    private Weapon activeWeapon;
    private float activeWeaponFireInterval;
    private MovementPlayer movementPlayerScript;
    private void Awake()
    {
        if (SingleTone == null)
            SingleTone = this;
        else if (SingleTone != null)
            Destroy(this);
    }
    private void Start()
    {
        movementPlayerScript = transform.parent.GetComponent<MovementPlayer>();
    }
    public void SetWeaponActive(string name, bool turn)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Weapon weapon = transform.GetChild(i).GetComponent<Weapon>();
            if (weapon.item.name == name)
            {
                activeWeapon = weapon;
                activeWeaponFireInterval = weapon.fireInterval;
                weapon.gameObject.SetActive(turn);
                if (!turn)
                    activeWeapon = null;
                break;
            }
        }
    }
    public void SetWeapon(Weapon weapon)
    {
        if (weapon == null)
            return;

        SpriteRenderer sprite;

        activeWeapon = weapon;
        activeWeaponFireInterval = weapon.fireInterval;
        print($"{weapon.item.name} - was taken");
        InventoryWindow.SingleTone.FindCellByItemName(weapon.item.name)?.SelectItem();

        if ((angle > -180 && angle < -90) || (angle < 180 && angle > 90))
        {
            if (activeWeapon != null)
            {
                if (activeWeapon.TryGetComponent<SpriteRenderer>(out sprite))
                {
                    if (sprite != null)
                        sprite.flipY = true;
                }
            }
        }
    }
    private void SetDirectionByMouse()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion rot = Quaternion.Euler(0, 0, angle);
        transform.localRotation = Quaternion.Lerp(transform.rotation, rot, accelaration);

        if (((angle > -180 && angle < -90) || (angle < 180 && angle > 90)) && !wasLeft)
        {
            wasLeft = true;
            movementPlayerScript.Expending(true);

            if (activeWeapon != null && activeWeapon.gameObject.activeSelf)
                activeWeapon.GetComponent<SpriteRenderer>().flipY = true;
        }
        else if (angle < 90 && angle > -90 && wasLeft)
        {
            wasLeft = false;
            movementPlayerScript.Expending(false);

            if (activeWeapon != null && activeWeapon.gameObject.activeSelf)
                activeWeapon.GetComponent<SpriteRenderer>().flipY = false;
        }
    }
    private void Update()
    {
        SetDirectionByMouse();
        UseWeapon();
    }
    private void UseWeapon()
    {
        timer += Time.deltaTime;

        if (timer >= activeWeaponFireInterval && activeWeapon != null && Input.GetMouseButton(0))
        {
            activeWeapon.WeaponAttack();
            
            timer = 0;
        }
    }
}

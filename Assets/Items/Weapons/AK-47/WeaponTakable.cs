using UnityEngine;

public class WeaponTakable : ItemTakable
{
    [SerializeField] private Weapon _weapon;

    protected override void SetItemObjectToHandler(WeaponHandler weaponHandler)
    {
        // SoundManager.SingleTone.PlaySound(item.takeSound, item.soundVolume); //поменять
        weaponHandler.SetWeapon(_weapon);
    }
}

using UnityEngine;

public class WeaponTakable : ItemTakable
{
    [SerializeField] GameObject weaponPrefab;
    public override void Use()
    {
        SetWeaponInSlot();
        PlayerWeaponHandler.SingleTone.SetWeapon(weaponPrefab.GetComponent<Weapon>());
    }
    private void SetWeaponInSlot()
    {
        GameObject obj = Instantiate(weaponPrefab, PlayerWeaponHandler.SingleTone.transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.Euler(0, 0, 0);
        weaponPrefab.GetComponent<Weapon>().Initialize();
        obj.SetActive(false);
        SoundManager.SingleTone.PlaySound(item.takeSound, item.soundVolume);
    }
}

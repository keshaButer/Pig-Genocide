using UnityEngine;

public class GranadeTakable : ItemTakable
{
    [SerializeField] Weapon _weapon;

    protected override void SetItemObjectToHandler(Transform weaponSlotTransform)
    {
        Weapon spawnedWeapon = Instantiate(_weapon, weaponSlotTransform);
        spawnedWeapon.transform.localPosition = Vector3.zero;
        spawnedWeapon.transform.localRotation = Quaternion.Euler(0, 0, 0);

        spawnedWeapon.gameObject.SetActive(false);
        // SoundManager.SingleTone.PlaySound(item.takeSound, item.soundVolume); //поменять
        
    }
}

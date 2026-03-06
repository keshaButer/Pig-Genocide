using UnityEngine;

public class GranadeTakable : ItemTakable
{
    [SerializeField] GameObject weaponPrefab;
    [SerializeField] Transform weaponSlot;

    public override void Use()
    {
        // base.Interact();
        SpawnPrefab();
    }
    private void SpawnPrefab()
    {
        GameObject obj = Instantiate(weaponPrefab, weaponSlot);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.Euler(0, 0, 0);
        obj.SetActive(false);
        SoundManager.SingleTone.PlaySound(item.takeSound, item.soundVolume);
    }
}

using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isFill;
    public Item item;
    public bool isActiveItem;
    [SerializeField] Color defaultColor;
    [SerializeField] Color selectedColor;

    public void SelectItem()
    {
        if (isFill && !Inventory.SingleTone.transform.GetComponent<MovementPlayer>().IsCrouch)
        {
            if (!isActiveItem)
            {
                if (Inventory.SingleTone.currentActiveItem != null)
                {
                    PlayerWeaponHandler.SingleTone.SetWeaponActive(Inventory.SingleTone.currentActiveItem.item.name, false);
                    Inventory.SingleTone.currentActiveItem.isActiveItem = false;
                }
                Inventory.SingleTone.currentActiveItem = this;

                PlayerWeaponHandler.SingleTone.SetWeaponActive(item.name, true);
                isActiveItem = true;
            }
            else
            {
                PlayerWeaponHandler.SingleTone.SetWeaponActive(item.name, false);
                Inventory.SingleTone.currentActiveItem = null;
                isActiveItem = false;
            }
        }
    }
}

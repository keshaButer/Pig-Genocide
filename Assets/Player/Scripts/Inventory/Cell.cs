using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public bool isFill;
    public Item item;
    public bool isActiveItem;
    [SerializeField] Color defaultColor;
    [SerializeField] Color selectedColor;

    public void OnPointerDown(PointerEventData eventData)
    {
        SelectItem();
    }
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
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.parent.parent.GetChild(0).GetChild(int.Parse(gameObject.name)).GetComponent<Image>().color = selectedColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.parent.parent.GetChild(0).GetChild(int.Parse(gameObject.name)).GetComponent<Image>().color = defaultColor;
    }
}

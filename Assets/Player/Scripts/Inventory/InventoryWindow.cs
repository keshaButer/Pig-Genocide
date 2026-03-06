using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryWindow : MonoBehaviour
{
    public static InventoryWindow SingleTone;
    public List<Cell> cells;
    void Awake()
    {
        if (SingleTone == null)
            SingleTone = this;
        else if (SingleTone != null)
            Destroy(this);
    }
    public void Initialize()
    {
        for (int i = 0; i < transform.GetChild(2).childCount; i++)
        {
            cells.Add(transform.GetChild(2).GetChild(i).GetComponent<Cell>());
        }
    }
    public void Redraw()
    {
        for (int i = 0; i < Inventory.SingleTone.items.Count; i++)
        {
            FillCell(Inventory.SingleTone.items[i].icon, cells[i], Inventory.SingleTone.items[i]);
        }
    }
    private void FillCell(Sprite _sprite, Cell _cell, Item item)
    {
        Image image = _cell.GetComponent<Image>();
        image.color = Color.white;
        image.sprite = _sprite;
        _cell.item = item;
        _cell.isFill = true;
    }
    public Cell FindCellByItemName(string name)
    {
        if (cells.Count > 0)
        {
            foreach (Cell cell in cells)
            {
                if (cell.item != null)
                {
                    if (cell.item.name == name)
                        return cell;
                }
            }
        }
        return null;
    }
}

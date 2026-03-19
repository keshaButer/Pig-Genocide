using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [SerializeField] public uint maxItemsCount = 4;
    public List<Item> startItems;
    public List<Item> currentItems;
    public Cell currentActiveItem;
    private Cell tempCurrentActiveItem;

    private void Start()
    {
        AddStartItems();
        // InventoryWindow.SingleTone.Initialize();
        // InventoryWindow.SingleTone.Redraw();
        EventManager.SatDown += DeselectCurrentItem;
        EventManager.StandUp += SelectTempItem;
    }
    private void AddStartItems()
    {
        foreach (Item element in startItems)
        {
            currentItems.Add(element);
        }
    }
    public void AddItem(Item _item)
    {
        // if (items.Count < maxItemsCount)
        // {
        //     items.Add(_item);
        //     InventoryWindow.SingleTone.Redraw();
        // }
        currentItems.Add(_item);
        InventoryWindow.SingleTone.Redraw();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            transform.GetChild(2).GetChild(0)?.GetComponent<Cell>().SelectItem();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            transform.GetChild(2).GetChild(1)?.GetComponent<Cell>().SelectItem();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            transform.GetChild(2).GetChild(2)?.GetComponent<Cell>().SelectItem();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            transform.GetChild(2)?.GetChild(3)?.GetComponent<Cell>()?.SelectItem();
        }
    }
    private void DeselectCurrentItem()
    {
        if (currentActiveItem != null)
        {
            tempCurrentActiveItem = currentActiveItem;
            currentActiveItem.SelectItem();
        }
    }
    private void SelectTempItem()
    {
        if (tempCurrentActiveItem != null)
            tempCurrentActiveItem.SelectItem();
    }
    private void OnDisable()
    {
        EventManager.SatDown -= DeselectCurrentItem;
        EventManager.StandUp -= SelectTempItem;
    }
}

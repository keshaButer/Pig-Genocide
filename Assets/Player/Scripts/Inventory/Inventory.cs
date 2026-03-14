using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public static Inventory SingleTone;
    [SerializeField] public uint maxItemsCount = 4;
    public List<Item> startItems;
    public List<Item> items;
    public Cell currentActiveItem;
    private Cell tempCurrentActiveItem;
    void Awake()
    {
        if (SingleTone == null)
            SingleTone = this;
        else if (SingleTone != null)
            Destroy(this);
    }
    private void Start()
    {
        foreach (Item el in startItems)
        {
            items.Add(el);
        }
        InventoryWindow.SingleTone.Initialize();
        InventoryWindow.SingleTone.Redraw();
        EventManager.SatDown += DeselectCurrentItem;
        EventManager.StandUp += SelectTempItem;
    }
    public void AddItem(Item _item)
    {
        // if (items.Count < maxItemsCount)
        // {
        //     items.Add(_item);
        //     InventoryWindow.SingleTone.Redraw();
        // }
        items.Add(_item);
        InventoryWindow.SingleTone.Redraw();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            transform.GetChild(2).GetChild(0).GetComponent<Cell>().SelectItem();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            transform.GetChild(2).GetChild(1).GetComponent<Cell>().SelectItem();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            transform.GetChild(2).GetChild(2).GetComponent<Cell>().SelectItem();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            transform.GetChild(2).GetChild(3).GetComponent<Cell>().SelectItem();
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
}

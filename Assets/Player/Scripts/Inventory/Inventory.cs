using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private uint _maxItemsCount = 4;
    [SerializeField] private List<Item> _startItems = new List<Item>();

    public event Action OnAddItem;
    public IReadOnlyList<Item> CurrentItems => _currentItems.AsReadOnly();

    private List<Item> _currentItems = new List<Item>();
    // private Cell _currentActiveItem;

    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        AddItems(_startItems);
    }
    private void AddItems(List<Item> items)
    {
        if (_currentItems.Count + items.Count > _maxItemsCount)
            return;

        _currentItems.AddRange(items);
        OnAddItem?.Invoke();
    }
    public void AddItem(Item item)
    {
        if (_currentItems.Count >= _maxItemsCount)
            return;

        _currentItems.Add(item);
        OnAddItem?.Invoke();
    }
}

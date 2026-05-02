using VContainer;
using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform _cellsParent;

    private List<Cell> _cells = new List<Cell>();
    private Inventory _inventory;
    private InventoryInput _inventoryInput;
    
    [Inject]
    public void Construct(IPlayerProvider playerProvider)
    {
        playerProvider.OnPlayerSpawned += OnPlayerSpawned;
        
        if (playerProvider.Player != null)
            OnPlayerSpawned(playerProvider.Player);
    }

    private void OnPlayerSpawned(GameObject player)
    {
        _inventory = player.GetComponent<Inventory>();
        _inventoryInput = player.GetComponent<InventoryInput>();

        _inventory.OnAddItem += Redraw;
        _inventoryInput.OnSelectSlot += SelectCell;

        FillCellsList();
    }

    private void SelectCell(int index)
    {
        if (index > _cells.Count)
        {
            Debug.Log($"Нет столько клеток, чтобы выбрать на этом месте {index} предмет.");
            return;
        }

        foreach (Cell cell in _cells)        
            cell.DeselectCell();
        
        _cells[index - 1].SelectCell();
    }
    private void FillCellsList() 
    {
        for (int i = 0; i < _cellsParent.childCount; i++)
        {
            _cells.Add(_cellsParent.GetChild(i).GetComponent<Cell>());
        }
    }
    private void Redraw()
    {
        if (_cells.Count == 0) return;

        for (int i = 0; i < _inventory.CurrentItems.Count; i++)
        {
            if (i >= _cells.Count)
                break;

            _cells[i].FillCell(_inventory.CurrentItems[i]);
        }
    }
}
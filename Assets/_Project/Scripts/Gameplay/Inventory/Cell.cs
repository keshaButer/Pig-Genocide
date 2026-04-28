using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _selectedColor;

    private Item _item;
    private bool _isFill = false;

    private Image _imageComponent;

    private void Start()
    {
        _imageComponent = GetComponent<Image>();
    }
    public void SelectCell()
    {
        if (_isFill)
            return;

        _imageComponent.color = _selectedColor;
        _isFill = true;
    }
    public void DeselectCell()
    {
        if (!_isFill)
            return;
        
        _imageComponent.color = _defaultColor;
        _isFill = false;
    }
    public void FillCell(Item item)
    {
        _item = item;
        _imageComponent.sprite = _item.Icon;
        _imageComponent.color = _defaultColor;
        _isFill = true;
    }
}

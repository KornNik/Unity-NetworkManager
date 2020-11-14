using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour {

    public Image Icon;
    public Button UnequipButton;
    public Equipment Equipment;

    Item item;

    public void SetItem(Item newItem) {
        item = newItem;
        Icon.sprite = item.Icon;
        Icon.enabled = true;
        UnequipButton.interactable = true;
    }

    public void ClearSlot() {
        item = null;
        Icon.sprite = null;
        Icon.enabled = false;
        UnequipButton.interactable = false;
    }

    public void Unequip() {
        Equipment.UnequipItem(item);
    }
}

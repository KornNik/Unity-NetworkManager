using UnityEngine.Networking;

public class Equipment : NetworkBehaviour
{
    private UserData _data;

    public event SyncList<Item>.SyncListChanged OnItemChanged;
    public SyncListItem Items = new SyncListItem();
    public Player Player;

    public void Load(UserData data)
    {
        _data = data;
        for (int i = 0; i < data.Equipment.Count; i++)
        {
            EquipmentItem item = (EquipmentItem)ItemBase.GetItem(data.Equipment[i]);
            Items.Add(item);
            item.Equip(Player);
        }
    }

    public override void OnStartLocalPlayer()
    {
        Items.Callback += ItemChanged;
    }

    private void ItemChanged(SyncList<Item>.Operation op, int itemIndex)
    {
        OnItemChanged(op, itemIndex);
    }

    public EquipmentItem EquipItem(EquipmentItem item)
    {
        EquipmentItem oldItem = null;
        for (int i = 0; i < Items.Count; i++)
        {
            if (((EquipmentItem)Items[i]).EquipSlot == item.EquipSlot)
            {
                oldItem = (EquipmentItem)Items[i];
                oldItem.Unequip(Player);
                _data.Equipment.Remove(ItemBase.GetItemId(Items[i]));
                Items.RemoveAt(i);
                break;
            }
        }
        Items.Add(item);
        item.Equip(Player);
        _data.Equipment.Add(ItemBase.GetItemId(item));
        return oldItem;
    }

    public void UnequipItem(Item item)
    {
        CmdUnequipItem(Items.IndexOf(item));
    }

    [Command]
    void CmdUnequipItem(int index)
    {
        if (Items[index] != null && Player.Inventory.AddItem(Items[index]))
        {
            ((EquipmentItem)Items[index]).Unequip(Player);
            _data.Equipment.Remove(ItemBase.GetItemId(Items[index]));
            Items.RemoveAt(index);
        }
    }
}

using UnityEngine.Networking;

public class Equipment : NetworkBehaviour
{

    public event SyncList<Item>.SyncListChanged OnItemChanged;
    public SyncListItem Items = new SyncListItem();

    public Player Player;

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
                Items.RemoveAt(i);
                break;
            }
        }
        Items.Add(item);
        item.Equip(Player);
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
            Items.RemoveAt(index);
        }
    }
}

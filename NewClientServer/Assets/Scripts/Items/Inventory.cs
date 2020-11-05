using UnityEngine;
using UnityEngine.Networking;

public class Inventory : NetworkBehaviour
{
    private UserData _data;

    public int Space = 20;
    public Player Player;
    public event SyncList<Item>.SyncListChanged OnItemChanged;
    public SyncListItem Items = new SyncListItem();

    public void Load(UserData data)
    {
        _data = data;
        for (int i = 0; i < data.Inventory.Count; i++)
        {
            Items.Add(ItemBase.GetItem(data.Inventory[i]));
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

    public bool AddItem(Item item)
    {
        if (Items.Count < Space)
        {
            Items.Add(item);
            _data.Inventory.Add(ItemBase.GetItemId(item));
            return true;
        }
        else return false;
    }

    public void UseItem(Item item)
    {
        CmdUseItem(Items.IndexOf(item));
    }

    public void DropItem(Item item)
    {
        CmdDropItem(Items.IndexOf(item));
    }

    public void RemoveItem(Item item)
    {
        Items.Remove(item);
        _data.Inventory.Remove(ItemBase.GetItemId(item));
    }

    private void Drop(Item item)
    {
        ItemPickup pickupItem = Instantiate(item.PickupPrefab, Player.Character.transform.position,
            Quaternion.Euler(0, Random.Range(0, 360f), 0));
        pickupItem.Item = item;
        NetworkServer.Spawn(pickupItem.gameObject);
    }

    [Command]
    private void CmdUseItem(int index)
    {
        if (Items[index] != null)
        {
            Items[index].Use(Player);
        }
    }

    [Command]
    private void CmdDropItem(int index)
    {
        if (Items[index] != null)
        {
            Drop(Items[index]);
            RemoveItem(Items[index]);
        }
    }

}
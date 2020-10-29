using UnityEngine;

[CreateAssetMenu(fileName = "New equipment", menuName = "Inventory/Equipment")]
public class EquipmentItem : Item {

    public EquipmentSlotType EquipSlot;

    public int DamageModifier;
    public int ArmorModifier;
    public int SpeedModifier;

    public override void Use(Player player) {
        player.Inventory.RemoveItem(this);
        EquipmentItem oldItem = player.Equipment.EquipItem(this);
        if (oldItem != null) player.Inventory.AddItem(oldItem);
        base.Use(player);
    }

    public virtual void Equip(Player player)
    {
        if (player != null)
        {
            UnitStats unitStats = player.Character.UnitStats;
            unitStats.Damage.AddModifier(DamageModifier);
            unitStats.Armor.AddModifier(ArmorModifier);
            unitStats.MoveSpeed.AddModifier(SpeedModifier);
        }
    }

    public virtual void Unequip(Player player)
    {
        if (player != null)
        {
            UnitStats unitStats = player.Character.UnitStats;
            unitStats.Damage.RemoveModifier(DamageModifier);
            unitStats.Armor.RemoveModifier(ArmorModifier);
            unitStats.MoveSpeed.RemoveModifier(SpeedModifier);
        }
    }
}

public enum EquipmentSlotType { Head, Chest, Legs, RighHand, LeftHand }
using UnityEngine;
using UnityEngine.Networking;

public class StatsManager : NetworkBehaviour
{

    [SyncVar] public int Damage;
    [SyncVar] public int Armor;
    [SyncVar] public int MoveSpeed;
    [SyncVar] public int Level;
    [SyncVar] public int StatPoints;
    [SyncVar] public float Exp;
    [SyncVar] public float NextLevelExp;

    public Player Player;

    [Command]
    public void CmdUpgradeStat(int stat)
    {
        if (Player.Progress.RemoveStatPoint())
        {
            switch (stat)
            {
                case (int)StatType.Damage:
                    Player.Character.UnitStats.Damage.baseValue++;
                    break;
                case (int)StatType.Armor:
                    Player.Character.UnitStats.Armor.baseValue++;
                    break;
                case (int)StatType.MoveSpeed:
                    Player.Character.UnitStats.MoveSpeed.baseValue++;
                    break;
            }
        }
    }
}

public enum StatType { Damage, Armor, MoveSpeed }
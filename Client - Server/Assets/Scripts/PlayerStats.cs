using UnityEngine;

public class PlayerStats : UnitStats
{
    private StatsManager _statsManager;

    public StatsManager StatsManager
    {
        set
        {
            _statsManager = value;
            _statsManager.Damage = Damage.GetValue();
            _statsManager.Armor = Armor.GetValue();
            _statsManager.MoveSpeed = MoveSpeed.GetValue();
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Damage.OnStatChanged += DamageChanged;
        Armor.OnStatChanged += ArmorChanged;
        MoveSpeed.OnStatChanged += MoveSpeedChanged;
    }

    private void DamageChanged(int value)
    {
        if (_statsManager != null) _statsManager.Damage = value;
    }

    private void ArmorChanged(int value)
    {
        if (_statsManager != null) _statsManager.Armor = value;
    }

    private void MoveSpeedChanged(int value)
    {
        if (_statsManager != null) _statsManager.MoveSpeed = value;
    }

}

using UnityEngine;

public class PlayerStats : UnitStats
{
    private StatsManager _statsManager;
    private UserData _data;

    public override int currHealth
    {
        get { return base.currHealth; }
        protected set
        {
            base.currHealth = value;
            _data.CurHealth = currHealth;
        }
    }

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

    public void Load(UserData data)
    {
        _data = data;
        currHealth = _data.CurHealth;
        if (_data.StatDamage > 0)
        {
            Damage.baseValue = _data.StatDamage;
        }

        if (_data.StatArmor > 0)
        {
            Armor.baseValue = _data.StatArmor;
        }

        if (_data.StatMoveSpeed > 0)
        {
            MoveSpeed.baseValue = _data.StatMoveSpeed;
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
        if (Damage.baseValue != _data.StatDamage)
        {
            _data.StatDamage = Damage.baseValue;
        }
        if (_statsManager != null) _statsManager.Damage = value;
    }

    private void ArmorChanged(int value)
    {
        if (Armor.baseValue != _data.StatArmor)
        {
            _data.StatArmor = Armor.baseValue;
        }
        if (_statsManager != null) _statsManager.Armor = value;
    }

    private void MoveSpeedChanged(int value)
    {
        if (MoveSpeed.baseValue != _data.StatMoveSpeed)
        {
            _data.StatMoveSpeed = MoveSpeed.baseValue;
        }
        if (_statsManager != null) _statsManager.MoveSpeed = value;
    }

}

using UnityEngine;
using UnityEngine.Networking;

public class UnitStats : NetworkBehaviour
{
    [SerializeField] protected int _maxHealth;
    [SyncVar] private int _currHealth;

    public Stat Damage;
    public Stat Armor;
    public Stat MoveSpeed;

    public virtual int CurrHealth
    {
        get { return _currHealth; }
        protected set { _currHealth = value; }
    }

    public virtual void TakeDamage(int damage)
    {
        damage -= Armor.GetValue();
        if (damage > 0) { CurrHealth -= damage; }
        if (CurrHealth <= 0) { CurrHealth = 0; }

    }

    public void SetHealthRate(float rate)
    {
        CurrHealth = rate == 0 ? 0 : (int)(_maxHealth / rate);
    }
    public void AddHealth(int amount)
    {
        CurrHealth += amount;
        if (CurrHealth > _maxHealth) { CurrHealth = _maxHealth; }
    }
}


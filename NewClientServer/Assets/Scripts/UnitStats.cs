using UnityEngine;
using UnityEngine.Networking;

public class UnitStats : NetworkBehaviour
{
    [SerializeField] protected int _maxHealth;
    [SyncVar] private int _currHealth;

    public Stat Damage;
    public Stat Armor;
    public Stat MoveSpeed;

    public virtual int currHealth
    {
        get { return _currHealth; }
        protected set { _currHealth = value; }
    }

    public virtual void TakeDamage(int damage)
    {
        damage -= Armor.GetValue();
        if (damage > 0) { currHealth -= damage; }
        if (currHealth <= 0) { currHealth = 0; }

    }

    public void SetHealthRate(float rate)
    {
        currHealth = rate == 0 ? 0 : (int)(_maxHealth / rate);
    }
}


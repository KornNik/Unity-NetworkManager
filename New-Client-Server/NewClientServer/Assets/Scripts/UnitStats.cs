using UnityEngine;
using UnityEngine.Networking;

public class UnitStats : NetworkBehaviour
{
    [SerializeField] private int _maxHealth;
    [SyncVar] private int _currHealth;

    public Stat Damage;
    public Stat Armor;
    public Stat MoveSpeed;

    public int currHealth { get { return _currHealth; } }

    public override void OnStartServer()
    {
        _currHealth = _maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        damage -= Armor.GetValue();
        if (damage > 0) { _currHealth -= damage; }
        if (_currHealth <= 0) { _currHealth = 0; }

    }

    public void SetHealthRate(float rate)
    {
        _currHealth = rate == 0 ? 0 : (int)(_maxHealth / rate);
    }
}


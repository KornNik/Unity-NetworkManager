using UnityEngine;
using UnityEngine.Networking;
class EnemyStats : UnitStats
{
    public override void OnStartServer()
    {
        CurrHealth = _maxHealth;
    }

}


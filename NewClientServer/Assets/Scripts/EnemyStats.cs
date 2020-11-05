using UnityEngine;
using UnityEngine.Networking;
class EnemyStats : UnitStats
{
    public override void OnStartServer()
    {
        currHealth = _maxHealth;
    }

}


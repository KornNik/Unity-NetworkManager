using UnityEngine;
using UnityEngine.Networking;
using System;

public class Unit : Interactable
{
    [SerializeField] private UnitMotor _motor;
    [SerializeField] private UnitStats _unitStats;

    protected Interactable _focus;
    protected bool _isDead;
    protected float _interactDistance;

    public event Action EventOnDamage;
    public event Action EventOnDie;
    public event Action EventOnRevive;

    public UnitSkills UnitSkills;

    public UnitStats UnitStats { get { return _unitStats; } }
    public UnitMotor Motor { get { return _motor; } }
    public Interactable Focus { get { return _focus; } }

    public override void OnStartServer()
    {
        _motor.SetMoveSpeed(_unitStats.MoveSpeed.GetValue());
        _unitStats.MoveSpeed.OnStatChanged += _motor.SetMoveSpeed;
    }
    public override float GetInteractDistance(GameObject user)
    {
        Combat combat = user.GetComponent<Combat>();
        return base.GetInteractDistance(user) + (combat != null ? combat.AttackDistance : 0f);
    }

    private void Update()
    {
        OnUpdate();
    }

    protected virtual void OnAliveUpdate() { }
    protected virtual void OnDeadUpdate() { }

    protected void OnUpdate()
    {
        if(isServer)
        {
            if (!_isDead)
            {
                if(_unitStats.CurrHealth == 0) { Die(); }
                else { OnAliveUpdate(); }
            }
            else { OnDeadUpdate(); }
        }
    }

    public virtual void SetFocus(Interactable newFocus)
    {
        if (newFocus != _focus)
        {
            _focus = newFocus;
            _interactDistance = _focus.GetInteractDistance(gameObject);
            _motor.FollowTarget(newFocus, _interactDistance);
        }
    }

    public virtual void RemoveFocus()
    {
        _focus = null;
        _motor.StopFollowingTarget();
    }
    protected virtual void DamageWithCombat(GameObject user)
    {
        EventOnDamage();
    }

    public override bool Interact(GameObject user)
    {
        Combat combat = user.GetComponent<Combat>();
        if (combat != null)
        {
            if (combat.Attack(_unitStats)) { DamageWithCombat(user); }
            return true;
        }
        return base.Interact(user);
    }

    public void UseSkill(int skillNum)
    {
        if (!_isDead && skillNum < UnitSkills.Count)
        {
            UnitSkills[skillNum].Use(this);
        }
    }

    public void TakeDamage(GameObject user, int damage)
    {
        _unitStats.TakeDamage(damage);
        DamageWithCombat(user);
    }


    [ClientCallback]
    protected virtual void Die()
    {
        _isDead = true;
        GetComponent<Collider>().enabled = false;
        EventOnDie();
        if (isServer)
        {
            HasInteract = false;
            RemoveFocus();
            _motor.MoveToPoint(transform.position);
            RpcDie();
        }
    }

    [ClientRpc]
    void RpcDie()
    {
        if (!isServer) { Die(); }
    }

    [ClientCallback]
    protected virtual void Revive()
    {
        _isDead = false;
        GetComponent<Collider>().enabled = true;
        EventOnRevive();
        if (isServer)
        {
            HasInteract = true;
            _unitStats.SetHealthRate(1);
            RpcRevive();
        }
    }

    [ClientRpc]
    void RpcRevive()
    {
        if (!isServer) { Revive(); }
    }

}


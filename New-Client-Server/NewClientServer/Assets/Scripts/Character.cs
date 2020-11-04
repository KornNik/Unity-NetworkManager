using UnityEngine;

[RequireComponent(typeof(UnitMotor), typeof(PlayerStats))]
public class Character : Unit
{
    [SerializeField] private float _reviveDelay = 5f;

    private Vector3 _startPosition;
    private float _reviveTime;

    public Player Player;

    public PlayerStats PlayerStats { get { return _unitStats as PlayerStats; } }

    private void Start()
    {
        _startPosition = transform.position;
        _reviveTime = _reviveDelay;
    }

    private void Update()
    {
        OnUpdate();
    }

    protected override void OnDeadUpdate()
    {
        base.OnDeadUpdate();
        if (_reviveTime > 0) { _reviveTime -= Time.deltaTime; }
        else { _reviveTime = _reviveDelay; Revive(); }
    }

    protected override void OnAliveUpdate()
    {
        base.OnAliveUpdate();
        if (_focus != null)
        {
            if (!_focus.HasInteracte) { RemoveFocus(); }
            else
            {
                float distance = Vector3.Distance(_focus.InterectionTransform.position, transform.position);
                if (distance <= _focus.Radius) { if (!_focus.Interact(gameObject)) { RemoveFocus(); } }
            }
        }
    }

    protected override void Die()
    {
        base.Die();
    }

    protected override void Revive()
    {
        base.Revive();
        transform.position = _startPosition;
        if (isServer) { _motor.MoveToPoint(_startPosition); }
    }

    public void SetMovePoint(Vector3 point)
    {
        if (!_isDead) { RemoveFocus(); _motor.MoveToPoint(point); }
    }

    public void SetNewFocus(Interactable newFocus)
    {
        if (!_isDead)
        {
            if (newFocus.HasInteracte) { SetFocus(newFocus); }
        }
    }
}

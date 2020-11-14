using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(UnitMotor), typeof(EnemyStats))]
public class Enemy : Unit
{
    [Header("Movement")]
    [SerializeField] private float _moveRadius = 10f;
    [SerializeField] private float _minMoveDelay = 4f;
    [SerializeField] private float _maxMoveDelay = 12f;
    private Vector3 _startPosition;
    private Vector3 _currentDistanation;
    private float _changePositionTime;

    [Header("Behavioure")]
    [SerializeField] private bool _isAggressive;
    [SerializeField] private float _viewDistance = 8f;
    [SerializeField] private float _reviveDelay;
    [SerializeField] private float _rewardExp;
    [SerializeField] private float _agroDistance = 5f;


    private float _reviveTime;

    private List<Character> _enemies = new List<Character>();

    private void Start()
    {
        _startPosition = transform.position;
        _reviveTime = _reviveDelay;
        _changePositionTime = Random.Range(_minMoveDelay, _maxMoveDelay);
    }

    private void Update()
    {
        OnUpdate();
    }
    protected override void DamageWithCombat(GameObject user)
    {
        base.DamageWithCombat(user);
        Unit enemy = user.GetComponent<Unit>();
        if (enemy != null)
        {
            SetFocus(enemy.GetComponent<Interactable>());
            Character character = enemy as Character;
            if (character != null && !_enemies.Contains(character))
            {
                _enemies.Add(character);
            }
        }
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
        if (_focus == null)
        {
            Wandering(Time.deltaTime);
            if (_isAggressive) { FindEnemy(); }
        }
        else
        {
            float distance = Vector3.Distance(_focus.InterectionTransform.position, transform.position);
            if (distance > _viewDistance || !_focus.HasInteract) { RemoveFocus(); }
            else if (distance <= _interactDistance)
            {
                if (!_focus.Interact(gameObject)) { RemoveFocus(); }
            }
        }
    }

    protected override void Revive()
    {
        base.Revive();
        transform.position = _startPosition;
        if (isServer) { Motor.MoveToPoint(_startPosition); }
    }

    protected override void Die()
    {
        base.Die();
        if (isServer)
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].Player.Progress.AddExp(_rewardExp / _enemies.Count);
            }
            _enemies.Clear();
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _viewDistance);
    }

    private void Wandering(float deltaTime)
    {
        _changePositionTime -= deltaTime;
        if (_changePositionTime <= 0)
        {
            RandomMove();
            _changePositionTime = Random.Range(_minMoveDelay, _maxMoveDelay);
        }
    }
    
    private void RandomMove()
    {
        _currentDistanation = Quaternion.AngleAxis(Random.Range(0f, 360f), 
            Vector3.up) * new Vector3(_moveRadius, 0, 0) + _startPosition;
        Motor.MoveToPoint(_currentDistanation);
    }

    private void FindEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _agroDistance,
            1 << LayerMask.NameToLayer("Player"));
        for (int i = 0; i < colliders.Length; i++)
        {
            Interactable interactable = colliders[i].GetComponent<Interactable>();
            if (interactable != null && interactable.HasInteract) { SetFocus(interactable); break; }
        }
    }
}
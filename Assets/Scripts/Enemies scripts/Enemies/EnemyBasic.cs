using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

[RequireComponent(typeof(StateMachine_BasicEnemy))]
public class EnemyBasic : BaseEnemy
{
    public Transform target { get { return _target; } }
    public EnemyBossBasic boss { get; private set; }
    public float life;
    public float researchTime = 10;
    private float _speedRotation = 50f;
    private Transform _bulletSpawner;
    bool haveBoss = false;

    void Update()
    {
        _currentLife = life;
    }

    /// <summary> The return value is of 0 to 1. </summary>
    public float GetPercentageCurrentLife { get { return _currentLife / _maxLife; } }

    internal void _Attaking()
    {
        _currentAttack.Attack(_bulletSpawner.transform);
    }

    internal void _Moving()
    {
        transform.position -= _currentMove.Move(transform, _target, _speed);
        transform.LookAt(_target.transform);
    }

    internal void _StartSearching()
    {
        var array = FindObjectsOfType<EnemyBossBasic>();
        if (array.Length > 0 && !haveBoss)
        {
            var random = Random.Range(1, array.Length);
            _target = array[random].transform;
        }
        else
        {
            haveBoss = false;
            StartCoroutine(RestartSearch());
        }
    }

    private IEnumerator RestartSearch()
    {
        yield return new WaitForSeconds(researchTime);
        _StartSearching();
    }

    protected override void Strategies() {
        //Move behaviors
        IMoveEnemy move1 = new MoveSeekEnemy();
        IMoveEnemy move2 = new MoveFleeEnemy();

        _moveBehaviours = new Dictionary<string, IMoveEnemy>();
        _moveBehaviours.Add("MoveSeek", move1);
        _moveBehaviours.Add("MoveFlee", move2);

        _currentMove = _moveBehaviours["MoveSeek"];

        //Attack behaviors
        IAttackEnemy attack1 = new AttackSimple();

        _attackBehaviours = new Dictionary<string, IAttackEnemy>();
        _attackBehaviours.Add("AttackSimple", attack1);

       _currentAttack = _attackBehaviours["AttackSimple"];
    }

    protected override void Damage(float value)
    {
        base.Damage(value);
    }

    protected override void Die()
    {
        print("Die: Basic");
        Destroy(this.gameObject);
    }

    private void Start()
    {
        Strategies();
        _target = FindObjectOfType<Hero>().gameObject.transform;
        _bulletSpawner = GetComponentInChildren<Reference>().gameObject.transform;
    }
}

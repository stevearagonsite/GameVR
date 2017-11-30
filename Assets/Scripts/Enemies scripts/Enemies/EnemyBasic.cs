using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

[RequireComponent(typeof(StateMachine_BasicEnemy))]
public class EnemyBasic : BaseEnemy
{
    public GameObject Arm;
    public Transform bulletSpawner;

    public EnemyBossBasic boss { get; private set; }
    public Hero hero { get; private set; }
    public Transform target { get { return _target; } }
    internal void SetTarget(Transform value) { _target = value; }

    public float life = 100;
    public float researchTime = 10;

    public const string KEY_MOVE_SEEK = "MoveSeek";
    public const string KEY_MOVE_FLEE = "MoveFlee";
    public const string KEY_ATTACK_SIMPLE = "AttackSimple";

    private void Awake()
    {
        Strategies();
        hero = FindObjectOfType<Hero>();
        _StartSearching();
        ManagerUpdate.instance.updateFixed -= Execute;
        _speed = 4;
    }



    protected override void Strategies()
    {
        //Move behaviors
        IMoveEnemy move1 = new MoveSeekEnemy();
        IMoveEnemy move2 = new MoveFleeEnemy();

        _moveBehaviours = new Dictionary<string, IMoveEnemy>();
        _moveBehaviours.Add(KEY_MOVE_SEEK, move1);
        _moveBehaviours.Add(KEY_MOVE_FLEE, move2);

        _currentMove = _moveBehaviours[KEY_MOVE_SEEK];

        //Attack behaviors
        IAttackEnemy attack1 = new AttackSimple();

        _attackBehaviours = new Dictionary<string, IAttackEnemy>();
        _attackBehaviours.Add(KEY_ATTACK_SIMPLE, attack1);

        _currentAttack = _attackBehaviours[KEY_ATTACK_SIMPLE];
    }

    void Execute()
    {
        _currentLife = life;
    }

    /// <summary> The return value is of 0 to 1. </summary>
    public float GetPercentageCurrentLife { get { return _currentLife / _maxLife; } }

    internal void _Attaking()
    {
        _currentAttack.Attack(bulletSpawner.transform);
        Arm.transform.LookAt(target.transform);

    }

    internal void _Moving(Transform target)
    {
        transform.position += _currentMove.Move(transform, target, _speed);
        Arm.transform.LookAt(target.transform);

    }

    internal void _ChangeMoving(string value)
    {
        _currentMove = _moveBehaviours[value];
    }

    internal void _StartSearching()
    {
        var array = FindObjectsOfType<EnemyBossBasic>();
        if (array.Length > 0 && boss == null)
        {
            var random = Random.Range(1, array.Length) - 1;
            boss = array[random];
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
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

    private void OnDestroy()
    {
        ManagerUpdate.instance.updateFixed -= Execute;
    }
}

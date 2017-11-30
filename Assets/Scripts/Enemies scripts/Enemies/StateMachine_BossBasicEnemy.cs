using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FP;

[RequireComponent(typeof(EnemyBossBasic))]
public class StateMachine_BossBasicEnemy : MonoBehaviour {

    public enum EnemyBossBasicAction { Null, Pursuit, Attack, Damage }
    EventFSM<EnemyBossBasicAction> _fsm;
    private EnemyBossBasic _entity;
    private float _minDistanceAttack = 10;
    bool _isFurius = false;

    void Start () {
        _entity = GetComponent<EnemyBossBasic>();
        ManagerUpdate.instance.update += Execute;

        var Chasing = new State<EnemyBossBasicAction>("Chasing");
        var Attacking = new State<EnemyBossBasicAction>("Attacking");
        var Damaged = new State<EnemyBossBasicAction>("Damaged");

        Chasing
            .SetTransition(EnemyBossBasicAction.Attack, Attacking)
            .SetTransition(EnemyBossBasicAction.Damage, Damaged)
            ;
        Attacking
            .SetTransition(EnemyBossBasicAction.Pursuit, Chasing)
            .SetTransition(EnemyBossBasicAction.Damage, Damaged)
            ;
        Damaged
            .SetTransition(EnemyBossBasicAction.Pursuit, Chasing)
            .SetTransition(EnemyBossBasicAction.Attack, Attacking)
            ;

        //Pursuit fsm.
        Chasing[EnemyBossBasicAction.Attack].OnTransition += () => {
            print("Transition attack-pursuit enemy boss base");
        };

        Chasing.OnEnter += () => {
            print("Enter Chasing enemy boss base");
        };

        Chasing.OnUpdate += () => {
            _entity._Moving();
        };

        Chasing.OnExit += () => {
            print("Exit Chasing enemy boss base");
        };

        //Attack fsm.
        Attacking[EnemyBossBasicAction.Pursuit].OnTransition += () => {
            print("Transition pursuit-attack enemy boss base");
        };

        Attacking.OnUpdate += () => {
            print("Update Attackin genemy boss base");
            _entity._Attaking();
        };

        _fsm = new EventFSM<EnemyBossBasicAction>(Chasing);
    }

    private void Execute()
    {
        if (!_fsm.Equals(EnemyBossBasicAction.Attack)
            && Vector3.Distance(_entity.transform.position, _entity.target.position) < _minDistanceAttack){

            _fsm.Feed(EnemyBossBasicAction.Attack);
        }
        else if (!_fsm.Equals(EnemyBossBasicAction.Attack)){

            _fsm.Feed(EnemyBossBasicAction.Pursuit);
        }

        _fsm.Update();
    }
}

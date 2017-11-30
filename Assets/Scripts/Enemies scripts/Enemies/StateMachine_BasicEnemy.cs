using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FP;

[RequireComponent(typeof(EnemyBasic))]
public class StateMachine_BasicEnemy : MonoBehaviour {

    public enum EnemyBasicAction { Null, Pursuit, Attack, Search, Damage, Idle }
    EventFSM<EnemyBasicAction> _fsm;
    private EnemyBasic _entity;

    void Start () {
        _entity = GetComponent<EnemyBasic>();
        ManagerUpdate.instance.update += Execute;

        var Chasing = new State<EnemyBasicAction>("Chasing");
        var Attacking = new State<EnemyBasicAction>("Attacking");
        var Searching = new State<EnemyBasicAction>("Searching");
        var Damaged = new State<EnemyBasicAction>("Damaged");


        Chasing
            .SetTransition(EnemyBasicAction.Attack, Attacking)
            .SetTransition(EnemyBasicAction.Damage, Damaged)
            .SetTransition(EnemyBasicAction.Search, Searching)
            ;
        Attacking
            .SetTransition(EnemyBasicAction.Pursuit, Chasing)
            .SetTransition(EnemyBasicAction.Damage, Damaged)
            .SetTransition(EnemyBasicAction.Search, Searching)
            ;
        Damaged
            .SetTransition(EnemyBasicAction.Pursuit, Chasing)
            .SetTransition(EnemyBasicAction.Attack, Attacking)
            .SetTransition(EnemyBasicAction.Search, Searching)
            ;
        Searching
            .SetTransition(EnemyBasicAction.Pursuit, Chasing)
            .SetTransition(EnemyBasicAction.Damage, Damaged)
            ;

        //Pursuit fsm.
        Chasing[EnemyBasicAction.Attack].OnTransition += () => {
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
        Attacking[EnemyBasicAction.Pursuit].OnTransition += () => {
            print("Transition pursuit-attack enemy boss base");
        };

        Attacking.OnUpdate += () => {
            print("Update Attackin genemy boss base");
            _entity._Attaking();
        };

        _fsm = new EventFSM<EnemyBasicAction>(Chasing);
    }

    private void Execute()
    {
        if (!_fsm.Equals(EnemyBasicAction.Attack)
             && Vector3.Distance(_entity.transform.position, _entity.target.position) < 10)
        {

            _fsm.Feed(EnemyBasicAction.Attack);
        }
        else if (!_fsm.Equals(EnemyBasicAction.Attack))
        {

            _fsm.Feed(EnemyBasicAction.Pursuit);
        }
    }
}

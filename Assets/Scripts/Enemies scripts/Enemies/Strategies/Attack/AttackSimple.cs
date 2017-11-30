using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSimple : MonoBehaviour, IAttackEnemy{
    public void Attack(Transform reference){
        print("SimpleAttack");
    }
}

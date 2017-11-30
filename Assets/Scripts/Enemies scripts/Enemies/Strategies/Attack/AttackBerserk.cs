using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBerserk : MonoBehaviour, IAttackEnemy{
    public void Attack(Transform reference){
        print("SimpleBerserk");
    }
}

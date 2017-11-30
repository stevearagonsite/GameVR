using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSimple : MonoBehaviour, IAttackEnemy{

    string _path = "Bullets/Bullet";
    float _timeShoot = 5;
    float _currentTimeShoot = 5;

    public void Attack(Transform reference){
        _currentTimeShoot -= Time.deltaTime;

        if (_currentTimeShoot < 0){
            _currentTimeShoot = _timeShoot;
            var bullet = Instantiate(Resources.Load(_path, typeof(Bullet))) as Bullet;
            bullet.transform.position = reference.position;
            bullet.transform.rotation = reference.rotation;
        }
    }
}

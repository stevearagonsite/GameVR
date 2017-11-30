using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSinEnemy : IMoveEnemy
{
    float _wavySpeed = 5;
    float _wavyTime = 1f;

    public Vector3 Move(Transform entity, Transform target, float speed)
    {
        var dir = Vector3.Normalize(entity.position - target.position) * speed * Time.deltaTime;
        _wavyTime += Time.deltaTime;

        var vector = entity.right * Mathf.Sin(_wavySpeed * _wavyTime) * Time.deltaTime * 10f;

        return (dir + vector);
    }
}

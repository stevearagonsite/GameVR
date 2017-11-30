using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSinIdleEnemy : IMoveEnemy
{
    float _wavySpeed = 5;
    float _wavyTime = 1f;

    public Vector3 Move(Transform entity, Transform target, float speed)
    {
        _wavyTime += Time.deltaTime;

        var vector = entity.right * Mathf.Sin(_wavySpeed * _wavyTime) * Time.deltaTime * 10f;

        return vector;
    }
}

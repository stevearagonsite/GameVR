using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForwardEnemy : IMoveEnemy
{

    public Vector3 Move(Transform entity, Transform target, float speed)
    {
        return Vector3.Normalize(entity.position - target.position) * speed * Time.deltaTime;
    }
}

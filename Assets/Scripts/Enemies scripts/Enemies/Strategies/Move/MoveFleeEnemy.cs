using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFleeEnemy : IMoveEnemy{

    public float velocityLimit = 5f;

    public Vector3 Move(Transform entity, Transform target, float speed)
    {
        var deltaPos = target.position - entity.position;
        var desiredVel = -deltaPos.normalized * speed;        //La velocidad deseada es la OPUESTA a seek
        return desiredVel;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveEnemy{
    Vector3 Move(Transform Entity, Transform target, float speed);
}

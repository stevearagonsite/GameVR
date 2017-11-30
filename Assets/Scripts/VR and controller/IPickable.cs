using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable
{
    void PickUp(Transform toFollow);

    void Release();
}

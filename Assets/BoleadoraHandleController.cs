using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoleadoraHandleController : MonoBehaviour, IPickable
{
    private Transform _Following;
    public Action OnReleasedCallback;

    public BoleadoraHandleController AssignReleasedCallback(Action callback)
    {
        OnReleasedCallback = callback;
        return this;
    }

    private void Update()
    {
        if(_Following != null)
            gameObject.transform.position = _Following.position;
    }

    public void PickUp(Transform toFollow)
    {
        _Following = toFollow;
    }

    public void Release()
    {
        _Following = null;
        if (OnReleasedCallback != null)
            OnReleasedCallback();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ManagerUpdate : MonoBehaviour {

    public event Action update;
    public event Action updateFixed;
    public event Action updateLate;
    public event Action coroutines;
    public bool _isPause;
    public static ManagerUpdate instance;

    ManagerUpdate()
    {
        //Singleton
        if(instance != null)
        {
            instance = null;
            instance = this;
        }
        else
        {
            instance = this;
        }
    }

    private void Awake()
    {
        Clean();
        StartCoroutine(Coroutines());
        _isPause = false;
    }

    public void Pause()
    {
        _isPause = true;
    }

    public void Unpause()
    {
        _isPause = false;
    }

    private void Clean()
    {
        StopCoroutine(Coroutines());
        update = null;
        updateLate = null;
        updateFixed = null;
        coroutines = null;
    }

    #region UPDATES
    private IEnumerator Coroutines()
    {
        while (true)
        {
            if (coroutines != null && !_isPause)
            {
                coroutines();
            }
            yield return null;
        }
    }

    private void Update()
    {
        if (update != null && !_isPause)
        {
            update();
        }
    }

    private void LateUpdate()
    {
        if (updateLate != null && !_isPause)
            updateLate();
    }

    private void FixedUpdate()
    {
        if (updateFixed != null && !_isPause)
            updateFixed();
    }
    #endregion UPDATES
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private float _speed = 5;
    private float _timeDestroy = 10f;//in seconds.

    private void Start(){
        StartCoroutine(TimeDestroy());
    }

    private IEnumerator TimeDestroy(){
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    void Update () {
        transform.position += transform.forward * _speed * Time.deltaTime;
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoleadoraController : MonoBehaviour
{
    Rigidbody _rb;
    private bool _released;
    BoleadoraHandleController handle;
    public Rigidbody extreme;

    private void Awake()
    {
        handle = gameObject.GetComponentInChildren<BoleadoraHandleController>();
        handle.AssignReleasedCallback(Released);
        _rb = gameObject.GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _rb.isKinematic = true;
    }

    private void FixedUpdate()
    {
        if (!_released) return;
        _rb.MoveRotation(_rb.rotation * Quaternion.AngleAxis(15, transform.right));
    }

    private void Released()
    {
        var fullChain = gameObject.GetComponentsInChildren<Rigidbody>().Where(x => x != _rb);
        var pos = new Dictionary<GameObject, Vector3>();
        foreach (var e in fullChain)
        {
            e.useGravity = false;
            pos.Add(e.gameObject, e.transform.position);
        }

        gameObject.transform.position = handle.gameObject.transform.position;

        foreach(var elem in fullChain)
            elem.gameObject.transform.localPosition = gameObject.transform.InverseTransformPoint(pos[elem.gameObject]);

        _rb.useGravity = true;
        _rb.isKinematic = false;
        _rb.AddForce(extreme.velocity, ForceMode.Impulse);
        _released = true;
    }
}

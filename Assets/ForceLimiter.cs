using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceLimiter : MonoBehaviour
{
    private Rigidbody _rb;
    private void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
    }

	void FixedUpdate ()
    {
        if (_rb.velocity.sqrMagnitude > 1500f)
            _rb.velocity = _rb.velocity.normalized * Mathf.Sqrt(1500f);
	}
}

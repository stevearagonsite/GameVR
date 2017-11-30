using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DispatchEvent(GameObject sender);
public class ManagerEvent : MonoBehaviour
{
    public enum EventID { BouncyBarrier, BallAnotherField, None }
    public static ManagerEvent Instance;
    private DispatchEvent[] _events;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        _events = new DispatchEvent[(int)EventID.None];
    }

    public void EvRegister(EventID id, DispatchEvent function)
    {
        if (id != EventID.None)
            _events[(int)id] += function;
    }

    public void EvUnregister(EventID id, DispatchEvent function)
    {
        if (id != EventID.None)
            _events[(int)id] -= function;
    }

    public void FireEvent(EventID id, GameObject dispacher)
    {
        if (id != EventID.None)
            _events[(int)id](dispacher);
    }
}

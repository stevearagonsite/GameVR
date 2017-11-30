using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerHandControllerVR : MonoBehaviour
{
    IPickable _PickedUp;
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_TrackedController trackedContrl;

    private void OnEnable()
    {
        trackedObj = gameObject.GetComponent<SteamVR_TrackedObject>();
        trackedContrl = gameObject.GetComponent<SteamVR_TrackedController>();
        trackedContrl.TriggerClicked += HandleTriggerClicked;
        trackedContrl.TriggerUnclicked += HandleTriggerReleased;
    }

    private void HandleTriggerReleased(object sender, ClickedEventArgs e)
    {
        Debug.Log("Solto");
        if (_PickedUp == null) return;

        _PickedUp.Release();
        _PickedUp = null;
    }

    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        Debug.Log("hizo click");
        var originalSetting = Physics.queriesHitTriggers;
        Physics.queriesHitTriggers = true;
        var near = Physics.OverlapSphere(gameObject.transform.position, 5f).Where(x => x.gameObject.tag == "Pickable");
        if (near.Any())
        {
            foreach (var elem in near)
            {
                var p = elem.gameObject.GetComponent<IPickable>();
                p.PickUp(gameObject.transform);
                _PickedUp = p;
            }
        }

        Physics.queriesHitTriggers = originalSetting;
    }

    private SteamVR_Controller.Device controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }
}

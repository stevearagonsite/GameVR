using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    float originalD;
    float y;

    IPickable _PickedUp;

    private void Awake()
    {
        originalD = (gameObject.transform.position - Camera.main.gameObject.transform.position).magnitude;
        y = gameObject.transform.position.y;
    }

    void Update ()
    {
        var r = Camera.main.ScreenPointToRay(Input.mousePosition);
        gameObject.transform.position = Camera.main.gameObject.transform.position + r.direction * originalD;

        if(_PickedUp == null)
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, y, gameObject.transform.position.z);

        if (Input.GetMouseButtonDown(0))
        {
            var originalSetting = Physics.queriesHitTriggers;
            Physics.queriesHitTriggers = true;
            var near = Physics.OverlapSphere(gameObject.transform.position, 5f).Where(x => x.gameObject.tag == "Pickable");
            if (near.Any())
            {
                Debug.Log("agarre");
                foreach (var elem in near)
                {
                    var p = elem.gameObject.GetComponent<IPickable>();
                    p.PickUp(gameObject.transform);
                    _PickedUp = p;
                }
            }

            Physics.queriesHitTriggers = originalSetting;
        }else if (Input.GetMouseButtonUp(0) && _PickedUp != null)
        {
            _PickedUp.Release();
            _PickedUp = null;
        }
	}
}

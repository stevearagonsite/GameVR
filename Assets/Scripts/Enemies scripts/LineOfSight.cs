using UnityEngine;
using System.Collections;

public class LineOfSight : MonoBehaviour {

    public GameObject target;
    public float viewAngle;
    public float viewDistance;
    
	const int wallLayer = 9;
	bool targetInSight;
	int layerMask;

	public Transform Target { get { return target.transform; } }
	public bool TargetInSight { get { return targetInSight; } }

	public void Configure(float viewAngle, float viewDistance) {
		this.viewAngle = viewAngle;
		this.viewDistance = viewDistance;
	}

	void Awake() {
		layerMask = (1 << wallLayer);
	}

	void Update () {
		// Primero calculamos que cumpla con los requisitos de distancia y ángulo.
		// Es decir, que este dentro del campo de visión sin contar obstáculos.

		// Siempre la dirección desde un punto a otro es: Posición Final - Posición Inicial
		var dirToTarget = target.transform.position - transform.position; 
        
		// Vector3.Angle nos da el ángulo entre dos direcciones
		var angleToTarget = Vector3.Angle(transform.forward, dirToTarget);

		// Vector3.Distance nos da la distancia entre dos posiciones
		var sqrDistanceToTarget = (transform.position - target.transform.position).sqrMagnitude;

		RaycastHit rch;

		targetInSight = 
			// Verifica el angulo de vision
			angleToTarget <= viewAngle &&
			// Verifica la distancia de vision
			sqrDistanceToTarget <= viewDistance * viewDistance &&
			// Verifica si hay obstaculos en el medio
			!Physics.Raycast(
				transform.position,
				dirToTarget,
				out rch,
				Mathf.Sqrt(sqrDistanceToTarget),
				layerMask
			);
	}

    void OnDrawGizmos() {
		// Dibujamos una línea desde el NPC hasta el enemigo.
		// Va a ser de color verde si lo esta viendo, roja sino.

		Gizmos.color = targetInSight ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, target.transform.position);

        //Dibujamos los límites del campo de visión.
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * viewDistance));

        Vector3 rightLimit = Quaternion.AngleAxis(viewAngle, transform.up) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + (rightLimit * viewDistance));

        Vector3 leftLimit = Quaternion.AngleAxis(-viewAngle, transform.up) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + (leftLimit * viewDistance));
    }
}

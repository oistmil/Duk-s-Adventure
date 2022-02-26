using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

	public Transform target; // Player transform

	private Vector3 _cameraOffset;

	[Range(0.01f, 1.0f)]
	public float SmoothFactor = 0.5f;


	[Range(-180, 180)]
	public int rotX = 18;
	[Range(-180, 180)]
	public int roty = 15;
	[Range(-180, 180)]
	public int rotz = 0;

	public bool LookAtTarget = false;

	void Start () {
		_cameraOffset = transform.position - target.position;
	}
	
	void LateUpdate()
	{
		Vector3 newPos = target.position + _cameraOffset + new Vector3(rotX, roty, rotz);

		transform.position = Vector3.Slerp (transform.position, newPos, SmoothFactor);

		if(LookAtTarget)
			transform.LookAt (target);
	}

	void Update()
	{
	}

}

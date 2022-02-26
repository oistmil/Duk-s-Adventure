using UnityEngine;
using System.Collections;

public class Lake : MonoBehaviour {
	
	void OnControllerColliderHit(ControllerColliderHit collision)
	{
		if (collision.gameObject.name == "Water")
		{
			gameObject.transform.position = new Vector3 (-11.0f, 0.8f, 180.0f);
			gameObject.GetComponent<PlayerState>().ResponPlayer();
		}
	}

}

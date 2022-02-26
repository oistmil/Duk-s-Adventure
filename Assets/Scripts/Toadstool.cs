using UnityEngine;
using System.Collections;

public class Toadstool : MonoBehaviour { // 수업시간 10. 의 Bullets.cs 응용

	Renderer renderer;
	public GameObject particle;

	void Start()
	{
		renderer = this.GetComponent<Renderer>();
		GameObject.FindGameObjectWithTag("GameManager").SendMessage("ToadstoolCollide");
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (new Vector3 (0, 0, 50.0f * Time.deltaTime));
	}

	void OnCollisionEnter(Collision collision)
	{
		Vector3 contactPoint = collision.contacts[0].point;
		Instantiate (particle, contactPoint, Quaternion.identity);
		Destroy (gameObject);
	}
	
}

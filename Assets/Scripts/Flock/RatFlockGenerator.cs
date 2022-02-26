using UnityEngine;
using System.Collections;

public class RatFlockGenerator : MonoBehaviour {

	public int ratFlockCount  = 0;

	public GameObject RatFlock;
	// Use this for initialization
	void Start () {
		for(int i=0;i<3;i++)
		{
			RatFlockGenerate();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (ratFlockCount < 3)
			RatFlockGenerate();
	}

	void RatFlockGenerate(){
		ratFlockCount++;
		GameObject ratflock = Instantiate (RatFlock, new Vector3(100.0f, 3.0f, -100.0f), Quaternion.identity) as GameObject;
		ratflock.transform.parent = transform;
	}
}

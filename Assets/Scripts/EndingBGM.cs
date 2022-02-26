using UnityEngine;
using System.Collections;

public class EndingBGM : MonoBehaviour {

	void Awake()
	{
		GameObject.FindGameObjectWithTag("BGM").SendMessage("DestoryBGM");
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

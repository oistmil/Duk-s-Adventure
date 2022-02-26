using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {
	void Awake()
	{
		DontDestroyOnLoad (gameObject);
	}

	public void DestoryBGM()
	{
		Destroy (gameObject);
	}
}

using UnityEngine;
using System.Collections;

public class WolfGenerator : MonoBehaviour {

	public GameObject Wolf;
	public int wolfCount = 0;
	public GUIStyle style;
	// Use this for initialization
	void Start () {
		WolfGenerate();
	}
	
	// Update is called once per frame
	void Update () {
		if (wolfCount <= 0) 
		{
			Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
			if (-169 <playerPos.x && playerPos.x < -150 && -213 < playerPos.z && playerPos.z < -203)
				Application.LoadLevel (Application.loadedLevel+1);
		}
	}

	void WolfGenerate()
	{
		for (int i = wolfCount; i < 10; i++)
		{
			GameObject wolf = Instantiate (Wolf, new Vector3(-130.0f, 3.0f, -130.0f), Quaternion.identity) as GameObject;
			wolf.transform.parent = transform;
		}
		wolfCount = 10;
	}

	void OnGUI()
	{
		if (wolfCount == 0)
			GUI.Label (new Rect (0, 0, Screen.width, Screen.height), "늑대를 모두 처치했습니다! 이제 북동쪽 마을로 갈 수 있습니다!", style);
		else
			GUI.Label (new Rect (0, 0, Screen.width, Screen.height), "늑대를 모두 처치해주세요. 남은 늑대 수:"+wolfCount.ToString(), style);
	}
}

using UnityEngine;
using System.Collections;

public class HelperNPC : MonoBehaviour {

	Vector3 playerPos;
	bool playerInRange = false;
	public GUISkin skin;
	string[][] Conversation = new string[6][];
	int conversationCount = 0;
	// Use this for initialization
	void Start () {
		Conversation [0] = new string[]{};
		Conversation [1] = new string[]{"성이가 사람에게 잡혀갔으니, 성이는 사람들이 사는 동북쪽의 마을에 있을거야!", "우선 이 징검다리 건너보자. 나는 먼저 가서 기다릴게"};
		Conversation [2] = new string[]{""};
		Conversation [3] = new string[]{"마을로 가려면 늑대들의 영역을 통과해야 하나봐..", "우선, 늑대들을 기절시킬 수 있는 독버섯을 줄게. 이 독버섯은 왼쪽의 숲에서 또 얻을 수 있어", 
			"그리고, 왼쪽 숲에 있는 쥐들을 먹으면 체력 회복이 되니 참고해!"};
		Conversation [4] = new string[]{""};
		Conversation [5] = new string[]{"저기 울타리 안에 성이와 다른 여우들이 갇혀있는 거 같아!", "울타리를 열려면.. 아마 열쇠가 필요할 거야!", "사람의 눈을 피해서 열쇠를 찾고 울타리를 열어서 여우들을 구출해줘!"};
	}
	
	// Update is called once per frame
	void Update () {
		IsPlayerInRange ();
		
		if (playerInRange)
		{
			StartCoroutine(LoadSceneTo3From2());
		}
	}

	void IsPlayerInRange()
	{
		playerPos = GameObject.FindGameObjectWithTag ("Player").transform.position;
		if ( (-4.0f < playerPos.x)&&(playerPos.x < 15.0f) && (1.5f < playerPos.z)&&(playerPos.z < 8.0f))
			playerInRange = true;
		else 
			playerInRange = false;
	}

	IEnumerator LoadSceneTo3From2 ()
	{
		animation.Play ("Armature|Jump");
		yield return new WaitForSeconds (1.5f);
		Application.LoadLevel(3);
	}

	void OnGUI()
	{
		if (Application.loadedLevel == 2)
			return;

		GUI.skin = skin;
		Rect rect1 = new Rect (Screen.width/4, Screen.height/8, Screen.width*3/4-Screen.width/20, Screen.height*2/8);
		Rect rect2 = new Rect (Screen.width*4/5, Screen.height*2/8, Screen.width/10, Screen.height/10);

		GUI.Label (rect1, Conversation[Application.loadedLevel][conversationCount]);
		if (GUI.Button (rect2, "확인"))
		{
			if (conversationCount < Conversation[Application.loadedLevel].Length-1)
				conversationCount++;
			else if (conversationCount==Conversation[Application.loadedLevel].Length-1)
				Application.LoadLevel(Application.loadedLevel+1);
		}
	}
	
}

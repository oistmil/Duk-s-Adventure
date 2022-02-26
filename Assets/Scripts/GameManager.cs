using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GUISkin skin;
	public GUIStyle resultStyle;
	public GameObject fox;
	bool gameclear = false;
	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (gameObject);
	}

	void Start()
	{
		StartCoroutine (EndSceneWithClear ());
	}

	IEnumerator EndSceneWithClear()
	{
		while(Application.loadedLevelName != "EndScene")
		{
			yield return null;
		}

		if (gameclear)
			Instantiate (fox, new Vector3 (2.0f, 0.0f,-1.8f), Quaternion.LookRotation(new Vector3( -90.0f, 0.0f, 0.0f)));
	}

	public void GameClear () 
	{
		gameclear = true;
	}
	
	void OnGUI()
	{
		if (Application.loadedLevelName == "EndScene")
		{
			GUI.skin = skin;
			GUI.Label(new Rect(Screen.width/10, Screen.height/3, Screen.width*8/10, Screen.height*1/3),
			          gameclear? "덕이는 여우들을 구출하는데 성공하였습니다. 이후 덕이는 성이와 함께 집으로 돌아가 행복하게 살았습니다~":"덕이는 여우들을 구출하는데 실패했습니다...");
			GUI.Label(new Rect(0, Screen.height/4, Screen.width, Screen.height/4), gameclear? "CLEAR!": "FAIL..", resultStyle);
			if(GUI.Button (new Rect(Screen.width*7/10, Screen.height*2/3, Screen.width*2/10, Screen.height*1/6), "다시하기"))
			{
				Application.LoadLevel(0);
				Destroy(gameObject);
			}
		}
	}
}

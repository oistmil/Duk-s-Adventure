using UnityEngine;
using System.Collections;

public class TitleScene : MonoBehaviour {

	public GUIStyle title;
	public GUIStyle story;
	public GUIStyle button;
	private bool description = false;
	private string descriptionContent = "여우 덕이와 성이는 오랜 친구사이입니다.\n " +
		"어느날, 먹이를 찾으러 떠났던 친구 성이가 집으로 돌아오지 않자 덕이는 성이를 찾기 위해 며칠을 헤맸습니다." +
		"\n그러던 또 어느날, 낯선 강아지에게 성이가 사람에게 잡혀갔다는 이야기를 듣게 됩니다. \n덕이는 성이를 구하기 위해 모험을 시작합니다.\n\n\n\n" +
		"이동:방향키/WASD | 점프:Space";

	// Use this for initialization
	void OnGUI()
	{

		if (description) 
		{
			if (GUI.Button (new Rect(Screen.width*9/10, 0, Screen.width/10, Screen.height/8), "X", button))
				description = false;
			GUI.Label (new Rect(Screen.width/3, Screen.height/8, Screen.width*2/3, Screen.height*7/8), descriptionContent, story);
		}
		else
		{
			GUI.Label (new Rect(Screen.width*1/3, Screen.height/4, Screen.width*2/3, Screen.height/4), "덕이의 모험", title);
				
			if(GUI.Button (new Rect(Screen.width*2/3+10, Screen.height/2, Screen.width/6, Screen.height/6), "게임 시작", button))
				Application.LoadLevel(Application.loadedLevel+1);

			if (GUI.Button (new Rect(Screen.width/2, Screen.height/2, Screen.width/6, Screen.height/6), "게임 설명", button))
				description = true;
		}
	}
}

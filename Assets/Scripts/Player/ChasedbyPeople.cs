using UnityEngine;
using System.Collections;

public class ChasedbyPeople : MonoBehaviour {

	int opportunity = 3;
	public GUIStyle opportunityStyle;
	public GUIStyle commandStyle;
	bool obtainKey = false;
	public bool PersonCanAttack = true;
	private int collideWith = 2; // 0: 테이블(열쇠), 1: 펜스(울타리)
	string[] command;
	public GUIStyle keyStyle;
	// Use this for initialization
	void Start () {
		command = new string[]{"[Z] 열쇠 얻기", "[Z] 울타리 열기", ""};
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Attacked()
	{
		StartCoroutine (Invincible());
		obtainKey = false;
		if (opportunity <= 0)
			Application.LoadLevel (Application.loadedLevel+1); 
		GameObject.FindGameObjectWithTag("GameManager").SendMessage("PlayerRespon");
		transform.position = new Vector3 (200.0f, 15.0f, -170.0f);
	}

	IEnumerator Invincible()
	{
		if(PersonCanAttack)
		{
			PersonCanAttack = false;
			opportunity--;
			yield return new WaitForSeconds(5.0f);
			PersonCanAttack = true;
		}
	}

	void OnGUI()
	{
		GUI.Label (new Rect (0, 0, Screen.width, Screen.height), "남은 기회:" + opportunity.ToString (), opportunityStyle);
		GUI.Label (new Rect (0, 0, Screen.width, Screen.height), obtainKey ? "울타리로 가서 열쇠로 여우들을 구하세요!" : "테이블 위에 있는 열쇠를 획득하세요!", commandStyle);
		GUI.Label(new Rect (0, 0, Screen.width, Screen.height),command[collideWith],keyStyle); 
	}

	void OnControllerColliderHit(ControllerColliderHit collision)
	{
		if (collision.gameObject.tag == "Fence" && obtainKey) 
		{
			collideWith = 1;
			if (Input.GetKeyDown(KeyCode.Z))
			{
				GameObject.FindGameObjectWithTag("GameManager").SendMessage("PlayerRespon");
				GameObject.FindGameObjectWithTag("GameManager").SendMessage("GameClear");
				Application.LoadLevel (Application.loadedLevel+1);
			}
		} 
		else if (collision.gameObject.tag == "Table" && !obtainKey)
		{
			collideWith = 0;
			if (Input.GetKeyDown(KeyCode.Z))
			{
				obtainKey = true;
				GameObject.FindGameObjectWithTag("GameManager").SendMessage("GetSomething");
			}
		}
	}

	IEnumerator UpdateCommand()
	{
		while(true)
		{
			if(collideWith != 2)
			{
				yield return new WaitForSeconds(1.0f);
				collideWith = 2;
			}
			yield return null;
		}
	}
}

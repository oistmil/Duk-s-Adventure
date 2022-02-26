using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {

	public Transform shootpos;
	public GameObject toadstool;
	int numOfToadstool;

	private int collideWith = 0; // 0: 공격(버섯발사 안내), 1: 쥐(먹기(체력회복) 안내), 2: 독버섯 채집 안내.
	public GUIStyle style;
	string[] command;
	// Use this for initialization
	void Start () {
		command = new string[]{"[X] 버섯 발사(늑대 공격)", "[Z] 쥐 잡기", "[Z] 버섯 채집"};
		StartCoroutine (UpdateCommand());
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown(KeyCode.Z))
		{
			animation.Play("Attack");
		}
		if (Input.GetKeyDown(KeyCode.X))
		{
			animation.Play("Attack");
			Shoot();
		}
	}
	

	void Shoot() // 독버섯 수를 고려하여 늑대 공격
	{
		if (gameObject.GetComponent<PlayerState> ().toadstoolNum > 0)
		{
			Vector3 direction = shootpos.position - transform.position;
			direction.y = 0.2f;

			Instantiate (toadstool, shootpos.position, Quaternion.LookRotation(direction));
			gameObject.GetComponent<PlayerState> ().toadstoolNum -= 1;
		}
	}

	void OnControllerColliderHit(ControllerColliderHit collision)
	{
		// 쥐를 잡아먹기 
		if (collision.gameObject.tag == "Rat"){
			collideWith = 1;
			if (Input.GetKeyDown(KeyCode.Z))
			{
				gameObject.GetComponent<PlayerState> ().health += Random.Range (10.0f, 30.0f);
				collision.gameObject.SendMessage("BittenByPlayer");
				GameObject.FindGameObjectWithTag("GameManager").SendMessage("GetSomething");
			}
		}
		// 독버섯 획득하기
		else if (collision.gameObject.tag == "Toadstools")
			collideWith = 2;
	}

	void OnGUI()
	{
		GUI.Label(new Rect (0, 0, Screen.width, Screen.height),command[collideWith],style); 

	}

	IEnumerator UpdateCommand()
	{
		while(true)
		{
			if(collideWith != 0)
			{
				yield return new WaitForSeconds(2.0f);
				collideWith = 0;
			}
			yield return null;
		}
	}

}

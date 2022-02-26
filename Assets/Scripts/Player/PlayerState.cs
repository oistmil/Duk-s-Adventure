using UnityEngine;
using System.Collections;

public class PlayerState : MonoBehaviour {

	// under 2 vari~ set as ''private''
	public float PlayerHealth = 100.0f;
	public int toadstoolNum = 0;

	public float health{
		get {return PlayerHealth;}
		set{
			PlayerHealth=value;
			if (PlayerHealth > 100.0f)
				PlayerHealth = 100.0f;
		}
	}

	public GUISkin skin;

	public int ToadStool{
		set { toadstoolNum = value; }
		get { return toadstoolNum; }
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerHealth < 0.0f)
		{
			StartCoroutine(PlayerDead());
			PlayerHealth = 100.0f;
		}
	}

	void OnGUI ()
	{
		GUI.skin = skin;
		Rect rect1 = new Rect (0, 0, Screen.width, Screen.height / 5);
		GUI.Label (rect1, "체력: "+Mathf.Round(PlayerHealth).ToString() + "\n독버섯 개수: "+toadstoolNum.ToString());
	}

	void OnControllerColliderHit(ControllerColliderHit collision)
	{
		if (collision.gameObject.name == "Toadstools" && Input.GetKeyDown(KeyCode.Z))
		{
			int randomNum = Random.Range (5, 10);
			toadstoolNum += randomNum;
			collision.gameObject.SendMessage("ObtainedToadstool");
		}
	}

	IEnumerator PlayerDead()
	{
		gameObject.GetComponent<PlayerController>().PlayerDead = true;
		animation.CrossFade("Death", 0.3f);
		GameObject.FindGameObjectWithTag("GameManager").SendMessage("AnimalDead");
		GameObject.FindObjectOfType<WolfGenerator> ().SendMessage ("WolfGenerate");
		yield return new WaitForSeconds(1.0f);
		gameObject.transform.position = new Vector3 (-5.0f, 3.0f, -11.0f);
		ResponPlayer ();
		PlayerHealth = 100.0f;
		gameObject.GetComponent<PlayerController>().PlayerDead = false;
	}

	public void ResponPlayer()
	{
		GameObject.FindGameObjectWithTag("GameManager").SendMessage("PlayerRespon");
		Renderer renderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
		
		foreach (Material material in renderer.materials)
		{
			Color original = material.color;
			StartCoroutine(FadeIn (material, original));
		}
	}
	
	public IEnumerator FadeIn(Material material, Color original)
	{
		material.color = new Color (material.color.r, material.color.b, material.color.g, 0.0f);
		
		while(material.color.a < 1.0f)
		{
			material.color = new Color(material.color.r, material.color.b, material.color.g, material.color.a+Time.deltaTime);
			yield return new WaitForSeconds(0.001f);
		}
		
		material.color = original;
		yield return null;
		
	}

}

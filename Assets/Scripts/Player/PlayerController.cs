using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float walkSpeed = 15.0f;
	public float jumpSpeed = 8.0f;
	public float gravity = 20.0f;

	private Vector3 velocity;
	private float defaultSpeed = 15.0f;
	private float maxSpeed = 25.0f;

	private bool isDead = false;
	public bool PlayerDead {
		get { return isDead;}
		set { isDead = value; }
	}

	public GUIStyle style;
	private bool canControlSpeed = true;
	// Use this for initialization
	void Start () {
		animation["Gallop"].speed = 2.0f;
		StartCoroutine(ControlSpeed());
	}
	
	// Update is called once per frame
	void Update () {
		if (isDead)
			return;
		CharacterController controller = gameObject.GetComponent<CharacterController> ();
		if (controller.isGrounded){
			velocity = new Vector3(-Input.GetAxis("Horizontal"), 0.0f, -Input.GetAxis("Vertical"));
			velocity *= walkSpeed;
			if (Input.GetButtonDown("Jump"))
			{
				GameObject.FindGameObjectWithTag("GameManager").SendMessage("PlayerJump");
				velocity.y = jumpSpeed;
				animation.Play("Jump_ToIdle");
			}
			else if(velocity.magnitude > 0.5f) {
				animation.CrossFade("Gallop", 0.1f);
				transform.LookAt(transform.position + velocity);
			}
			else {
				animation.CrossFade("Idle", 0.5f);
			}
			if (Input.GetKeyDown (KeyCode.B))
			{
				walkSpeed = maxSpeed;
				walkSpeed = defaultSpeed;
			}
			
		}
		velocity.y -= gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
	}

	public IEnumerator ControlSpeed()
	{
		while(true)
		{
			if (walkSpeed == maxSpeed)
			{
				walkSpeed = defaultSpeed;
				yield return new WaitForSeconds(5.0f);
				canControlSpeed = true;
			}
			else if (Input.GetKeyDown(KeyCode.LeftControl) && walkSpeed == defaultSpeed)
			{
				walkSpeed = maxSpeed;
				canControlSpeed = false;
				yield return new WaitForSeconds(5.0f);
			}
			else
			{
				yield return null;
			}
		}
	}

	void OnGUI ()
	{
		Rect rect1 = new Rect (0, 0, Screen.width, Screen.height);
		if(canControlSpeed)
			GUI.Label (rect1, "가속: [Ctrl]", style);
	}
}	

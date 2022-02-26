using UnityEngine;
using System.Collections;

public class WolfFSM : FSM {
	
	public enum FSMState
	{
		None,
		Patrol,
		Chase,
		Attack,
		Dead,
	}

	public FSMState currentState;

	public AudioSource audioSource;

	private float currentSpeed;
	private float currentRotSpeed;

	private bool canAttack = true;
	private float attackRate; // shootRate
	private float elapsedTime;
	private GameObject player;
	private int layerMask = 1 << 9;
	Renderer renderer;

	protected override void Initialize()
	{
		currentState = FSMState.Patrol;
		currentSpeed = 15.0f;
		currentRotSpeed = 2.0f;
		bDead = false;
		elapsedTime = 0.0f;
		attackRate = 3.0f;
		maxHP = 100.0f;

		// 순찰 범위에 대해 설정하기..

		player = GameObject.FindGameObjectWithTag ("Player");
		playerTransform = player.transform;

		if(!playerTransform)
			print ("플레이어 없음");
		renderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();

		CalculateNextDestination ();

	}

	protected override void FSMUpdate() // Update()에서 실행 중
	{
		// 플레이어가 쫓을 수 있는 영역을 벗어난 경우.
		if (transform.position.x > -40.0f || transform.position.z > -40.0f)
		{
			currentState = FSMState.Patrol;
		}

		switch(currentState)
		{
			case FSMState.Patrol: UpdatePatrolState(); break;
			case FSMState.Chase: UpdateChaseState(); break;
			case FSMState.Attack: UpdateAttackState(); break;
			case FSMState.Dead: UpdateDeadState(); break;
		}

		elapsedTime += Time.deltaTime;

	}

	protected void UpdatePatrolState()
	{
		RaycastHit hit;
		if (Vector3.Distance (transform.position, playerTransform.position) <= 20.0f)
		{
			currentState = FSMState.Chase;
			GameObject.FindGameObjectWithTag("GameManager").SendMessage("WolfChasing", audioSource);
		}

		animation.CrossFade ("Walk");

		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (destPos - transform.position), Time.deltaTime*currentRotSpeed);
		transform.Translate (Vector3.forward * Time.deltaTime * 5.0f);

		if (Vector3.Distance(destPos, transform.position) < 10.0f)
		{
			CalculateNextDestination();
		}

	}
	
	protected void UpdateChaseState()
	{
		destPos = playerTransform.position;

		float dist = Vector3.Distance (transform.position, playerTransform.position);

		if (dist <= 5.0f) 
		{
			currentState = FSMState.Attack;
		}
		else if (dist >= 20.0f)
		{
			currentState = FSMState.Patrol;
			GameObject.FindGameObjectWithTag("GameManager").SendMessage("WolfPatrolling", audioSource);
		}

		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (destPos - transform.position), Time.deltaTime*currentRotSpeed);
		transform.Translate (Vector3.forward * Time.deltaTime * currentSpeed);

		animation.CrossFade ("Gallop", 0.5f);
	}
	
	protected void UpdateAttackState()
	{
		if (maxHP <= 0.0f)
		{
			currentState = FSMState.Dead;
		}

		destPos = playerTransform.position;
		
		float dist = Vector3.Distance (transform.position, playerTransform.position);

		if (dist >= 5.0f && dist <= 15.0f)
		{
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (destPos - transform.position), Time.deltaTime*currentRotSpeed);
			transform.Translate (Vector3.forward * Time.deltaTime * currentSpeed);

			animation.CrossFade("Gallop", 0.2f);

			currentState = FSMState.Attack;
		}
		else if (dist >= 15.0f)
		{
			currentState = FSMState.Patrol;
			GameObject.FindGameObjectWithTag("GameManager").SendMessage("WolfPatrolling", audioSource);
		}
		else {
			animation.CrossFade ("Attack", 0.5f);
			AttackPlayer ();
		}
	}
	
	protected void UpdateDeadState()
	{
		if(!bDead)
		{
			bDead = true;
			transform.parent.GetComponent<WolfGenerator>().wolfCount--;
			animation.CrossFade("Death");
			GameObject.FindGameObjectWithTag("GameManager").SendMessage("AnimalDead");
			StartCoroutine (Disappear());
			Destroy(gameObject, 3.0f);
		}
	}

	void CalculateNextDestination () // 랜덤으로 돌아다닐 지점 정하기
	{
		destPos = new Vector3 (Random.Range (-200.0f, -40.0f), 0.0f, Random.Range (-200.0f, -40.0f));
	}

	void OnCollisionEnter(Collision collision) // 플레이어에게 공격 당함 
	{
		if (collision.gameObject.tag == "Toadstool")
		{
			maxHP -= Random.Range (45.0f, 70.0f);
		}
	}

	void AttackPlayer() // 플레이어 공격 
	{
		if (Random.Range (1,100) < 5)
		{
			player.GetComponent<PlayerState>().health -= Random.Range (1.0f, 3.0f);
		}
	}

	public IEnumerator Disappear() // 늑대가 기절했을 경우 사라지기 위한 코드.
	{
		yield return new WaitForSeconds(0.01f);
		foreach (Material material in renderer.materials)
		{
			StartCoroutine(FadeOut (material));
		}
	}

	public IEnumerator FadeOut(Material material) // 재질의 투명도를 조절하기 위한 코드
	{
		material.color = new Color (material.color.r, material.color.b, material.color.g, 1.0f);
		
		while(material.color.a > 0.0f)
		{
			material.color = new Color(material.color.r, material.color.b, material.color.g, material.color.a-Time.deltaTime);
			yield return null;
		}
		
		Destroy (gameObject);
	}

}

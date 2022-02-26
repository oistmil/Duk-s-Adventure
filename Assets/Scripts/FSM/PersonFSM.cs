using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class PersonFSM : FSM {

	public enum FSMState
	{
		None,
		Patrol,
		Chase,
		Attack,
	}
	
	public FSMState currentState;
	public Vector3[] point;
	private float currentSpeed;
	private float currentRotSpeed;
	private float elapsedTime;
	private GameObject player;

	private float angleRange = 60.0f;
	private float dotValue = 0.9f;
	private Vector3 direactionToPlayer;
	// 최소거리와 최대 거리의 사이에는 사람의 레이캐스트에 딱 부딪혔는지만 검사하여 쫓을지 결정함.
	private float maxDistance = 60.0f; // 사람이 경계하는 최대 거리.
	private float minDistance = 40.0f; // 사람의 시선에 걸리는 최소 거리.
	private bool isCollision = false;

	protected override void Initialize()
	{
		currentState = FSMState.Patrol;
		currentSpeed = 17.0f;
		currentRotSpeed = 2.0f;
		bDead = false;
		elapsedTime = 0.0f;
		
		// 순찰 범위에 대해 설정하기..
		
		playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
		
		if(!playerTransform)
			print ("플레이어 없음");
		
		CalculateNextDestination ();
		
	}

	void CalculateNextDestination ()
	{
		destPos = new Vector3(Random.Range (170.0f, 252.0f), 13.0f, Random.Range (-320.0f, -274.0f));
	}

	protected override void FSMUpdate() // Update()에서 실행 중
	{
		switch(currentState)
		{
		case FSMState.Patrol: UpdatePatrolState(); break;
		case FSMState.Chase: UpdateChaseState(); break;
		case FSMState.Attack: UpdateAttackState(); break;
		}
		
		elapsedTime += Time.deltaTime;
		
	}
	
	protected void UpdatePatrolState()
	{
		PlayerInRange ();
		animation.CrossFade ("HumanArmature|Man_Walk");

		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (destPos - transform.position), Time.deltaTime*currentRotSpeed);
		transform.Translate (Vector3.forward * Time.deltaTime * 5.0f);
		if (Vector3.Distance(destPos, transform.position) <= 5.0f)
		{
			CalculateNextDestination();
		}
		
	}

	void PlayerInRange() // 사람 NPC가 순찰하는 범위.
	{
		dotValue = Mathf.Cos (Mathf.Deg2Rad * (angleRange / 2));
		direactionToPlayer = playerTransform.position - transform.position;
		if(direactionToPlayer.magnitude < minDistance)
		{
			if (Vector3.Dot (direactionToPlayer.normalized, transform.forward) > dotValue)
				currentState = FSMState.Chase;
		}
		else if (direactionToPlayer.magnitude < maxDistance)
		{
			if (Physics.Raycast(transform.position+new Vector3(0.0f, 1.5f, 0.0f), transform.forward, maxDistance, (1<<9)))
				currentState = FSMState.Chase;
		}

	}

	private void OnDrawGizmos() // 사람NPC 시야 범위 출력 
	{
		#if UNITY_EDITOR
		Handles.color = currentState == FSMState.Chase ? new Color(1.0f, 0.0f, 0.0f, 0.15f) : new Color(0.0f, 0.0f, 1.0f, 0.15f);
		Handles.DrawSolidArc (transform.position, Vector3.up, transform.forward, angleRange / 2, minDistance);
		Handles.DrawSolidArc (transform.position, Vector3.up, transform.forward, -angleRange / 2, minDistance);
		#endif
		Debug.DrawLine (transform.position+new Vector3(0.0f, 1.5f, 0.0f), transform.position + (transform.forward*maxDistance), Color.green);
	}
	
	protected void UpdateChaseState()
	{
		destPos = playerTransform.position;
		
		float dist = Vector3.Distance (transform.position, playerTransform.position);
		
		if (dist <= 5.0f) 
		{
			currentState = FSMState.Attack;
		}
		else if (dist >= maxDistance)
		{
			currentState = FSMState.Patrol;
		}
		
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (destPos - transform.position), Time.deltaTime*currentRotSpeed);
		transform.Translate (Vector3.forward * Time.deltaTime * currentSpeed);
		
		animation.CrossFade ("HumanArmature|Man_Run", 0.5f);
	}
	
	protected void UpdateAttackState()
	{
		
		destPos = playerTransform.position;
		
		float dist = Vector3.Distance (transform.position, playerTransform.position);
		
		if (dist >= 10.0f && dist <= 15.0f)
		{
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (destPos - transform.position), Time.deltaTime*currentRotSpeed);
			transform.Translate (Vector3.forward * Time.deltaTime * currentSpeed);
			
			animation.CrossFade("HumanArmature|Man_Run", 0.2f);
			
			currentState = FSMState.Chase;
		}
		else if (dist >= maxDistance)
		{
			currentState = FSMState.Patrol;
		}
		else {
			animation.CrossFade ("HumanArmature|Man_SwordSlash", 0.5f);
			AttackPlayer();
		}
	}

	void AttackPlayer()
	{
		if (Random.Range (1,100) < 5)
		{
			GameObject.FindGameObjectWithTag("Player").SendMessage("Attacked");
			currentState = FSMState.Patrol;
		}
	}

}

using UnityEngine;
using System.Collections;

public class RatFSM : FSM {

	
	public enum FSMState
	{
		None,
		Patrol,
		Chased, // Rat are chased
		Dead,
	}
	
	private FSMState currentState;

	public FSMState CurrentState {
		get {
			return currentState;
		}
	}

	private float currentSpeed;
	private float currentRotSpeed;
	
	private bool canAttack = true;
	private float attackRate; // shootRate
	private float elapsedTime;
	private Transform flockController;

	public NavMeshAgent agent;

	void Awake()
	{
		flockController = transform.parent;
	}
	
	protected override void Initialize()
	{
		currentState = FSMState.Patrol;
		currentSpeed = 15.0f;
		currentRotSpeed = 2.0f;
		bDead = false;
		elapsedTime = 0.0f;
		attackRate = 3.0f;
		maxHP = 100.0f;

		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		playerTransform = player.transform;
		
		if(!playerTransform)
			print ("플레이어 없음");
		
	}

	void BittenByPlayer()
	{
		currentState = FSMState.Dead;
	}
	
	protected override void FSMUpdate()
	{
		switch(currentState)
		{
		case FSMState.Patrol: UpdatePatrolState(); break;
		case FSMState.Chased: UpdateChasedState(); break;
		case FSMState.Dead: UpdateDeadState(); break;
		}
		
		elapsedTime += Time.deltaTime;
		
		if (maxHP <= 0.0f)
			currentState = FSMState.Dead;
	}
	
	protected void UpdatePatrolState()
	{
		if (Vector3.Distance (transform.position, playerTransform.position) <= 20.0f)
		{
			currentState = FSMState.Chased;
			print ("Patrol -> Chased");
		}
	}
	
	protected void UpdateChasedState()
	{
		destPos = playerTransform.position;

		float dist = Vector3.Distance (transform.position, playerTransform.position);
		
		if (dist >= 40.0f)
		{
			currentState = FSMState.Patrol;
			print ("Chased -> Patrol");
		}
	
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (-(destPos - transform.position)), Time.deltaTime*currentRotSpeed);
		transform.Translate (Vector3.forward * Time.deltaTime * currentSpeed);

		animation.CrossFade ("RatArmature|Rat_Run", 0.5f);

	}

	
	protected void UpdateDeadState() // 플레이어에게 먹힌 쥐, Flock에서 제거.
	{
		if(!bDead)
		{
			bDead = true;
			flockController.gameObject.SendMessage("LostChildRat");
			Destroy(gameObject);
		}
	}

	

}

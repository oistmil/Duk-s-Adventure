using UnityEngine;
using System.Collections;

public class FSM : MonoBehaviour {

	// 적(쥐, 늑대, 사람)에 적용
	
	protected Transform playerTransform;
	
	protected Vector3 destPos; // 다음 목적지.
	protected GameObject[] pointList; // FSM 종류에 따라 경로 관리에 사용.
	
	
	public float maxHP = 100.0f;
	public float currentHP = 100.0f;
	protected bool bDead;
	
	public float damage = 30.0f;
	
	protected float playerDist = 10.0f;
	protected float attackDist = 5f;
	
	protected virtual void Initialize(){} // 초기화
	protected virtual void FSMUpdate(){}
	protected virtual void FSMFixedUpdate(){}
	
	// Use this for initialization
	void Start () {
		Initialize ();
	}
	
	// Update is called once per frame
	void Update () {
		FSMUpdate ();
	}
	
	void FixedUpdate(){
		FSMFixedUpdate ();
	}
}

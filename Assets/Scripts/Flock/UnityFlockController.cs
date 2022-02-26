using UnityEngine;
using System.Collections;

public class UnityFlockController : MonoBehaviour
{
	public Vector3 bound;
	public float speed = 100.0f;
	public Vector3[] pointA = new Vector3[3];

	private Vector3 initialPosition;
	private Vector3 nextMovementPoint;
	private int index = 0;

	private int NumOfRat = 4;

	// Use this for initialization
	void Start()
	{
		for (int i=0;i<3;i++)
		{
			pointA[i] = new Vector3(Random.Range(24.0f, 200.0f) ,0.0f, Random.Range(0.0f, -200.0f));
		}

		initialPosition = transform.position;
		CalculateNextMovementPoint();
	}
	
	void CalculateNextMovementPoint()
	{
		nextMovementPoint = pointA[index];
		index = (index + 1) % pointA.Length;
	}
	
	// Update is called once per frame
	void Update() 
	{
		if(NumOfRat == 0) // flock 내에 rat이 없는 경우 이 오브젝트 삭제.
		{
			Destroy(gameObject);
			transform.parent.GetComponent<RatFlockGenerator>().ratFlockCount--; // RatFlock수를 유지시키기 위해.
		}

		transform.Translate(Vector3.forward * speed * Time.deltaTime);
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(nextMovementPoint - transform.position), 1.0f * Time.deltaTime);
		
		if (Vector3.Distance(nextMovementPoint, transform.position) <= 10.0f)
			CalculateNextMovementPoint();

	}

	public void setPoint(Vector3[] point)
	{
		for (int i=0;i<3;i++)
		{
			pointA[i] = point[i];
		}
	}

	void LostChildRat()
	{
		NumOfRat--;
	}
	
}
using UnityEngine;
using System.Collections;

public class ObtainToadstool : MonoBehaviour {

	Renderer[] renderers;
	// Use this for initialization
	void Start () {

		renderers = this.GetComponentsInChildren<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ObtainedToadstool()
	{
		GameObject.FindGameObjectWithTag("GameManager").SendMessage("GetToadstools");
		StartCoroutine(Disapper ());

	}

	public IEnumerator Disapper()
	{
		foreach (Renderer renderer in renderers){
			foreach (Material material in renderer.materials)
			{
				material.color = new Color (material.color.r, material.color.b, material.color.g, 0.0f);
			}
		}
		yield return new WaitForSeconds (5.0f);
		Appear ();

	}

	public void Appear()
	{
		foreach (Renderer renderer in renderers){
			foreach (Material material in renderer.materials)
			{
				StartCoroutine(FadeIn (material));
			}
		}
	}

	public IEnumerator FadeIn(Material material)
	{
		while(material.color.a < 1.0f)
		{
			material.color = new Color(material.color.r, material.color.b, material.color.g, material.color.a+Time.deltaTime);
			yield return null;
		}
	}
}

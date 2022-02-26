using UnityEngine;
using System.Collections;

public class SoundEffector : MonoBehaviour {
	
	public AudioClip dead;
	public AudioClip toadstoolCollide;
	public AudioClip getSomethig;
	public AudioClip getToadstools;
	public AudioClip respon;
	public AudioClip jump;


	public void WolfChasing(AudioSource audioSource)
	{
		if(audioSource.isPlaying == false)
		{
			audioSource.PlayDelayed (Random.Range (0.0f, 1.0f));
		}
	}

	public void WolfPatrolling(AudioSource audioSource)
	{
		audioSource.Stop ();
	}

	public void AnimalDead()
	{
		audio.PlayOneShot (dead);
	}

	public void PlayerRespon()
	{
		audio.PlayOneShot (respon);
	}

	public void GetSomething()
	{
		audio.PlayOneShot (getSomethig);
	}

	public void GetToadstools()
	{
		audio.PlayOneShot (getToadstools);
	}

	public void ToadstoolCollide()
	{
		audio.PlayOneShot (toadstoolCollide);
	}

	public void PlayerJump()
	{
		audio.PlayOneShot (jump);
	}
}

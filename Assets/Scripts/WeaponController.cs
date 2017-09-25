using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

	public GameObject shot;
	public Transform[] shotSpawns;
	public float fireRate;
	public float delay;
	private AudioSource audioSource;

	void Start () {
		audioSource = GetComponent<AudioSource> ();
		InvokeRepeating ("Fire", delay, fireRate);
	}

	void Fire () {
		foreach (Transform shotSpawn in shotSpawns) {
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
		}
		audioSource.Play ();
	}
}

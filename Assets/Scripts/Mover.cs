using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
	public float speed;
	private Rigidbody rb;
	private bool changedSpeed;

	void Start()
	{
		changedSpeed = false;
		rb = GetComponent<Rigidbody> ();
		rb.velocity = transform.forward * speed;
	}

	void FixedUpdate ()
	{
		if (this.tag == "Boss") {
			rb = GetComponent<Rigidbody> ();
			if ((rb.position.z <= 13.5) && (changedSpeed == false)) {
				rb.velocity = Vector3.zero;
				changedSpeed = true;
			}
		}
	}
}

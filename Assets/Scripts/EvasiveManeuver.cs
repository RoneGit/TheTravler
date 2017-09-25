using UnityEngine;
using System.Collections;

public class EvasiveManeuver : MonoBehaviour {
	public float dodge;
	public float smoothing;
	public float tilt;
	public Vector2 startWait;
	public Vector2 meneuverTime;
	public Vector2 meneuverWait;
	public PlayerController.Boundary boundary;
	private Rigidbody rb;
	private float targetManuver;

	void Start () 
	{
		rb = GetComponent<Rigidbody> ();
		StartCoroutine (Evade ());
	}

	IEnumerator Evade()
	{
		yield return new WaitForSeconds(Random.Range(startWait.x,startWait.y));
		while (true) {
			targetManuver = Random.Range (1, dodge) * -Mathf.Sign (transform.position.x);
			yield return new WaitForSeconds (Random.Range (meneuverTime.x, meneuverTime.y));
			targetManuver = 0;
			yield return new WaitForSeconds (Random.Range (meneuverWait.x, meneuverWait.y));
		}
	}

	void FixedUpdate ()
	{
		float newMeneuver = Mathf.MoveTowards(rb.velocity.x, targetManuver, Time.deltaTime * smoothing);
		float currentSpeed = rb.velocity.z;
		rb.velocity = new Vector3 (newMeneuver, 0.0f, currentSpeed);
		rb.position = new Vector3 (
			Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
		);
		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
	}
}

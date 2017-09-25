using UnityEngine;
using System.Collections;

public class ShipShield : MonoBehaviour {
	public Transform ship;

	void FixedUpdate () {
		gameObject.transform.position = ship.position;
		Quaternion q = new Quaternion ();
		gameObject.transform.rotation = q;
	}
}

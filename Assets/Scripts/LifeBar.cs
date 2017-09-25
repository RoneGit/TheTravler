using UnityEngine;
using System.Collections;

public class LifeBar : MonoBehaviour {
	public GameObject life1;
	public GameObject life2;
	public GameObject life3;


	public void lossLife()
	{
		if (life3.activeSelf == true) {
			life3.SetActive (false);
		} else if (life2.activeSelf == true) {
			life2.SetActive (false);
		} else {
			life1.SetActive (false);
		}
	}
}

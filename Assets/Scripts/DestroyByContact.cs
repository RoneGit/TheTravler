using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {
	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;
	public int life;
	private GameController gameController;

	void Start ()
	{
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Boundary" || other.tag == "Enemy" || other.tag == "Boss") {
			//from the beggining thet stuff wont collid with boundary
			return;
		} else {
			if ((other.tag == "Player") && (gameObject.tag != "WeaponUpgrade") &&
				(gameObject.tag != "SpeedUpgrade") && (gameObject.tag != "ShieldUpgrade") && (gameObject.tag != "LifeUpgrade")) {
				collisionPlayerAndEnemy (other.transform);

			} else if ((gameObject.tag == "Player") && (other.tag != "WeaponUpgrade") && 
				(other.tag != "SpeedUpgrade") && (other.tag != "ShieldUpgrade") && (other.tag != "LifeUpgrade")) {
				collisionPlayerAndEnemy (gameObject.transform);

			} else if (other.tag == "ShipShield") {				
				collisionPlayerAndGameobjectWithShield (other.transform);

			} else if ((other.tag == "WeaponUpgrade") || (other.tag == "SpeedUpgrade") || (other.tag == "ShieldUpgrade") || (other.tag == "LifeUpgrade")) {
				if ((gameObject.tag == "Player") || (gameObject.tag == "ShipShield")) {					
					collisionPlayerAndUpgrade (other.tag, other.gameObject);

				}
			} else if ((gameObject.tag == "WeaponUpgrade") || (gameObject.tag == "SpeedUpgrade") || (gameObject.tag == "ShieldUpgrade") || (gameObject.tag == "LifeUpgrade")) {
				if ((other.tag == "Player") || (other.tag == "ShipShield")){
					collisionPlayerAndUpgrade (gameObject.tag, gameObject);

				}
			}
			else {
				collisionWithTwoObjects (other);
			}
		}
	}

	private void collisionPlayerAndEnemy(Transform transform)
	{
		//player collids with enemy or astroid

		//string messege = "player collids with enemy or astroid";
		//Debug.Log (messege);
		Instantiate (playerExplosion, transform.position, transform.rotation); 
		gameController.lossLife ();
	}

	private void collisionPlayerAndGameobjectWithShield(Transform colliderTransform)
	{
		//player collids gameobjects with shield on

		//string messege = "player collids gameobjects with shield on";
		//Debug.Log (messege);
		if (explosion != null) {
			Instantiate (explosion, colliderTransform.position, colliderTransform.rotation); 
		}
		if ((gameObject.tag == "WeaponUpgrade") || (gameObject.tag == "SpeedUpgrade") || (gameObject.tag == "ShieldUpgrade") || (gameObject.tag == "LifeUpgrade")) {
			gameController.collectUpgrade (gameObject.tag);
		} else { 
			gameController.addScore (scoreValue); 
		}
		Destroy (gameObject);
	}

	private void collisionPlayerAndUpgrade(string upgradeTag, GameObject objectToDestroy)
	{
		//player picks up upgrade
		//string messege = "player picks up upgrade";
		//Debug.Log (messege);
		gameController.collectUpgrade (upgradeTag);
		Destroy (objectToDestroy);
	}

	private void collisionWithTwoObjects(Collider collidingObject)
	{
		//enemy destroyed
		if (life == 1) {
			if (explosion != null) {
				Instantiate (explosion, collidingObject.transform.position, collidingObject.transform.rotation); 
			}
			//string messege = "enemy destroyed";
			//Debug.Log (messege);
			gameController.addScore (scoreValue);
			gameController.spawnUpgrade (gameObject.transform.position);
			Destroy (gameObject);
		} else {
			life--;
		}
		Destroy (collidingObject.gameObject);
	}
}

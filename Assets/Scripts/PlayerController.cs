using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed;
	public float bonusSpeed;
	public float tilt;
	public Boundary boundary;
	public GameObject shot;
	public Transform[] shotSpawn;
	public float fireRate;
	public float nextFire;
	public GameObject shipShield;
	public float spawnShieldDuration;
	public float bonusDuration;

    private Quaternion calibrationQuaternion;
    private int shotsUpgrade;
	private int maxShotsUpgrade;
	private bool bonusShieldAdded;
	private int bonusSpeedAdded;

	[System.Serializable]
	public class Boundary
	{
		public float xMin,xMax,zMin,zMax;
	}

	void Start()
	{
		shotsUpgrade = 0;
		bonusSpeed = 0;
		maxShotsUpgrade = 2;
		bonusShieldAdded = false;
		bonusSpeedAdded = 0;
        CalibrateAccelerometer();
	}

    //Used to calibrate the Iput.acceleration input
    public void CalibrateAccelerometer()
    {
        Vector3 accelerationSnapshot = Input.acceleration;
        Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f), accelerationSnapshot);
        calibrationQuaternion = Quaternion.Inverse(rotateQuaternion);
    }

    //Get the 'calibrated' value from the Input
    Vector3 FixAcceleration(Vector3 acceleration)
    {
        Vector3 fixedAcceleration = calibrationQuaternion * acceleration;
        return fixedAcceleration;
    }

    void FixedUpdate ()
	{
		//float moveHorizontal = Input.GetAxis ("Horizontal");
		//float moveVertical = Input.GetAxis ("Vertical");

		Vector3 accelerationRaw = Input.acceleration;
        Vector3 acceleration = FixAcceleration(accelerationRaw);
        Vector3 movement = new Vector3 (acceleration.x, 0.0f, acceleration.y);
		GetComponent<Rigidbody>().velocity = movement * (speed + bonusSpeed);
		GetComponent<Rigidbody>().rotation = Quaternion.Euler (0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);

		GetComponent<Rigidbody>().position = new Vector3 
			(
				Mathf.Clamp (GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax), 
				0.0f, 
				Mathf.Clamp (GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
			);
	}

	void Update()
	{
		if (Input.GetButton("Fire1") && (Time.time > nextFire))
		{
			nextFire = Time.time + fireRate;
			switch (shotsUpgrade)
			{
			case 0:
				Instantiate (shot, shotSpawn[0].position, shotSpawn[0].rotation); 
				break;
			case 1:
				twoShots ();
				break;
			case 2:
				threeShots ();
				break;
			}
			GetComponent<AudioSource> ().Play ();
		}
	}

	private void twoShots()
	{
		Instantiate (shot, shotSpawn[1].position, shotSpawn[1].rotation); 
		Instantiate (shot, shotSpawn[2].position, shotSpawn[2].rotation); 
	}

	private void threeShots()
	{
		Instantiate (shot, shotSpawn[0].position, shotSpawn[0].rotation); 
		Instantiate (shot, shotSpawn[3].position, shotSpawn[3].rotation); 
		Instantiate (shot, shotSpawn[4].position, shotSpawn[4].rotation); 
	}

	public void activeteShield(bool spawnOrBonus)
	{	
		float duration = spawnOrBonus == true ? spawnShieldDuration : bonusDuration;
		if (spawnOrBonus == false) { bonusShieldAdded = true;}
		StartCoroutine( shieldOff(duration));
		shipShield.SetActive (true);
	}

	IEnumerator shieldOff (float duration)
	{
		yield return new WaitForSeconds (duration);
		if (((bonusShieldAdded == true) && (duration == bonusDuration)) || ((bonusShieldAdded == false) && (duration == spawnShieldDuration))) {
			shipShield.SetActive (false);
			bonusShieldAdded = false;
		}
	}

	public void addShotsUpgrade(){
		if (shotsUpgrade < maxShotsUpgrade) {
			shotsUpgrade++;
		}
	}

	public void disableUpgrades(){
		shotsUpgrade = 0;
		bonusSpeed = 0;
		shipShield.SetActive (false);
	}

	public void activeSpeed()
	{
		StartCoroutine( disableSpeed());
		bonusSpeedAdded++;
		addSpeed ();
	}

	public void addSpeed(){
		bonusSpeed = 5;
	}

	IEnumerator  disableSpeed(){
		yield return new WaitForSeconds (bonusDuration);
		if (bonusSpeedAdded == 2) {
			bonusSpeedAdded = 0;
		} else {
			bonusSpeed = 0;
		}
	}
}

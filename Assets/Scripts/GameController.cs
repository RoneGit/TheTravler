using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
	public GameObject[] hazerds;
	public GameObject spaceShip;
	public GameObject[] Upgrades;
	public GameObject[] bossList;
	public PlayerController playerController;
	public Vector3 spawnValues;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	public Text gameOverText;
    public Text lifeText;
	public Text scoreText;
	public Text levelText;
    public Text finalScoreText;
	public RawImage life1;
	public RawImage life2;
	public RawImage life3;
    public Button StartGameButton;
    public Button RulesButton;
    public Button ExitButton;
    public GameObject MenuButton;
    public GameObject MenuPanel;
    public GameObject HowToPlayPanel;

    private int life;
    private int hazerdCount;
    private int score;
	private bool gameOver;
	private bool restart;
	private bool bossLevel;
	private bool finishedWave;
    private bool gameRunning;
	private int numberOfWaves;
	private bool bossSpawned;
	private int currentBossIndex;
	private int currentLevel;
	private GameObject currentBoss = null;
	private int bossLife;
	private DestroyByContact currentBossScript;

	void Start () {
        resetStats();
        //StartGameButton.onClick.AddListener(StartGame);
        //ExitButton.onClick.AddListener(Exit);
        //StartCoroutine (runGame());
    }

    void resetStats()
    {
        score = 0;
        life = 3;
        currentLevel = 0;
        currentBossIndex = -1;
        levelText.text = "Level: 1";
        gameOverText.text = "Game Over";
        bossLife = 20;
        hazerdCount = 5;
        numberOfWaves = 0;
        updateScore();
        Time.timeScale = 1;
        gameOverText.enabled = false;
        finalScoreText.enabled = false;
        gameOver = false;
        gameRunning = false;
        restart = false;
        bossLevel = false;
        finishedWave = false;
        bossSpawned = false;
    }

    void removeAllObjects()
    {
        string[] gameObjectsToRemove = new string[] { "Enemy", "Boss", "WeaponUpgrade", "ShieldUpgrade", "SpeedUpgrade", "LifeUpgrade" };
        int index = 0;
        foreach (string tag in gameObjectsToRemove)
        {

            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
            if (objects != null)
            {
                foreach (GameObject obg in objects)
                {
                    index++;
                
                    Destroy(obg);
                }
            }
        }
    }

    void showOrHideGame(bool showOrHide)
    {
        lifeText.enabled = showOrHide;
        scoreText.enabled = showOrHide;
        levelText.enabled = showOrHide;
        life1.enabled = showOrHide;
        life2.enabled = showOrHide;
        life3.enabled = showOrHide;
        MenuButton.SetActive(showOrHide);
        spaceShip.SetActive(showOrHide);
    }

	IEnumerator runGame()
	{
		while (gameOver == false) {
			if (finishedWave == false) {
				StartCoroutine (SpawnWaves ());
				finishedWave = true;
			}
            if (gameOver == true)
                break;
			if ((bossLevel == true) && (bossSpawned == false)){
				SpawnBoss ();
				bossSpawned = true;
				yield return new WaitForSeconds (5);
			}
			if (bossLevel == true) {
				//string messege = string.Format ("bossLevel: {0}", bossLevel.ToString());
				//Debug.Log (messege);
				if (currentBoss == null) {
					bossLevel = false;
					finishedWave = false;
					bossSpawned = false;
				}
			}
			yield return new WaitForSeconds (waveWait);
		}
	}

	IEnumerator SpawnWaves ()
	{
		int spawnedHazerd = 0;
		Vector3 spawnPosition;
		Quaternion spawnRotation;
		currentLevel++;
		currentBossIndex++;
		if (currentBossIndex > bossList.Length - 1) {
			currentBossIndex = 0;
		}
		hazerdCount +=10;
		numberOfWaves += 1;
		bossLife += 10;
		levelText.text = "Level: " + currentLevel;
        yield return new WaitForSeconds (startWait);
		for (int k = 0; k < numberOfWaves; k++) {
		
            spawnedHazerd = 0;
			while (spawnedHazerd < hazerdCount) {
                if (gameOver == true)
                    break;
                GameObject hazard = hazerds [Random.Range (0, hazerds.Length)];
				spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				spawnedHazerd++;
				yield return new WaitForSeconds (spawnWait);
			}
            if (gameOver == true)
                break;
            yield return new WaitForSeconds (waveWait);
		}
		bossLevel = true;
	}

	void SpawnBoss ()
	{
		Vector3 spawnPosition;
		Quaternion spawnRotation;
		spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
		spawnRotation = Quaternion.identity;
		currentBoss = (GameObject)Instantiate (bossList[currentBossIndex], spawnPosition, spawnRotation);
		currentBossScript = (DestroyByContact)currentBoss.GetComponent<DestroyByContact>();
		currentBossScript.life = bossLife;
	}

	void Update ()
	{
		if (gameOver == true) {		
			restart = true;
		} 
	}

	public void addScore(int newGameScore)
	{
		score += newGameScore;
		updateScore ();
	}

    void updateScore()
	{
		scoreText.text = "Score: " + score;
	}

	public void lossLife()
	{
		life--;
		deleteLife ();
		spaceShip.SetActive (false);
		StartCoroutine (spawn ());
		if (life == 0) {
			GameOver ();
		}
	}

	private void deleteLife()
	{
		if (life3.enabled == true) {
			life3.enabled = false;
		} else if (life2.enabled == true) {
			life2.enabled = false;
		} else {
			life1.enabled = false;
		}
	}

	private void addLife()
	{
		if (life2.enabled == false) {
			life2.enabled = true;
			life++;
		} else if (life3.enabled == false) {
			life3.enabled = true;
			life++;
		}
	}

	IEnumerator spawn ()
	{
		yield return new WaitForSeconds (spawnWait);
		spawnShip ();
	}

	public void spawnShip()
	{
		if (gameOver == false) 
		{
			spaceShip.transform.position = new Vector3 (0.0f, 0.0f);
			spaceShip.SetActive (true);
			playerController.disableUpgrades ();
			playerController.activeteShield (true);
		}
	}

	public void spawnUpgrade(Vector3 position)
	{
		int random = Random.Range (0, 20);
		if ((random == 0) || (random == 1)) {
			Instantiate (Upgrades [0], position, new Quaternion ()); 
		} else if (random == 2) {
			Instantiate (Upgrades [1], position, new Quaternion ()); 
		} else if (random == 3) {
			Instantiate (Upgrades [2], position, new Quaternion ()); 
		} else if (random == 4) {
			Instantiate (Upgrades [3], position, new Quaternion ()); 
		}
	}

	public void collectUpgrade(string upgrade)
	{
		if (upgrade == "WeaponUpgrade") {
			playerController.addShotsUpgrade ();
		} else if (upgrade == "ShieldUpgrade") {
			playerController.activeteShield (false);
		} else if (upgrade == "SpeedUpgrade") {
			playerController.activeSpeed ();
		} else if (upgrade == "LifeUpgrade") {
			addLife ();
		}
	}

	public void GameOver()
	{
        gameOverText.enabled = true;
        finalScoreText.text = "Your Score Is " + score;
        finalScoreText.enabled = true;
        gameOver = true;
        StartCoroutine(WaitForGameOver(6));
	}

    IEnumerator WaitForGameOver(float duration)
    {
        yield return new WaitForSeconds(duration);   //Wait
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClinkStartGame()
    {
        resetStats();
        gameRunning = true;
        removeAllObjects();
        MenuPanel.SetActive(false);
        showOrHideGame(true);
        playerController.CalibrateAccelerometer();
        StartCoroutine(runGame());
    }

    public void OnClinkExit()
    {
        Application.Quit();
    }

    public void OnClickShowMenu()
    {
        Time.timeScale = 0;
        //gameOver = true;
        //removeAllObjects();
        //showOrHideMenu(false);
        MenuPanel.SetActive(true);
    }

    public void OnClickResumeGame()
    {
        if (gameRunning == true)
        {
            Time.timeScale = 1;
            playerController.CalibrateAccelerometer();
            MenuPanel.SetActive(false);
            //showOrHideMenu(true);
        }
    }

    public void OnClinkHowToPlay()
    {
        HowToPlayPanel.SetActive(true);
        MenuPanel.SetActive(false);
    }

    public void OnClinkCloseHowToPlayPanel()
    {
        HowToPlayPanel.SetActive(false);
        MenuPanel.SetActive(true);
    }
}

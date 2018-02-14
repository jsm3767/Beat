using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;
using UnityEngine.UI;

public struct WaveEnemy
{

    public Vector2 spawnPosition;
    public int beat;
    public EnemyType enemyType;
    

    public WaveEnemy( Vector2 spawnPosition_in, int beat_in, EnemyType enemyType_in )
    {
        spawnPosition = spawnPosition_in;
        beat = beat_in;
        enemyType = enemyType_in;
    }
}

public enum EnemyType
{
    Basic,
    Shooter,
    Circling
}


public class GameManager : MonoBehaviour
{
    private Dictionary<EnemyType, GameObject> enemyDictionary;

    public GameObject shooterEnemy;
    public GameObject circlingEnemy;

    private List<Enemy> enemies;
    private MoveTick tick;

    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private GameObject playerOBJ;
    [SerializeField] private GameObject powerUp;
    [SerializeField] private GameObject bulletprefab;
    public List<string> activePowerUps;
    public List<int> timerForActivePowerUps;
    private Player player;

    int wave;

    private float timer = 0;
    float bpm = 176.0f;
    private float secondsPerBeat;

    [SerializeField] private List<AudioClip> songs;
    [SerializeField] private List<float> songBPM;
    [SerializeField] private int track = 2;
    private AudioSource audio;

	bool gameStarted = false;

    private float halfHeight;
    private float halfWidth;

    private int shake = 0;
    private VignetteAndChromaticAberration[] ChromAbb;
    [SerializeField] GameObject mainCamOBJ;

    private int score = 0;
    private int highScore = 0;
    private int pointValue = 10;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;
    public float HalfHeight
    {
        get { return halfHeight; }
    }
    public float HalfWidth
    {
        get { return halfWidth; }
    }

    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    public float Timer
    {
        get { return timer; }
        set { timer = value; }
    }
    public float SecondsPerBeat
    {
        get { return secondsPerBeat; }
        set { secondsPerBeat = value; }
    }
    // Use this for initialization
    void Start()
    {
		audio = GetComponent<AudioSource>();
		AudioClip currentSong = songs[3];
		bpm = songBPM[3];
		audio.clip = currentSong;
		audio.Play();
		player = playerOBJ.GetComponent<Player>();
		secondsPerBeat = 60.0f / bpm;
        score = 0;
        enemyDictionary = new Dictionary<EnemyType, GameObject>();
        enemyDictionary.Add( EnemyType.Basic, EnemyPrefab );
        enemyDictionary.Add( EnemyType.Shooter, shooterEnemy );
        enemyDictionary.Add( EnemyType.Circling, circlingEnemy);

        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * ( ( (float)Screen.width / (float)Screen.height ) );

        enemies = new List<Enemy>();
		tick = FindObjectOfType<MoveTick>();

		ChromAbb = mainCamOBJ.GetComponents<VignetteAndChromaticAberration>();

        scoreText.gameObject.SetActive(false);

        if(PlayerPrefs.HasKey("playerscore"))
        {
            highScore = PlayerPrefs.GetInt("playerscore");
        }
        else
        {
            PlayerPrefs.SetInt("playerscore", highScore);
        }

        highScoreText.text = "HIGH: " + highScore;
    }

    // Update is called once per frame
    void Update()
    {
        
        timer += Time.deltaTime;
        //Debug.Log(timer);
        if (timer > secondsPerBeat)
        {
            Pulse();
            timer -= secondsPerBeat;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        List<Enemy> deadEnemies = new List<Enemy>();
        foreach (Enemy e in enemies)
        {
            if (!e.alive)
            {
                deadEnemies.Add(e);
            }
        }

        foreach (Enemy e in deadEnemies)
        {
            score += pointValue;
            scoreText.text = "" + score;
            enemies.Remove(e);
            Destroy(e.gameObject);
        }

		if (enemies.Count == 0 && gameStarted)
        {
            wave++;
            //Some hardcoding, TODO change
            //for testing purposes
            if( wave == 3 )
            {
                Debug.Log( halfWidth );
                Debug.Log( halfHeight );
                List<WaveEnemy> wave1 = new List<WaveEnemy>();
                wave1.Add(new WaveEnemy(new Vector2(-(halfWidth), (halfHeight)), 0, EnemyType.Shooter));
                wave1.Add(new WaveEnemy(new Vector2(+(halfWidth), 0.0f), 0, EnemyType.Shooter));
                wave1.Add(new WaveEnemy(new Vector2(-(halfWidth), (halfHeight)), 1, EnemyType.Shooter));
                wave1.Add(new WaveEnemy(new Vector2(+(halfWidth), 0.0f), 1, EnemyType.Shooter));
                wave1.Add(new WaveEnemy(new Vector2(-(halfWidth), (halfHeight)), 2, EnemyType.Shooter));
                wave1.Add(new WaveEnemy(new Vector2(+(halfWidth), 0.0f), 2, EnemyType.Shooter));
                wave1.Add(new WaveEnemy(new Vector2(-(halfWidth), (halfHeight)), 3, EnemyType.Shooter));
                wave1.Add(new WaveEnemy(new Vector2(+(halfWidth), 0.0f), 3, EnemyType.Shooter));
                wave1.Add(new WaveEnemy(new Vector2(-(halfWidth), (halfHeight)), 4, EnemyType.Shooter));
                wave1.Add(new WaveEnemy(new Vector2(+(halfWidth), 0.0f), 4, EnemyType.Shooter));
                wave1.Add(new WaveEnemy(new Vector2(-(halfWidth), (halfHeight)), 5, EnemyType.Shooter));
                wave1.Add(new WaveEnemy(new Vector2(+(halfWidth), 0.0f), 5, EnemyType.Shooter));
                wave1.Add(new WaveEnemy(new Vector2(-(halfWidth), (halfHeight)), 6, EnemyType.Shooter));
                wave1.Add(new WaveEnemy(new Vector2(+(halfWidth), 0.0f), 6, EnemyType.Shooter));

                StartCoroutine( SpawnWaveAsync( wave1 ) );
            }
            else if( wave == 2 )
            {
                List<WaveEnemy> wave2 = new List<WaveEnemy>();
                wave2.Add(new WaveEnemy(new Vector2(0.0f, halfWidth / 5.0f),
                    0, EnemyType.Circling));
                wave2.Add(new WaveEnemy(new Vector2(0.0f, halfWidth / 5.0f),
                    1, EnemyType.Circling));
                wave2.Add(new WaveEnemy(new Vector2(0.0f, halfWidth / 5.0f),
                    2, EnemyType.Circling));
                wave2.Add(new WaveEnemy(new Vector2(0.0f, halfWidth / 5.0f),
                    3, EnemyType.Circling));
                wave2.Add(new WaveEnemy(new Vector2(0.0f, halfWidth / 5.0f),
                    4, EnemyType.Circling));
                wave2.Add(new WaveEnemy(new Vector2(0.0f, halfWidth / 5.0f),
                    5, EnemyType.Circling));
                wave2.Add(new WaveEnemy(new Vector2(0.0f, halfWidth / 5.0f),
                    6, EnemyType.Circling));

                StartCoroutine(SpawnWaveAsync(wave2));
            }
            else
            {
                SpawnWave();
            }
        }
        
        if( player.transform.position.x < -halfWidth || player.transform.position.x > halfWidth || player.transform.position.y < -halfHeight || player.transform.position.y > halfHeight )
        {
            if (gameStarted)
            {
                KillPlayer();
            }
            else
            {
                player.transform.position = new Vector3(0, 0, 0);
            }
        }

		PulseCamEffects ();
        
    }

	private void PulseCamEffects(){
		if (shake > 0)
		{
			Camera.main.transform.position += ((Vector3)Random.insideUnitCircle);
			shake--;
		}
		else
		{
			Camera.main.transform.position = new Vector3(0, 0, -10);
		}

		ChromAbb [0].mode = VignetteAndChromaticAberration.AberrationMode.Simple;
		ChromAbb [1].mode = VignetteAndChromaticAberration.AberrationMode.Advanced;

		if(ChromAbb [0].chromaticAberration > 0)
		{
			ChromAbb [0].chromaticAberration-=5f;
		}

		if(ChromAbb [1].axialAberration > 0)
		{
			ChromAbb [1].axialAberration-=0.5f;
		}
	}

    IEnumerator SpawnWaveAsync( List<WaveEnemy> wave )
    {
        //int nBeats = wave.Count;

        int max = -1;
        int currentBeat = 0;
        //need data for which enemy to spawn per beat and where to put them
        for( int index = 0; index < wave.Count; index++ )
        {
            if( wave[ index ].beat > max )
            {
                max = wave[ index ].beat;
            }
        }

        while( currentBeat < max + 1 )
        {
            for( int index = 0; index < wave.Count; index++ )
            {
                if( wave[ index ].beat == currentBeat )
                {
                    SpawnEnemy( wave[ index ].enemyType, wave[ index ].spawnPosition );
                }
            }

            currentBeat++;
            //TODO refactor later if this is too inaccurate
            yield return new WaitForSeconds( secondsPerBeat );
        }
    }
    void SpawnEnemy(EnemyType enemyType, Vector2 spawnPosition)
    {
        if (enemyType == EnemyType.Circling)
        {
            int x = 0;
        }
        GameObject e = Instantiate( enemyDictionary[enemyType] );
        e.SendMessage( "SetGameManager", this );
        e.SendMessage( "SetPlayer", playerOBJ );
        e.transform.position = spawnPosition;
        enemies.Add( e.GetComponent<Enemy>() );
    }

    public void StartGame(int difficulty)
    {
        Debug.Log("Start Game");
		int trackNum = difficulty;
        pointValue -= difficulty*3;
        AudioClip currentSong = songs[trackNum];
        bpm = songBPM[trackNum];
        audio.clip = currentSong;
        audio.Play();

        wave = 1;
        secondsPerBeat = 60.0f / bpm;
        SpawnWave();
		gameStarted = true;
        activePowerUps = new List<string>();
        timerForActivePowerUps = new List<int>();

        player.transform.position = new Vector3(0, 0, 0);

		timer = 0;
        scoreText.gameObject.SetActive(true);
    }


    void SpawnWave()
    {
        Debug.Log("Spawn Wave");
        for (int i = 0; i < wave; i++)
        {
            GameObject e = Instantiate(EnemyPrefab);
            Enemy eScript = e.GetComponent<Enemy>();
            eScript.PlayerObject = playerOBJ;
            eScript.GM = this;
            float angle = Random.Range(0, 100);
            e.transform.position = new Vector3(Mathf.Sin(angle) * 130, Mathf.Cos(angle) * 70, 0);
            enemies.Add(eScript);
        }
    }

    void Pulse()
    {
        if (gameStarted)
        {
            if (!Input.GetMouseButtonDown(0))
            {
                player.Pulse();
            }
            tick.Pulse();
            PowerUpPulse();
            foreach (Enemy e in enemies)
            {
                if (e.alive)
                {
                    e.SendMessage( "Pulse" );
                }
            }
        }
        else
        {
            PowerUpPulse();
            player.Pulse();
            tick.Pulse();
            
        }

        shake = 1;
		ChromAbb [0].chromaticAberration = 30;
		ChromAbb [1].axialAberration = 5;
    }

    public void AddPowerUp(string powerupname, int delay)
    {
        activePowerUps.Add(powerupname);
        timerForActivePowerUps.Add(delay);
    }


    void PowerUpPulse()
    {
        for (int i = 0; i < activePowerUps.Count; i++)
        {
            if (timerForActivePowerUps[i] > 0)
            {
                timerForActivePowerUps[i]--;
                Debug.Log(timerForActivePowerUps[i]);
            }
            else
            {
                switch (activePowerUps[i])
                {
                    case "TestPowerUp":
                        for (int d = 0; d < 8; d++)
                        {
                            GameObject newbullet = Instantiate(bulletprefab);
                            Vector3 newPos = new Vector3(0, 1, 0);
                            newPos = Quaternion.AngleAxis(45 * d, Vector3.forward) * newPos;
                            newbullet.transform.position = player.transform.position + (newPos * 2);
                            Debug.Log(newPos.x);
                            newbullet.GetComponent<BulletMovement>().FireBullet(newPos * 100);
                        }
                        break;
                }
                activePowerUps.RemoveAt(i);
                timerForActivePowerUps.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < activePowerUps.Count; i++)
        {
        }
    }

    public void KillPlayer()
    {
        int high = PlayerPrefs.GetInt("playerscore");
        if(score > high)
        {
            PlayerPrefs.SetInt("playerscore", score);
        }
		player.Die ();
		PulseCamEffects ();

        StartCoroutine(ReloadLevel(1));
    }

    private IEnumerator ReloadLevel(float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    Shooter
}


public class GameManager : MonoBehaviour
{
    private Dictionary<EnemyType, GameObject> enemyDictionary;
    public GameObject shooterEnemy;

    private List<Enemy> enemies;
    private MoveTick tick;

    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private GameObject playerOBJ;
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

    public float HalfHeight
    {
        get { return halfHeight; }
    }
    public float HalfWidth
    {
        get { return halfWidth; }
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
		AudioClip currentSong = songs[2];
		bpm = songBPM[2];
		audio.clip = currentSong;
		audio.Play();
		player = playerOBJ.GetComponent<Player>();
		secondsPerBeat = 60.0f / bpm;

        enemyDictionary = new Dictionary<EnemyType, GameObject>();
        enemyDictionary.Add( EnemyType.Basic, EnemyPrefab );
        enemyDictionary.Add( EnemyType.Shooter, shooterEnemy );
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * ( ( (float)Screen.width / (float)Screen.height ) );

        enemies = new List<Enemy>();
		tick = FindObjectOfType<MoveTick>();
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

        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (Enemy e in enemies)
            {
                e.alive = false;
            }
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
            enemies.Remove(e);
            Destroy(e.gameObject);
        }

		if (enemies.Count == 0 && gameStarted)
        {
            wave++;
            //Some hardcoding, TODO change
            //for testing purposes
            if( wave == 2 )
            {
                Debug.Log( halfWidth );
                Debug.Log( halfHeight );
                List<WaveEnemy> wave1 = new List<WaveEnemy>();
                wave1.Add( new WaveEnemy( new Vector2( -( halfWidth ), ( halfHeight ) ), 0, EnemyType.Shooter ) );
                wave1.Add( new WaveEnemy( new Vector2( +( halfWidth ), 0.0f ), 0, EnemyType.Shooter ) );
                wave1.Add( new WaveEnemy( new Vector2( -( halfWidth ), ( halfHeight ) ), 1, EnemyType.Shooter ) );
                wave1.Add( new WaveEnemy( new Vector2( +( halfWidth ), 0.0f ), 1, EnemyType.Shooter ) );
                wave1.Add( new WaveEnemy( new Vector2( -( halfWidth ), ( halfHeight ) ), 2, EnemyType.Shooter ) );
                wave1.Add( new WaveEnemy( new Vector2( +( halfWidth ), 0.0f ), 2, EnemyType.Shooter ) );
                wave1.Add( new WaveEnemy( new Vector2( -( halfWidth ), ( halfHeight ) ), 3, EnemyType.Shooter ) );
                wave1.Add( new WaveEnemy( new Vector2( +( halfWidth ), 0.0f ), 3, EnemyType.Shooter ) );
                wave1.Add( new WaveEnemy( new Vector2( -( halfWidth ), ( halfHeight ) ), 4, EnemyType.Shooter ) );
                wave1.Add( new WaveEnemy( new Vector2( +( halfWidth ), 0.0f ), 4, EnemyType.Shooter ) );

                StartCoroutine( SpawnWaveAsync( wave1 ) );
            }
            else
            {
                SpawnWave();
            }
        }
        
        if( player.transform.position.x < -halfWidth || player.transform.position.x > halfWidth || player.transform.position.y < -halfHeight || player.transform.position.y > halfHeight )
        {
            KillPlayer();
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

        while( currentBeat < max )
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
    void SpawnEnemy( EnemyType enemyType, Vector2 spawnPosition )
    {
        GameObject e = Instantiate( enemyDictionary[enemyType] );
        e.SendMessage( "SetGameManager", this );
        e.SendMessage( "SetPlayer", playerOBJ );
        e.transform.position = spawnPosition;
        enemies.Add( e.GetComponent<Enemy>() );
    }

    public 	void StartGame(int difficulty)
    {
        Debug.Log("Start Game");
		int trackNum = difficulty;
        AudioClip currentSong = songs[trackNum];
        bpm = songBPM[trackNum];
        audio.clip = currentSong;
        audio.Play();

        wave = 1;
        secondsPerBeat = 60.0f / bpm;
        SpawnWave();
		gameStarted = true;

        player.transform.position = new Vector3(0, 0, 0);
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
            player.Pulse();
            tick.Pulse();
        }
    }

    public void KillPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

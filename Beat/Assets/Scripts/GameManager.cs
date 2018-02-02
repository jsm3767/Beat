using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

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
            wave++; SpawnWave();
        }
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
                    e.Pulse();
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

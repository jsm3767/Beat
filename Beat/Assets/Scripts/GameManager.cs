using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private List<Enemy> enemies;
    private MoveTick[] ticks;

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
    void Start () {
		StartGame ();        
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

        if (enemies.Count == 0)
        {
            wave++; SpawnWave();
        }
    }

	void StartGame(){
		audio = GetComponent<AudioSource> ();
		int trackNum = Random.Range (0, songs.Count);

		trackNum = track;

		AudioClip currentSong = songs[trackNum];
		bpm = songBPM [trackNum];
		audio.clip = currentSong;
		audio.Play ();

		wave = 1;
		player = playerOBJ.GetComponent<Player>();
		ticks = FindObjectsOfType<MoveTick>();
		secondsPerBeat = 60.0f / bpm;
		enemies = new List<Enemy>();
		SpawnWave();
	}


    void SpawnWave()
    {
        for( int i =0; i < wave; i++)
        {
            GameObject e = Instantiate(EnemyPrefab);
            Enemy eScript = e.GetComponent<Enemy>();
            eScript.PlayerObject = playerOBJ;
			eScript.GM = this;
			e.transform.position = new Vector3(Mathf.Sin(Random.Range(0,100))*130, Mathf.Cos(Random.Range(0,100))*70, 0);
            enemies.Add(eScript);
        }
    }

    void Pulse()
    {
        player.Pulse();
        
        foreach( MoveTick t in ticks )
        {
            t.Pulse();
        }

        foreach(Enemy e in enemies)
        {
            if (e.alive)
            {
                e.Pulse();
            }
        }
    }

	public void KillPlayer(){
		Application.LoadLevel (Application.loadedLevel);
	}
}

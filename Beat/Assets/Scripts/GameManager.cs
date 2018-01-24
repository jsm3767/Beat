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
    [SerializeField] float bpm = 176.0f;
    private float secondsPerBeat;

    public AudioSource test;
    public AudioSource song;

    
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
        wave = 1;
        player = playerOBJ.GetComponent<Player>();
        ticks = FindObjectsOfType<MoveTick>();
        secondsPerBeat = 60.0f / bpm;
        enemies = new List<Enemy>();
        SpawnWave();
        
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


    void SpawnWave()
    {
        for( int i =0; i < wave; i++)
        {
            GameObject e = Instantiate(EnemyPrefab);
            Enemy eScript = e.GetComponent<Enemy>();
            eScript.PlayerObject = playerOBJ;
            e.transform.position = new Vector3(Mathf.Sin(i)*20, Mathf.Cos(i)*20, 0);
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
}

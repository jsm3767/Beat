﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private List<Enemy> enemies;
	[SerializeField] private GameObject EnemyPrefab;

    [SerializeField] private GameObject playerOBJ;
	private Player player;

	int wave;

    private float timer = 0;
    [SerializeField] float bpm = 176.0f;
    private float milisecondsPerBeat;

    public AudioSource test;
    public AudioSource song;

    // Use this for initialization
    void Start () {
        wave = 1;
        player = playerOBJ.GetComponent<Player>();
        milisecondsPerBeat = 60.0f / bpm;
        enemies = new List<Enemy>();
        SpawnWave();
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        Debug.Log(timer);
        if(timer > milisecondsPerBeat)
        {
            Pulse();
            timer -= milisecondsPerBeat;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach(Enemy e in enemies)
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
        for( int i = 0; i < wave; i++)
        {
            GameObject e = Instantiate(EnemyPrefab);
            Debug.Log(e);
            Enemy eScript = e.GetComponent<Enemy>();
            Debug.Log(eScript);
            eScript.PlayerObject = playerOBJ;
            e.transform.position = new Vector3(Mathf.Sin(i)*20, Mathf.Cos(i)*20, 0);
            enemies.Add(eScript);
            Debug.Log(enemies);
        }
    }

    void Pulse()
    {
        player.Pulse();

        
        foreach(Enemy e in enemies)
        {
            if (e.alive)
            {
                e.Pulse();
            }
        }
        
    }


}

using System.Collections;
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
    private float secondsPerBeat = 0;

    public AudioSource test;
    public AudioSource song;

    // Use this for initialization
    void Start () {
        wave = 0;
        player = playerOBJ.GetComponent<Player>();
        secondsPerBeat = 60.0f / (bpm / 2.0f);
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if(timer > secondsPerBeat)
        {
            Pulse();
            timer -= secondsPerBeat;
        }

    }

    void Pulse()
    {
        player.Pulse();
        //foreach(Enemy e in enemies)
        //{
        //    e.Pulse();
        //}
    }


}

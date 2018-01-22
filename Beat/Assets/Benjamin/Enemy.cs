using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float timer = 0;
    private float bpm = 176.0f;
    private float secondsPerBeat = 0;

    private float moveSpeed = 5.0f;

    private Rigidbody2D rb;

    public GameObject playerObject;
    private Player playerRef;
    private Vector2 vectorToPlayer;

    public AudioSource test;
    public AudioSource song;
    
    bool started = false;

    //private float offset = 0.585f; //time to wait before first beat starts

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerRef = playerObject.GetComponent<Player>();
        secondsPerBeat = 60.0f / (bpm / 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        vectorToPlayer = playerRef.GetPosition() - this.transform.position;

        if (timer > secondsPerBeat)
        {
            timer = timer - secondsPerBeat; //important: carry over so it doesn't go off beat
            Pulse();
        }


        
    }

    public void Pulse()
    {
        rb.AddForce(vectorToPlayer.normalized * 5.0f, ForceMode2D.Impulse);
        test.Play();
        //shoot backward

    }
}


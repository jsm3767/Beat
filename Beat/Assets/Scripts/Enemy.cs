﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 5.0f;

    protected Rigidbody2D rb;

    public GameObject playerObject;

    protected Player playerRef;
    protected Vector2 vectorToPlayer;

    public bool alive = true;

    protected GameManager gm;
    public GameManager GM { get { return gm; } set { gm = value; } }

    public GameObject PlayerObject { get { return playerObject; } set { playerObject = value; } }

    public bool started = true;

    //private float offset = 0.585f; //time to wait before first beat starts

    // Use this for initialization
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerRef = playerObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void Pulse()
    {
        if( !started )
            Start();

        vectorToPlayer = playerRef.GetPosition() - this.transform.position;
        Vector2 direction = new Vector2 (vectorToPlayer.x * (float)(Random.Range(80,120)/100.0f),vectorToPlayer.y * (float)(Random.Range(80,120)/100.0f));
        rb.AddForce( direction.normalized * moveSpeed, ForceMode2D.Impulse );
    }

    public virtual void OnTriggerEnter2D( Collider2D other )
    {
        if( other.gameObject.tag == "Bullet" )
        {
            alive = false;
            Destroy( other.gameObject );
        }
        else if( other.gameObject.tag == "Player" )
        {
            gm.KillPlayer();
        }
    }

    void SetGameManager( GameManager gameManager_in )
    {
        this.gm = gameManager_in;
    }

    void SetPlayer( GameObject player_in )
    {
        this.playerObject = player_in;
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy
{
    private float circleRadius = 200.0f;
    private bool shoots = false;
    private Vector2 spawnPosition;
    private bool direction = true;
    private bool onScreen = false;
    private Vector2 moveVectorPositive;
    private Vector2 moveVectorNegative;
    private Vector2 moveVector;
    // Use this for initialization
    public override void Start()
    {
        base.Start();

        moveSpeed = 55.0f;

        moveVectorPositive = new Vector2( 6.0f, -1.0f );
        moveVectorNegative = new Vector2( -6.0f, -1.0f );

        if( direction )
        {
            moveVector = moveVectorPositive.normalized * moveSpeed;
        }
        else
        {
            moveVector = moveVectorNegative.normalized * moveSpeed;
        }

    }

    public override void Pulse()
    {
        //vectorToPlayer = playerRef.GetPosition() - this.transform.position;
        //Vector2 temp = vectorToPlayer.normalized * circleRadius;
        //temp = (Vector2)transform.position - temp; //vector to destination
        //rb.AddForce( temp.normalized * moveSpeed, ForceMode2D.Impulse );

        Vector2 temp =  (Vector2)transform.position;

        if( temp.x + 3.0f > gm.HalfWidth )
        {
            direction = false;
        }
        if( temp.x - 3.0f < -( gm.HalfWidth ) )
        {
            direction = true;
        }

        if( direction )
        {
            moveVector = moveVectorPositive.normalized * moveSpeed;
        }
        else
        {
            moveVector = moveVectorNegative.normalized * moveSpeed;
        }
        rb.AddForce( moveVector, ForceMode2D.Impulse );


        if( transform.position.y < -( gm.HalfHeight ) || transform.position.y > ( gm.HalfHeight ) )
        {
            alive = false;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Circles the player at a distance, does not die off screen
//Todo: add separation
public class FloaterEnemy : Enemy
{
    private float circleRadius = 10.0f;
    private bool shoots = false;
    private Vector2 spawnPosition;
    private Vector2 direction;

    private int interval = 3;
    private int iterator = 0;
    private bool puffed;



    private Vector2 moveVector;
    // Use this for initialization

    public override void Start()
    {
        base.Start();

        moveSpeed = 20.0f;
        direction = new Vector2( Random.value -.5f, Random.value - .5f ).normalized;
            
    }

    public override void Pulse()
    {
        iterator++;

        if( puffed )
        { 
            puffed = false;
            transform.localScale /= 3.0f;
        }
        if( iterator == interval )
        {
            puffed = true;
            transform.localScale *= 3.0f;
            iterator = 0;
        }
        
        //test different amounts of distanceCorrection
        
        if( transform.position.x > gm.HalfWidth )
        {
            if( direction.x > 0 )
                direction.x = -direction.x;
        }
        else if(transform.position.x < -gm.HalfWidth )
        {
            if( direction.x < 0 )
                direction.x = -direction.x;
        }

        if( transform.position.y > gm.HalfHeight  )
        {
            if( direction.y > 0 )
                direction.y = -direction.y;
        }
        else if(transform.position.y < -gm.HalfHeight )
        {
            if( direction.y < 0 )
                direction.y = -direction.y;
        }
        //if( transform.position.y < -( gm.HalfHeight ) || transform.position.y > ( gm.HalfHeight ) )
        //{
        //    alive = false;
        //}

    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce( direction * moveSpeed, ForceMode2D.Force );
    }
}

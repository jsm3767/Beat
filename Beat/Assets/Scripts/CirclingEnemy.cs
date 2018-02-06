using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Circles the player at a distance, does not die off screen
//Todo: add separation
public class CirclingEnemy : Enemy
{
    private float circleRadius = 10.0f;
    private bool shoots = false;
    private Vector2 spawnPosition;
    private bool direction;
    

    private Vector2 moveVector;
    // Use this for initialization

    public override void Start()
    {
        base.Start();

        moveSpeed = 50.0f;
        direction = (Random.value < .5f);
    }

    public override void Pulse()
    {
        Vector2 temp = ((Vector2)transform.position - (Vector2)playerRef.GetPosition());
        Vector2 perp;

        if (direction)
        { 
            perp = new Vector2(-temp.y, temp.x);
        }
        else
        {
            perp = new Vector2(temp.y, -temp.x);
        }

        Vector2 distanceCorrection = new Vector2();
        if( temp.magnitude < circleRadius)
        {
            distanceCorrection = temp.normalized * (circleRadius - temp.magnitude);
        }
        else if( temp.magnitude >= circleRadius )
        {
            distanceCorrection = -temp.normalized * (temp.magnitude - circleRadius);
        }

        //test different amounts of distanceCorrection
        rb.AddForce( (perp.normalized +( distanceCorrection.normalized * .5f)).normalized * moveSpeed , ForceMode2D.Impulse );


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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy
{
    private float circleRadius = 200.0f;

    // Use this for initialization
    void Start()
    {

    }

    public override void Pulse()
    {
        vectorToPlayer = playerRef.GetPosition() - this.transform.position;
        Vector2 temp = vectorToPlayer.normalized * circleRadius;
        temp = (Vector2)transform.position - temp; //vector to destination
        rb.AddForce(temp.normalized * moveSpeed, ForceMode2D.Impulse);

    }

    // Update is called once per frame
    void Update()
    {

    }
}

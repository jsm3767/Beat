﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{

    Vector2 mousePosition;
    private Vector2 vectorToMouse;

    public Rigidbody2D bulletRidgidBody;

    // Use this for initialization
    void Start()
    {
        bulletRidgidBody = GetComponent<Rigidbody2D>();

        Destroy(this, 10);
    }

    public void FireBullet(Vector3 direction)
    {
        bulletRidgidBody = GetComponent<Rigidbody2D>();
        bulletRidgidBody.AddForce(direction * 15);
        //gameObject.transform.forward = (gameObject.transform.position + direction) - gameObject.transform.position;
        //float angle = Mathf.Atan2(gameObject.transform.position.y + direction.y, gameObject.transform.position.x - direction.x);
        //this.transform.rotation = new Quaternion(0, 0, 0, 0);
        //this.transform.RotateAroundLocal(new Vector3(1, 0, 0), 45);
        //this.transform.RotateAroundLocal(new Vector3(0, 0, 1), 45);
    }

    // Update is called once per frame
    void Update()
    {

    }


}

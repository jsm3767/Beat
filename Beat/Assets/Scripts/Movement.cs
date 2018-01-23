using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{


    public float beatsPerMinute;
    public float spaceshipSpeed;

    //vectors for movement

    public Vector3 direction;
    public Vector3 acceleration;
    public Vector3 velocity;
    public Vector3 vectorToMousePosition;

    public float spaceshipWeight;
    public float timeBetweenBeats;

    public float time = 0;

    public GameObject bulletPrefab;

    /// Use this for initialization
    void Start()
    {
        timeBetweenBeats = 60 / beatsPerMinute;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        time += Time.deltaTime;
        if (time > timeBetweenBeats)
        {
            ApplyForceAwayFromMouseCursor();
            time = 0;
            velocity = Vector3.zero;
            MoveTheSpaceship();
            shootBullet();
        }
        else
        {
            MoveTheSpaceship();
            SlowShipDown();
            //wrap();
        }
    }

    /// <summary>
    /// applies a force to the sprite
    /// </summary>
    /// <param name="force"></param>
    void ApplyForce(Vector3 force)
    {
        acceleration += force / spaceshipWeight;
    }

    /// <summary>
    ///slows the ship down
    ///Note: Needs a little more work fine tuning the numbers, It feels alright at slower bpm, less impactful at high bpm
    /// </summary>
    void SlowShipDown()
    {
        ApplyForce(-direction * (spaceshipSpeed) * timeBetweenBeats);
    }


    /// <summary>
    /// ///Gets the mouse's position on screen, creates a vector between the sprite and the mouse position. Normalizes that vector then applies it to the sprite as a force multiplied by
    ///the speed of the spaceship and some constant value. I chose 20 because I like how that feels, this can be done other ways it'll just have to be discussed.
    /// </summary>
    void ApplyForceAwayFromMouseCursor()
    {
        Vector2 cursorPositionToWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 directionOfPush = gameObject.transform.position - new Vector3(cursorPositionToWorldPosition.x, cursorPositionToWorldPosition.y);
        directionOfPush.Normalize();
        ApplyForce(directionOfPush * spaceshipSpeed * 20);
        float angle = Mathf.Atan2(cursorPositionToWorldPosition.y, cursorPositionToWorldPosition.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    /// <summary>
    /// Calculates new position based on velocity and acceleration. Recalculates direction.
    /// </summary>
    void MoveTheSpaceship()
    {
        Vector3 newPosition = gameObject.transform.position;

        velocity += acceleration;

        newPosition += velocity * Time.deltaTime;

        acceleration = Vector3.zero;

        direction = velocity.normalized;

        gameObject.transform.position = newPosition;
    }
    /// <summary>
    /// Wraps the sprite to the other side of the screen
    /// </summary>
    void wrap()
    {
        if (gameObject.transform.position.x > 70 || gameObject.transform.position.x < -70)
        {
            Vector3 newPos = new Vector3(-gameObject.transform.position.x, gameObject.transform.position.y, 0);
            gameObject.transform.position = newPos;
        }
        if (gameObject.transform.position.y > 50 || gameObject.transform.position.y < -50)
        {
            Vector3 newPos = new Vector3(gameObject.transform.position.x, -gameObject.transform.position.y, 0);
            gameObject.transform.position = newPos;
        }
    }

    void shootBullet()
    {
        GameObject newbulletPrefab = Instantiate(bulletPrefab);
        newbulletPrefab.transform.forward = direction;
        newbulletPrefab.transform.position = gameObject.transform.position;
        newbulletPrefab.transform.Rotate(new Vector3(0, 1, 0), 90);
    }
}

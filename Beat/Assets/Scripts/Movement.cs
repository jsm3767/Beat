using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

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

    // Use this for initialization
    void Start () {
        timeBetweenBeats = 60 / beatsPerMinute;
    }
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (time > timeBetweenBeats)
        {
            ApplyForceAwayFromMouseCursor();
            time = 0;
            velocity = Vector3.zero;
        }
        MoveTheSpaceship();
        SlowShipDown();
    }


    void AddForce(Vector3 force)
    {
        acceleration += force.normalized * 10 / spaceshipWeight;
    }

    void SlowShipDown()
    {

    }

    void ApplyForceAwayFromMouseCursor()
    {
        Vector2 cursorPositionToWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        AddForce(gameObject.transform.position - new Vector3(cursorPositionToWorldPosition.x, cursorPositionToWorldPosition.y));
    }

    void MoveTheSpaceship()
    {
        Vector3 newPosition = gameObject.transform.position;

        velocity += acceleration;

        newPosition += velocity * Time.deltaTime;

        acceleration = Vector3.zero;

        direction = velocity.normalized;

        gameObject.transform.position = newPosition;

    }
}

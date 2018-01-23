using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float speed = 4.0f;
    Vector2 mousePosition;
    
    private float moveSpeed = 5.0f;
    private Rigidbody2D rb;
    private Vector2 vectorToMouse;
    //private Collider2D 

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       // float d = speed * Time.deltaTime;
        mousePosition = Camera.main.ScreenPointToRay( Input.mousePosition ).origin;
        vectorToMouse = mousePosition - (Vector2)transform.position;

        Debug.Log( mousePosition );
        
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }

    public void Pulse()
    {
        rb.AddForce( -vectorToMouse.normalized * 5.0f, ForceMode2D.Impulse );
        //shoot backward

    }
}

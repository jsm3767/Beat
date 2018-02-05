using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 4.0f;
    Vector2 mousePosition;
    
    private float moveSpeed = 5.0f;
    private Rigidbody2D rb;
    private Vector2 vectorToMouse;
    //private Collider2D 

    public GameObject bulletPrefab;
    // Use this for initialization

    public ParticleSystem gunSmoke;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gunSmoke = gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
       // float d = speed * Time.deltaTime;
        mousePosition = Camera.main.ScreenPointToRay( Input.mousePosition ).origin;
        vectorToMouse = mousePosition - (Vector2)transform.position;
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }

    public void Pulse()
    {
        rb.AddForce( -vectorToMouse.normalized * speed, ForceMode2D.Impulse );

        float angle = Mathf.Atan2(vectorToMouse.y, vectorToMouse.x);
        this.transform.rotation = new Quaternion(0, 0, 0, 0);
        this.transform.RotateAroundLocal(new Vector3(0,0,1), angle - Mathf.PI / 2);

        gunSmoke.Play();
        Debug.Log(Time.fixedTime);
        gunSmoke.startColor = new Color(Time.fixedTime%1, Time.fixedTime/2 % 1, Time.fixedTime/3 % 1);

        //shoot backward
        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.transform.position = gameObject.transform.position;
    }
}

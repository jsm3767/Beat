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
	public ParticleSystem gunSmoke2;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gunSmoke = gameObject.GetComponent<ParticleSystem>();
		GameObject gunSmoke2OBJ = GameObject.FindGameObjectWithTag ("gunsmoke2");
		gunSmoke2 = gunSmoke2OBJ.GetComponent<ParticleSystem>();
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
        Color bulletColor = new Color(Time.fixedTime % 1, Time.fixedTime / 2 % 1, Time.fixedTime / 3 % 1);

        gunSmoke.startColor = bulletColor;
        gunSmoke2.startColor = bulletColor;

        bulletPrefab.GetComponent<SpriteRenderer>().color = bulletColor;

        //shoot backward
        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.transform.position = gameObject.transform.position;
        newBullet.GetComponent<BulletMovement>().FireBullet(vectorToMouse.normalized * speed * 2);
        newBullet.GetComponent<ParticleSystem>().startColor = bulletColor;
    }

}

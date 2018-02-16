using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 4.0f;
    [SerializeField] GameObject shieldSprite;
    Vector2 mousePosition;
    
    private float moveSpeed = 5.0f;
    private Rigidbody2D rb;
    private Vector2 vectorToMouse;
    //private Collider2D 

    public GameObject bulletPrefab;
    // Use this for initialization

    public ParticleSystem gunSmoke;
	public ParticleSystem gunSmoke2;

	public bool alive = true;
    public bool isShielded = false;

	[SerializeField] GameObject TraceObj;

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

		float angle = Mathf.Atan2(vectorToMouse.y, vectorToMouse.x);
		TraceObj.transform.position = this.transform.position;
		TraceObj.transform.rotation = new Quaternion(0, 0, 0, 0);
		TraceObj.transform.RotateAroundLocal(new Vector3(0,0,1), angle - Mathf.PI / 2);

		if (alive == false) {
			Die ();
		}
    }

	public void Die(){
		alive = false;
		gunSmoke.Play ();
		TraceObj.SetActive (false);
		this.GetComponent<SpriteRenderer> ().enabled = false;
		rb.velocity = new Vector3 (0, 0, 0);
	}

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }

    public void ActivateShield()
    {
        GameObject.FindGameObjectWithTag("Shield").GetComponent<SpriteRenderer>().enabled = true;
        isShielded = true;
    }
    public void DectivateShield()
    {
        GameObject.FindGameObjectWithTag("Shield").GetComponent<SpriteRenderer>().enabled = false;
        isShielded = false;
    }

    public void Pulse()
    {
        rb.AddForce( -vectorToMouse.normalized * speed, ForceMode2D.Impulse );

        float angle = Mathf.Atan2(vectorToMouse.y, vectorToMouse.x);
        this.transform.rotation = new Quaternion(0, 0, 0, 0);
        this.transform.RotateAroundLocal(new Vector3(0,0,1), angle - Mathf.PI / 2);
        
		Color bulletColor = new Color((Time.fixedTime % 0.7f) + 0.2f, (Time.fixedTime/2 % 0.7f) + 0.2f, (Time.fixedTime/3 % 0.7f) + 0.2f);

        gunSmoke.startColor = bulletColor;
        gunSmoke2.startColor = bulletColor;

        bulletPrefab.GetComponent<SpriteRenderer>().color = bulletColor;

        //shoot backward
		if (alive) {
			gunSmoke.Play();

			GameObject newBullet = Instantiate(bulletPrefab);
			newBullet.transform.position = gameObject.transform.position;
			newBullet.GetComponent<BulletMovement>().FireBullet(vectorToMouse.normalized * speed * 2);
			newBullet.GetComponent<ParticleSystem>().startColor = bulletColor;
		}

    }

}

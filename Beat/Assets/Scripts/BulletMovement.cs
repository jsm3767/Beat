using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{

    Vector2 mousePosition;
    private Vector2 vectorToMouse;

    public Rigidbody2D bulletRidgidBody;

    [SerializeField] float bulletSpeed = 15;

    // Use this for initialization
    void Start()
    {
        bulletRidgidBody = GetComponent<Rigidbody2D>();

        Destroy(this, 10);
    }

    public void FireBullet(Vector3 direction)
    {
        bulletRidgidBody = GetComponent<Rigidbody2D>();
        bulletRidgidBody.AddForce(direction * bulletSpeed);

        float angle = Mathf.Atan2(direction.y, direction.x);
        this.transform.rotation = new Quaternion(0, 0, 0, 0);
        this.transform.RotateAroundLocal(new Vector3(0, 0, 1), angle - Mathf.PI / 2);
    }

    // Update is called once per frame
    void Update()
    {

    }


}

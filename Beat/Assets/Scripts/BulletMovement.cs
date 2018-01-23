using System.Collections;
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
        mousePosition = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
        vectorToMouse = mousePosition - (Vector2)transform.position;
        bulletRidgidBody.AddForce(vectorToMouse.normalized * 20, ForceMode2D.Impulse);
        float angle = Mathf.Atan2(-vectorToMouse.y, -vectorToMouse.x);
        this.transform.rotation = new Quaternion(0, 0, 0, 0);
        this.transform.RotateAroundLocal(new Vector3(0, 0, 1), angle + Mathf.PI / 2);

        Destroy(this, 10);
    }

    // Update is called once per frame
    void Update()
    {

    }

   
}

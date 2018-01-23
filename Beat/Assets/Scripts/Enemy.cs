using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;

    private Rigidbody2D rb;

    public GameObject playerObject;
    public GameObject PlayerObject { get { return playerObject; } set { playerObject = value; } }
    private Player playerRef;
    private Vector2 vectorToPlayer;
    public bool alive = true;
    
    bool started = false;

    //private float offset = 0.585f; //time to wait before first beat starts

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerRef = playerObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        vectorToPlayer = playerRef.GetPosition() - this.transform.position;
    }

    public void Pulse()
    {
        rb.AddForce(vectorToPlayer.normalized * moveSpeed, ForceMode2D.Impulse);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            alive = false;
            Destroy(other.gameObject);
            Debug.Log(other.gameObject.tag);
        }
            
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    [SerializeField] private string powerUpType;
    public int beatDelay;
    private GameManager gm;
    public GameManager GM { get { return gm; } set { gm = value; } }

    // Use this for initialization
    void Start () {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>(); ;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), other.GetComponent<Collider2D>());          
            return;
        }
        gm.AddPowerUp(powerUpType, beatDelay);
        Destroy(gameObject);
        return;
    }
}

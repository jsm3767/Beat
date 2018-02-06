using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    private string powerUpType;
    public int beatDelay;
    private GameManager gm;
    public GameManager GM { get { return gm; } set { gm = value; } }

    // Use this for initialization
    void Start () {
        powerUpType = gameObject.tag;
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>(); ;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        gm.AddPowerUp(powerUpType, beatDelay);
        Destroy(gameObject);
    }
}

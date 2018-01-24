using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTick : MonoBehaviour
{
    public float timer;
    private float secondsPerBeat;
    private GameManager gameManager;

    public GameObject[] ticks;
    private List<Vector3> originalPositions = new List<Vector3>();
    // Use this for initialization
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        ticks = GameObject.FindGameObjectsWithTag("Tick");
        foreach (GameObject t in ticks)
        {
            originalPositions.Add(t.GetComponent<RectTransform>().position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        secondsPerBeat = gameManager.SecondsPerBeat;
        timer = gameManager.Timer;
        
        if ( timer == 0.0f )
        {
            return;
        }
        //only allocate once and reuse
        Vector3 temp;
        foreach ( GameObject t in ticks)
        {
            if( t.GetComponent<RectTransform>().position.x < 0 )
            {
                temp = t.GetComponent<RectTransform>().localPosition;
                t.GetComponent<RectTransform>().localPosition = new Vector3(temp.x + ((timer / secondsPerBeat) * 128.0f), temp.y, temp.z);
            }
            else
            {
                temp = t.GetComponent<RectTransform>().localPosition;
                t.GetComponent<RectTransform>().localPosition = new Vector3(temp.x - ((timer / secondsPerBeat) * 128.0f), temp.y, temp.z);
            }
        }

    }

    //reset ticks to original positions
    public void Pulse()
    {
        for( int index = 0; index < ticks.Length; index++)
        {
            ticks[index].GetComponent<RectTransform>().localPosition = originalPositions[index];
        }
    }
}

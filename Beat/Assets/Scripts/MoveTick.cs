using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveTick : MonoBehaviour
{
    public float timer;
    private float secondsPerBeat;
    private GameManager gameManager;

    public GameObject tickPrefab;
    public GameObject canvas;
    public GameObject middleTick;

    public GameObject[] ticks;
    private List<Vector3> originalPositions = new List<Vector3>();

    int limit = 0;

    // Use this for initialization
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        int position = 128;
        while (position < Screen.width)
        {
            GameObject temp = Instantiate(tickPrefab, canvas.transform);
            temp.GetComponent<RectTransform>().localPosition = new Vector3(position, -Screen.height/2.0f + 64, 0);


            GameObject temp1 = Instantiate(tickPrefab, canvas.transform);
            temp1.GetComponent<RectTransform>().localPosition = new Vector3(-position, -Screen.height/2.0f + 64, 0);

            position += 128;
            if( position >= Screen.width)
            {
                limit = position - 128;
            }
        }
        
        ticks = GameObject.FindGameObjectsWithTag("Tick");
        foreach (GameObject t in ticks)
        {
            originalPositions.Add(t.GetComponent<RectTransform>().localPosition);
        }

        middleTick.transform.SetAsLastSibling();
    }

    // Update is called once per frame
    void Update()
    {
//        middleTick.GetComponent<RectTransform>().localPosition = new Vector3(0.0f, -Screen.height / 2.0f + 64, 0);
        secondsPerBeat = gameManager.SecondsPerBeat;
        timer = gameManager.Timer;
        if(timer > .1f)
        {
            middleTick.GetComponent<Image>().color = Color.white;
        }
        if (timer == 0.0f)
        {
            return;
        }
        //only allocate once and reuse
        Vector3 temp;

        for (int index = 0; index < ticks.Length; index++)
        {
            temp = originalPositions[index];

            if (originalPositions[index].x < 0)
            {
                ticks[index].GetComponent<RectTransform>().localPosition = new Vector3(temp.x + ((timer / secondsPerBeat) * 128.0f), -Screen.height / 2.0f + 64, temp.z);
            }
            else
            {
                ticks[index].GetComponent<RectTransform>().localPosition = new Vector3(temp.x - ((timer / secondsPerBeat) * 128.0f), -Screen.height / 2.0f + 64, temp.z);
            }
        }

    }

    public void EnqueuePowerUp()
    {

    }


    //reset ticks to original positions
    public void Pulse()
    {
        Vector3 temp;
        for (int index = 0; index < ticks.Length; index++)
        {
            temp = originalPositions[index];

            if (originalPositions[index].x < 0)
            {
                ticks[index].GetComponent<RectTransform>().localPosition = new Vector3(temp.x +  128.0f, -Screen.height / 2.0f + 64, temp.z);
            }
            else
            {
                ticks[index].GetComponent<RectTransform>().localPosition = new Vector3(temp.x - 128.0f, -Screen.height / 2.0f + 64, temp.z);
            }
        }
        
        int index0=-1;
        int index1=-1;

        for (int index = 0; index < ticks.Length; index++)
        {
            //10 is arbitrary just want the two closest might keep track of them later
            if (System.Math.Abs(ticks[index].GetComponent<RectTransform>().localPosition.x) < 10f)
            {
                if( index0 == -1)
                { 
                    index0 = index;
                }
                else
                {
                    index1 = index;
                }
            }
        }
        ticks[index0].GetComponent<RectTransform>().localPosition = new Vector3(limit, -Screen.height / 2.0f + 64);
        ticks[index1].GetComponent<RectTransform>().localPosition = new Vector3(-limit, -Screen.height / 2.0f + 64);
        

        for( int index = 0; index < ticks.Length; index++)
        {
            originalPositions[index] = ticks[index].GetComponent<RectTransform>().localPosition;
        }
        
        middleTick.GetComponent<Image>().color = Color.blue;
    }
}

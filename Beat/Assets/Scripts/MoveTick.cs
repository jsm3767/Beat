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
        middleTick.GetComponent<RectTransform>().localPosition = new Vector3(0.0f, -Screen.height / 2.0f + 64, 0);
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

    //reset ticks to original positions
    public void Pulse()
    {
        for (int index = 0; index < ticks.Length; index++)
        {
            ticks[index].GetComponent<RectTransform>().localPosition = new Vector3(originalPositions[index].x, -Screen.height / 2.0f + 64, 0);
        }
        middleTick.GetComponent<Image>().color = Color.blue;
    }
}

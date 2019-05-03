using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_GameEvents : MonoBehaviour
{
    public static Controller_GameEvents Instance { get; private set; }

    public enum State { INITIAL, START_EVENT1, START_EVENT2, START_EVENT3, STOP }

    public State currentState = State.INITIAL;

    [Header("COLUMN EVENT")]
    public GameObject columnPrefab;
    private GameObject[] columns= new GameObject[4];

    public GameObject spawnPositions1_player1;
    public GameObject spawnPositions1_player2;
    public GameObject spawnPositions2_player1;
    public GameObject spawnPositions2_player2;

    private Transform[] spawnTransformCollection1_p1;
    private Transform[] spawnTransformCollection2_p1;

    private Transform[] spawnTransformCollection1_p2;
    private Transform[] spawnTransformCollection2_p2;

    private Vector3 spawnPos1_p1;
    private Vector3 spawnPos2_p1;
    private Vector3 spawnPos1_p2;
    private Vector3 spawnPos2_p2;

    GameObject col1;
    GameObject col2;
    GameObject col3;
    GameObject col4;

    [Header("DISC EVENT")]
    GameObject disc;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

    }

    private void Update()
    {
        switch (currentState)
        {
            case State.INITIAL:

                break;

            case State.START_EVENT1:

                break;

            case State.START_EVENT2:

                break;
            case State.START_EVENT3:

                break;

            case State.STOP:

                break;

        }
    }
    public void startEventNumber(int event_number)
    {
        if (event_number==1)
        {
            ChangeState(State.START_EVENT1);
        }
        else if(event_number==2)
        {
            ChangeState(State.START_EVENT2);

        }
        else
        {
            ChangeState(State.START_EVENT3);

        }
    }
    public void stopEvent()
    {

        ChangeState(State.INITIAL);
    }

    private void getRandomSpawnPos(int rndNumber)
    {

        if(rndNumber == 1)
        {
            spawnTransformCollection1_p1 = spawnPositions1_player1.GetComponentsInChildren<Transform>();

            spawnPos1_p1 = spawnTransformCollection1_p1[1].position;
            spawnPos2_p1 = spawnTransformCollection1_p1[2].position;

            spawnTransformCollection1_p2 = spawnPositions1_player2.GetComponentsInChildren<Transform>();
            spawnPos1_p2 = spawnTransformCollection1_p2[1].position;
            spawnPos2_p2 = spawnTransformCollection1_p2[2].position;

        }
        else
        {
            spawnTransformCollection2_p1 = spawnPositions2_player1.GetComponentsInChildren<Transform>();
            spawnPos1_p1 = spawnTransformCollection2_p1[1].position;
            spawnPos2_p1 = spawnTransformCollection2_p1[2].position;

            spawnTransformCollection2_p2 = spawnPositions2_player2.GetComponentsInChildren<Transform>();
            spawnPos1_p2 = spawnTransformCollection2_p2[1].position;
            spawnPos2_p2 = spawnTransformCollection2_p2[2].position;
        }


    }

    public void ChangeState(State newState)
    {
        switch (currentState)
        {
            //Exit
            case State.INITIAL:

                break;

            case State.START_EVENT1:
                //destroy all event GO

                foreach (GameObject obj in columns)
                {
                    Destroy(obj);
                }
                //Destroy(col1);
                //Destroy(col2);
                //Destroy(col3);
                //Destroy(col4);
                break;

            case State.START_EVENT2:

                break;
            case State.START_EVENT3:

                break;

            case State.STOP:

                break;
        }

        switch (newState)
        {
            //Enter
            case State.INITIAL:
                break;

            case State.START_EVENT1:
                //spawn and asign all go positions
                int rnd = Random.Range(1, 3);
                getRandomSpawnPos(rnd);
                GameObject col1 = Instantiate(columnPrefab);
                col1.transform.position = spawnPos1_p1;
                GameObject col2 = Instantiate(columnPrefab);
                col2.transform.position = spawnPos2_p1;
                GameObject col3 = Instantiate(columnPrefab);
                col3.transform.position = spawnPos1_p2;
                GameObject col4 = Instantiate(columnPrefab);
                col4.transform.position = spawnPos2_p2;
                columns[0] = col1;
                columns[1] = col2;
                columns[2] = col3;
                columns[3] = col4;
                break;

            case State.START_EVENT2:

                break;
            case State.START_EVENT3:

                break;

            case State.STOP:

                break;


        }

        currentState = newState;
    }
    void spawnColumns()
    {

    }
}
using System.Collections;

using System.Collections.Generic;

using UnityEngine;


public class Controller_GameModes : MonoBehaviour
{
    // Start is called before the first frame update

    public static Controller_GameModes Instance { get; private set; }


    public enum State { INITIAL, PLAYING_NORMAL, PLAYING_EVENT }


    public State currentState = State.PLAYING_EVENT;

    [Header("Events")]
    public float eventMaxPlayTime;
    private float eventTimer;
    private bool playingEvent=false;
    private float eventStartTimer;
    public float eventWaitTimeToStart;

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

    void Start()
    {


    }

    // Update is called once per frame

    void Update()
    {
        switch(currentState)
        {
            case State.PLAYING_NORMAL:

                if (!playingEvent)
                {
                    eventStartTimer += Time.deltaTime;
                    if (eventStartTimer >= eventWaitTimeToStart)
                    {
                        ChangeState(State.PLAYING_EVENT);
                    }

                }
                
                break;

            case State.PLAYING_EVENT:
                eventTimer += Time.deltaTime;

                if (eventTimer >= eventMaxPlayTime)
                {                    
                    ChangeState(State.PLAYING_NORMAL);
                }
                break;
        }
    }
    public void ChangeState(State newState)
    {
        switch (currentState)
        {
            //Exit
            case State.PLAYING_NORMAL:
                eventStartTimer = 0;
                break;

            case State.PLAYING_EVENT:
                playingEvent = false;
                eventTimer = 0;
                Controller_GameEvents.Instance.stopEvent();//stop event call game_events instance
                //destroy all event gameobj

                break;

        }

        switch (newState)
        {
            //Enter
            case State.PLAYING_NORMAL:

                break;

            case State.PLAYING_EVENT:
                playingEvent = true;

                float rnd = Random.Range(1, 4);

                Controller_GameEvents.Instance.startEventNumber(1);//call controller_gameEvent spawnObj
                break;


        }

        currentState = newState;
    }

    public void ChangeGameMode()
    {

    }
}
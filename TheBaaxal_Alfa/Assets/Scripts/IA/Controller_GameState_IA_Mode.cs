using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_GameState_IA_Mode : MonoBehaviour
{
    // Start is called before the first frame update

    public static Controller_GameState_IA_Mode Instance { get; private set; }

    public enum State { INITIAL, PLAYING, RESTART_DISC, RESTART_SET, START_EVENT, RESET, PAUSE, GAME_OVER }

    public State currentState;
    public bool pointScoredBool;

    public float maxWaitTimeDisc;
    public float restartDiscTimer;
    private bool restartingDisc;
    private bool startDiscTimer;

    public Vector3 discStartPosition;
    private GameObject disc;
    public GameObject discPrefab;
    private DiscMovementIA discMovement;
    private Disc_MeshRotation discMeshRotation;
    private Vector3 discMoveDirection;

    public PlayerController_p1 p1;
    public IA IA;



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
        currentState = State.PLAYING;
        disc = FindObjectOfType<DiscMovementIA>().gameObject;
        discMovement = FindObjectOfType<DiscMovementIA>().GetComponent<DiscMovementIA>();
        discStartPosition = FindObjectOfType<DiscMovementIA>().gameObject.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.PLAYING:
                if (Controller_Points.Instance.initialThrow())
                    FirstBall();

                if (pointScoredBool)
                {
                    ChangeState(State.RESTART_DISC);
                }

                break;
            case State.RESTART_DISC:
                if (startDiscTimer)
                {
                    restartDiscTimer += Time.deltaTime;


                    if (restartDiscTimer >= maxWaitTimeDisc)
                    {

                        startDiscTimer = false;
                        restartDiscTimer = 0;
                        ChangeState(State.PLAYING);
                    }
                }


                break;
        }
    }


    public void pointScored(int player_number)
    {
        pointScoredBool = true;
        if (player_number == 1)
        {
            discMoveDirection = new Vector3(1, 0, 0);
        }

        else
        {
            discMoveDirection = new Vector3(-1, 0, 0);
        }


        Debug.Log("pointscored for player" + player_number);
    }

    private IEnumerator RestartDisc(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);

    }
    int decideSign()
    {
        if (Random.Range(0, 2) == 1)
        {
            return 1;
        }
        return -1;
    }

    private void FirstBall()
    {

        restartDiscTimer += Time.deltaTime;

        if (restartDiscTimer >= maxWaitTimeDisc)
        {
            discMoveDirection = new Vector3(decideSign(), 0, 0);
            discMovement.direction = discMoveDirection;
            discMovement.holdDisc = false;
            restartDiscTimer = 0;

        }
    }

    void ChangeState(State newState)
    {
        switch (currentState)
        {
            //Exit
            case State.PLAYING:
                pointScoredBool = false;
                break;
            case State.RESTART_DISC:
                discMovement.holdDisc = false;
                discMeshRotation.canRotate = true;

                break;
        }

        switch (newState)
        {
            //Enter
            case State.PLAYING:
                p1.playerCC.detectCollisions = true;

                break;
            case State.RESTART_DISC:
                startDiscTimer = true;
                Destroy(discMovement.gameObject);
                GameObject newDisc = Instantiate(discPrefab);
                newDisc.transform.position = discStartPosition;
                discMovement = newDisc.GetComponent<DiscMovementIA>();
                discMeshRotation = newDisc.GetComponentInChildren<Disc_MeshRotation>();
                discMovement.holdDisc = true;
                discMeshRotation.canRotate = false;
                discMovement.direction = discMoveDirection;
                p1.canMove = true;
                p1.playerCC.detectCollisions = false;
                IA.discGameobject = newDisc;
                break;

        }

        currentState = newState;
    }


}

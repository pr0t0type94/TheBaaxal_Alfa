using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_GameState : MonoBehaviour
{
    // Start is called before the first frame update

    public static Controller_GameState Instance { get; private set; }

    public enum State { INITIAL,PLAYING, RESTART_DISC, RESTART_SET, START_EVENT, RESET, PAUSE, GAME_OVER }

    public State currentState;

    public bool pointScoredBool;

    [Header("Restart Disc")]
    public float maxWaitTimeDisc;
    public float restartDiscTimer;
    private bool restartingDisc;
    private bool startDiscTimer;

    public Vector3 discStartPosition;
    private GameObject disc;
    public GameObject discPrefab;
    private Disc_Movement discMovement;
    private Disc_MeshRotation discMeshRotation;
    private Vector3 discMoveDirection;

    public PlayerController_p1 p1;
    public PlayerController_p2 p2;

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
        disc = FindObjectOfType<Disc_Movement>().gameObject;
        discMovement = FindObjectOfType<Disc_Movement>().GetComponent<Disc_Movement>();
        discStartPosition = FindObjectOfType<Disc_Movement>().gameObject.transform.position;

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

            case State.START_EVENT:
                ChangeState(State.PLAYING);
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

        //Debug.Log("pointscored for player" + player_number);
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

        if (restartDiscTimer >= (maxWaitTimeDisc*2))
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
            case State.START_EVENT:

                break;
        }

        switch (newState)
        {
            //Enter
            case State.PLAYING:
                p1.playerCC.detectCollisions = true;
                p2.playerCC.detectCollisions = true;
                break;
            case State.RESTART_DISC:
                startDiscTimer = true;
                Destroy(discMovement.gameObject);
                GameObject newDisc = Instantiate(discPrefab);
                newDisc.transform.position = discStartPosition;
                discMovement = newDisc.GetComponent<Disc_Movement>();
                discMeshRotation = newDisc.GetComponentInChildren<Disc_MeshRotation>();
                discMovement.holdDisc = true;
                discMeshRotation.canRotate = false;
                discMovement.direction = discMoveDirection;
                p1.canMove = true;
                p2.canMove = true;
                p1.playerCC.detectCollisions = false;
                p2.playerCC.detectCollisions = false;

                break;

            case State.START_EVENT:
                break;
        }

        currentState = newState;
    }


}

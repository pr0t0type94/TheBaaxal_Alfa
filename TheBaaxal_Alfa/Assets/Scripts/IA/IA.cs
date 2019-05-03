using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA : MonoBehaviour
{

    public enum State { INITIAL, MOVE_TO_DISC, REFLECT_DISC, RESET_POS, WAITING };
    public State currentState = State.INITIAL;
    public Rigidbody discRB;
    public Rigidbody gameObjectRB;
    public GameObject discGameobject;
    public GameObject surrogateTarget;
    public float maxPredictionTime = 3f;
    public bool canReturnDisc = false;
    public bool goRed;
    public bool shieldActivated = false;
    private bool startShieldTimer = false;
    public float shieldTimer = 1f;
    public Disc_Rotator rotator2;
    public Vector3 respawnPosition;
    private int randomInt;
    public DiscMovementIA discController;
    public DiscMovementIA discMovement;
    public Disc_MeshRotation discRotation;


    void Start()
    {
        respawnPosition = gameObject.transform.position;
    }



    void Update()
    {
        Debug.Log(DistanceToTarget(gameObject, discGameobject));
        switch (currentState)
        {
            case State.INITIAL:
                ChangeState(State.WAITING);
                break;


            case State.WAITING:
                if (DistanceToTarget(gameObject, discGameobject) <= 15)
                {
                    ChangeState(State.MOVE_TO_DISC);
                }
                break;

            case State.MOVE_TO_DISC:

                Vector3 newPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, discGameobject.transform.position.z);

                gameObject.transform.position = newPos;

                if (DistanceToTarget(gameObject, discGameobject) <= 8)
                {
                    ChangeState(State.REFLECT_DISC);
                }


                break;

            case State.REFLECT_DISC:

                if (DistanceToTarget(gameObject, discGameobject) >= 20)
                {
                    ChangeState(State.WAITING);
                }

                randomInt = Random.Range(0, 5);

                if (randomInt == 1)
                {
                    startShieldTimer = true;
                }

                startShieldTimer = true;
                if (startShieldTimer)
                {
                    shieldActivated = true;

                    shieldTimer -= Time.deltaTime;

                    if (shieldTimer <= 0)
                    {
                        startShieldTimer = false;
                        shieldActivated = false;
                        shieldTimer = 1f;
                    }
                }

                if (shieldActivated)
                {
                    gameObject.GetComponentInChildren<Renderer>().material.color = Color.blue;
                    canReturnDisc = true;
                    
                }
                else
                {
                    
                    if (!goRed)
                        gameObject.GetComponentInChildren<Renderer>().material.color = Color.white;
                    else
                        gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;

                    canReturnDisc = false;

                   

                }
                
             
                break;


        }
    }

    void ChangeState(State newState)
    {
        switch (currentState)
        {
            //Exit
            case State.REFLECT_DISC:

                shieldActivated = false;
                shieldTimer = 0.0f;
                break;
        }

        switch (newState)
        {
            //Enter
            case State.REFLECT_DISC:
                shieldTimer = 1f;
                break;
 

        }

        currentState = newState;
    }


    public static float DistanceToTarget(GameObject me, GameObject target)
    {
        return (target.transform.position - me.transform.position).magnitude;
    }

}


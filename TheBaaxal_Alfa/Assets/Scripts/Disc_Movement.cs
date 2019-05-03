using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Disc_Movement : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Speed")]

    public float currentXSpeed;
    public float acceleration;
    public Vector3 direction;
    public float xMaxSpeed;
    public float xMinSpeed;

    public bool holdDisc;

    public Disc_Rotator rotator;
    public Disc_Rotator2 rotator2;

    [Header("Acceleration")]

    public float accelerationTimer = 0.25f;
    public float accelerationMultiplier = 1.05f;
    bool doAcceleration = false;
    public float decelerationTimer = 0.60f;
    public float decelerationMultiplier = 1.05f;
    bool doDeceleration = false;

    [Header("Disc")]
    public TrailRenderer discTrail;
    public Disc_MeshRotation discRotation;

    /// <summary>
    /// perfect block
    /// </summary>
    /// 
    [Header("Perfect block")]
    public bool startPerfectBlockTimer;
    private float perfectBlockTime;
    public float perfectBlockMaxTime;

    bool canPerfectBlock = false;
 
    void Start()
    {

        currentXSpeed = xMinSpeed;
        //direction = new Vector3(decideSign(),0,0);
        discTrail.enabled = true;
        rotator = FindObjectOfType<Disc_Rotator>();
        rotator2 = FindObjectOfType<Disc_Rotator2>();

    }


    // Update is called once per frame
    void Update()
    {
        //if (Controller_Points.Instance.initialThrow())
        //    FirstBall();

        if (!holdDisc)
            moveDisc();
        
        if (doAcceleration)
            accelerateDisc();

        if(doDeceleration)
            decelerateDisc();

        if (startPerfectBlockTimer)
            perfectBlockFunction();

        if(canPerfectBlock)
        {
            //show perfect block message

        }
    }



    void updateSpeed()
    {
        if (currentXSpeed < xMinSpeed)
            currentXSpeed = xMinSpeed;

        if (currentXSpeed > xMaxSpeed)
            currentXSpeed = xMaxSpeed;

    }

    public void moveDisc()
    {
        updateSpeed();
        transform.position += currentXSpeed * direction * Time.deltaTime;
        
    }
    void accelerateDisc()
    {      
        currentXSpeed *= 1.05f;
        accelerationTimer -= Time.deltaTime;
        if(accelerationTimer <= 0)
        {
            doDeceleration = true;
            accelerationTimer = .25f;
            doAcceleration = false;
        }  
        
    }
    void decelerateDisc()
    {
        currentXSpeed /= 1.03f;
        decelerationTimer -= Time.deltaTime;
        if (decelerationTimer <= 0)
        {
            decelerationTimer = .60f;
            doDeceleration = false;
        }

    }

    public static Vector3 Reflect(Vector3 vector, Vector3 normal)
    {
        return vector - 2 * Vector3.Dot(vector, normal) * normal;
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.name == "Player1")
            {
                PlayerController_p1 pc = collision.gameObject.GetComponent<PlayerController_p1>();
                if(pc.shieldActivated && canPerfectBlock)
                {
                    Debug.Log("perfect block done");

                }
                else
                {
                    Debug.Log("normal block done");

                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            if (collision.gameObject.name == "Player1")
            {
                PlayerController_p1 pc = collision.gameObject.GetComponent<PlayerController_p1>();

                if (pc != null && pc.shieldActivated) //&&perfect block timer between x and y)
                {
                    pc.canMove = false;
                    ContactPoint con = collision.contacts[0];
                    direction = Reflect(direction, rotator.returnCurrentDirection());
                    pc.goRed = false;                    
                    doAcceleration=true;

                }
                else if (pc != null && !pc.shieldActivated && !pc.DashingBool)
                {
                    pc.canMove = false;
                    pc.goRed = true;
                    startPerfectBlockTimer = true;
                    holdDisc = true;
                    if(canPerfectBlock)
                    {
                        //Debug.Log("perfect block done");
                    }
                    else
                    {
                     //   Debug.Log("normal block done");

                    }

                }
                else if (pc!=null && pc.DashingBool)
                {
                    pc.canMove = false;
                    ContactPoint con = collision.contacts[0];
                    Vector3 newDirection = new Vector3(pc.l_Movement.x,0,0);
                    if(newDirection.x==0)
                    {
                        newDirection.x = 1;
                    }
                    newDirection.Normalize();
                    direction = Reflect(direction, newDirection);

                }
            }

            if (collision.gameObject.name == "Player2")
            {
                PlayerController_p2 pc2 = collision.gameObject.GetComponent<PlayerController_p2>();

                if (pc2 != null && pc2.shieldActivated)
                {
                    ContactPoint con = collision.contacts[0];
                    direction = Reflect(direction, rotator2.returnCurrentDirection());
                    pc2.goRed = false;
                    doAcceleration = true;
                }
                else if (pc2 != null && !pc2.shieldActivated && !pc2.DashingBool)
                {
                    pc2.goRed = true;
                    pointScored(1);

                    //score to player 1
                }
                else if (pc2 != null && pc2.DashingBool)
                {
                    pc2.canMove = false;
                    ContactPoint con = collision.contacts[0];
                    Vector3 newDirection = new Vector3(pc2.l_Movement.x, 0, 0);
                    if (newDirection.x == 0)
                    {
                        newDirection.x = -1;
                    }
                    newDirection.Normalize();
                    direction = Reflect(direction, newDirection);

                }
            }
        }

        if (collision.gameObject.tag == "Wall")
        {
            ContactPoint con = collision.contacts[0];
            direction = Reflect(direction, con.normal);
        }

        if (collision.gameObject.tag == "Goal2")
        {
            pointScored(1);
        }
        if (collision.gameObject.tag == "Goal")
        {
            pointScored(2);
        }
    }

    void pointScored(int player_number)
    {
        Controller_Points.Instance.pointScored(player_number);
        Controller_GameState.Instance.pointScored(player_number);
    }


    void perfectBlockFunction()
    {
        perfectBlockTime += Time.deltaTime;

        if(perfectBlockTime>=perfectBlockMaxTime)
        {
            holdDisc = false;
            startPerfectBlockTimer = false;
            perfectBlockTime = 0;
            pointScored(2);

        }

        if (perfectBlockTime >= 4.5f)
        {
            canPerfectBlock= false;

        }


        else if (perfectBlockTime <= 4.5f && perfectBlockTime > 0.001f)

        {
            canPerfectBlock = true;
        }

        //Debug.Log(perfectBlockTime);
        //Debug.Log(hasPerfectBlock);

    }


}
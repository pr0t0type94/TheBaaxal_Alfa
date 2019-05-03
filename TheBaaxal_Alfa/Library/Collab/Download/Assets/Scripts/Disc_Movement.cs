using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Disc_Movement : MonoBehaviour
{
    // Start is called before the first frame update
    public float currentXSpeed;
    public float acceleration;
    public Vector3 direction;
    public float xMaxSpeed;
    public float xMinSpeed;

    
    public bool pointScored;
    public bool pointScoredForPlayerOne;
    public bool pointScoredForPlayerTwo;

    public float cooldownThrow=0;
    public float maxCooldownThrow=2;
    private bool startCounter;
    private Vector3 respawnPos;


    public bool canMoveDisc;

    private Vector3 throwDir;

    public Disc_Rotator rotator;
    public Disc_Rotator2 rotator2;

    float accelerationTimer = 0.5f;
    bool doAcceleration = false;

    public TrailRenderer discTrail;
    public Disc_MeshRotation discRotation;

    void Start()
    {
        canMoveDisc = true;
        currentXSpeed = xMinSpeed;
        respawnPos = gameObject.transform.position;

        direction = new Vector3(decideSign(),0,0);
        discTrail.enabled = true;
    }

 
    // Update is called once per frame
    void Update()
    {
        if (Controller_Points.Instance.initialThrow())
            FirstBall();
        if (!canMoveDisc)
            moveDisc();
        if(pointScored)
        {
            pointScoredFunction();

        }

        if(doAcceleration)
        {
            accelerateDisc();
        }
    }
    int decideSign()
    {
        if (Random.Range(0, 2) == 0)
        {
            return -1;
        }
        return 1;
    }

    float updateSpeed()
    {
        if (currentXSpeed < xMinSpeed)
            currentXSpeed = xMinSpeed;

        return currentXSpeed += acceleration * Time.deltaTime;
    }

    public void moveDisc()
    {
        if (currentXSpeed < xMaxSpeed)
            updateSpeed();


        transform.position += currentXSpeed * direction * Time.deltaTime;
        
    }
    void accelerateDisc()
    {
        if(doAcceleration)
        {
            Debug.Log("accelerate disc");
            currentXSpeed *= 1.05f;
            accelerationTimer -= Time.deltaTime;
            if(accelerationTimer <= 0)
            {
                doAcceleration = false;
                accelerationTimer = .2f;
                currentXSpeed /= 1.05f;
            }
        }
      
    }

    public static Vector3 Reflect(Vector3 vector, Vector3 normal)
    {
        return vector - 2 * Vector3.Dot(vector, normal) * normal;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.name == "Player1")
            {
                PlayerController_p1 pc = collision.gameObject.GetComponent<PlayerController_p1>();

                if (pc != null && pc.shieldActivated)
                {
                    ContactPoint con = collision.contacts[0];
                    direction = Reflect(direction, rotator.returnCurrentDirection());
                    pc.goRed = false;
                   // doAcceleration=true;
                }
                else if (pc != null && !pc.shieldActivated)
                {
                    pc.goRed = true;
                    sumScore(2);
                    pointScored = true;
                    pointScoredForPlayerTwo = true;
                    pointScoredForPlayerOne = false;


                }
            }

            if (collision.gameObject.name == "Player2")
            {
                PlayerController_p2 pc2 = collision.gameObject.GetComponent<PlayerController_p2>();

                if (pc2 != null && pc2.canReturnDisc)
                {
                    ContactPoint con = collision.contacts[0];
                    direction = Reflect(direction, rotator2.returnCurrentDirection());
                    pc2.goRed = false;
                    //doAcceleration = true;
                }
                else if (pc2 != null && !pc2.canReturnDisc)
                {
                    pc2.goRed = true;
                    sumScore(1);
                    pointScored = true;
                    pointScoredForPlayerOne = true;
                    pointScoredForPlayerTwo = false;
     

                    //score to player 1
                }
            }
        }

        if (collision.gameObject.tag == "Wall")
        {
            ContactPoint con = collision.contacts[0];
            direction = Reflect(direction, con.normal);
            //doAcceleration = true;
        }

        if (collision.gameObject.tag == "Goal2")
        {
            Debug.Log("wall 2");
            sumScore(1);
            pointScored = true;
            pointScoredForPlayerOne = true;
            pointScoredForPlayerTwo = false;
        }
        if (collision.gameObject.tag == "Goal")
        {
            Debug.Log("wall 1");

            sumScore(2);
            pointScored = true;
            pointScoredForPlayerTwo = true;
            pointScoredForPlayerOne = false;

        }
    }

    void sumScore(int player_number)
    {
        Controller_Points.Instance.addScore(player_number);
    }
   

    private void FirstBall()
    {
               
            cooldownThrow += Time.deltaTime;

            if( cooldownThrow >= maxCooldownThrow)
            {
            canMoveDisc = false;
                cooldownThrow = 0;
            }

       
    }
    


    private void pointScoredFunction()
    {
        gameObject.transform.position = respawnPos;
        discRotation.canRotate = false;
        canMoveDisc = true;
        currentXSpeed = 0;
        discTrail.enabled = false;
        if (pointScoredForPlayerOne)//disc for player two
        {
            direction = new Vector3(1, 0, 0);
        }
        else
        {
            direction = new Vector3(-1, 0, 0);
            pointScoredForPlayerTwo = false;

        }

        startCounter = true;

        if (startCounter == true)
        {

            cooldownThrow += Time.deltaTime;

            if (cooldownThrow >= maxCooldownThrow)
            {
                pointScored = false;
                canMoveDisc = false;
                startCounter = false;
                cooldownThrow = 0;
                currentXSpeed = xMinSpeed;
                discTrail.enabled = true;
                discRotation.canRotate = true;

            }
        }      
    }
    private void StartersThrows()
    {

            cooldownThrow = 0;
            startCounter = true;
            pointScored = false;
        
        if (startCounter == true)
        {
            
            cooldownThrow += Time.deltaTime;

            if (cooldownThrow >= maxCooldownThrow)
            {
                canMoveDisc = false;
                startCounter = false;
                currentXSpeed = xMinSpeed;

            }
        }
    }






}
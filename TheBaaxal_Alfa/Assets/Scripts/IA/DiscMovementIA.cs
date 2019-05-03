using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscMovementIA : MonoBehaviour
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


    void Start()
    {

        currentXSpeed = xMinSpeed;
        //direction = new Vector3(decideSign(),0,0);
        discTrail.enabled = true;
        rotator = FindObjectOfType<Disc_Rotator>();

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

        if (doDeceleration)
            decelerateDisc();

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
        if (accelerationTimer <= 0)
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.name == "Player1")
            {
                PlayerController_p1 pc = collision.gameObject.GetComponent<PlayerController_p1>();

                if (pc != null && pc.shieldActivated)
                {
                    pc.canMove = false;
                    ContactPoint con = collision.contacts[0];
                    direction = Reflect(direction, rotator.returnCurrentDirection());
                    pc.goRed = false;
                    doAcceleration = true;
                }
                else if (pc != null && !pc.shieldActivated && !pc.DashingBool)
                {
                    pc.canMove = false;
                    pc.goRed = true;
                    pointScored(2);

                }
                else if (pc != null && pc.DashingBool)
                {
                    pc.canMove = false;
                    ContactPoint con = collision.contacts[0];
                    Vector3 newDirection = new Vector3(pc.l_Movement.x, 0, 0);
                    if (newDirection.x == 0)
                    {
                        newDirection.x = 1;
                    }
                    newDirection.Normalize();
                    direction = Reflect(direction, newDirection);

                }
            }

            if (collision.gameObject.name == "IA")
            {
                IA pc2 = collision.gameObject.GetComponent<IA>();

                if (pc2 != null && pc2.canReturnDisc)
                {
                    ContactPoint con = collision.contacts[0];
                    direction = Reflect(direction, rotator.returnCurrentDirection());
                    pc2.goRed = false;
                    doAcceleration = true;
                }
                else if (pc2 != null && !pc2.canReturnDisc)
                {
                    pc2.goRed = true;
                    pointScored(1);

                    //score to player 1
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
        Controller_GameState_IA_Mode.Instance.pointScored(player_number);
    }

}
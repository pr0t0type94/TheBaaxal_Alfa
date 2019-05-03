using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController_p2 : MonoBehaviour
{
    [HideInInspector]
    public CharacterController playerCC;

    [Header("Speed")]

    public float l_Speed;
    public float m_RunSpeed;
    private float iniSpeed;
    private float verticalSpeed;
    public bool OnGround;
    public Vector3 l_Movement;


    //For DASH
    [Header("Dash")]

    public Vector3 moveDirection;
    public float maxDashTime;
    public float dashSpeed;
    private float currentDashTime;
    public float dashReloadmaxTime;
    private float currentDashReloadTime;
    public bool DashingBool = false;
    private bool DashReloadBool = false;
    private bool dashCooldownBool;
    public float dashCooldownTime;
    private float currentDashCooldown;
    //

    //public GameController gameControler;

    [Header("Bools")]

    public bool canMove;
    public bool canAttack;
    public bool goRed;
    public bool shieldActivated = false;
    private bool startShieldTimer = false;
    private bool hasDisc;
    public bool canActivateShield = true;

    CollisionFlags l_CollisionFlags;

    [Header("Restart")]

    //public RestartGame resetController;

    public Vector3 respawnPosition;

    Renderer rend;

    [Header("Disc")]

    public float shieldTimer = 1f;

    public float moveTimer = .1f;
    public bool startMoveTimer = false;

    [Header("HitTimer")]

    public float hitTimer = 0.2f;
    public bool hitTimerActive = false;

    public string[] joyarray;
    bool joystickConnected = false;

    // Use this for initialization
    void Start()
    {
        playerCC = gameObject.GetComponent<CharacterController>();
        // discController = gameObject.GetComponent<Disc_Movement>();
        iniSpeed = l_Speed;
        canMove = true;
        respawnPosition = gameObject.transform.position;
        joyarray = Input.GetJoystickNames();

    }

    // Update is called once per frame
    void Update()
    {

        transform.forward = Vector3.right;


        if (joyarray.Length > 0)
        {
            joystickConnected = true;
            gameObject.GetComponentInChildren<Disc_Rotator2>().joyConnected = true;

        }
        else
        {
            joystickConnected = false;
            gameObject.GetComponentInChildren<Disc_Rotator2>().joyConnected = false;


        }

        if (joystickConnected)
        {

            float hor = Input.GetAxis("Horizontal1");

            float ver = Input.GetAxis("Vertical1");

            l_Movement = new Vector3(ver, 0, hor);

            l_Movement = Vector3.ClampMagnitude(l_Movement, 1) * l_Speed * Time.deltaTime;

        }
        else
        {
            float hor = Input.GetAxis("Horizontal2");

            float ver = Input.GetAxis("Vertical2");
            l_Movement = new Vector3(ver, 0, hor);

            l_Movement = Vector3.ClampMagnitude(l_Movement, 1) * l_Speed * Time.deltaTime;
        }

        if (Input.GetButton("Block1") && canActivateShield)
            startShieldTimer = true;

        if (startShieldTimer)
        {
            shieldFunction();
        }

        if (shieldActivated)
        {
            gameObject.GetComponentInChildren<Renderer>().material.color = Color.blue;
        }
        else
        {
            if (!goRed)
                gameObject.GetComponentInChildren<Renderer>().material.color = Color.white;
            else
                gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
        }

        //DASH
        performDash();

        //MOVEMENT

        l_Movement.Normalize();

        l_Movement *= Time.deltaTime * l_Speed;


        //GRAVITY
        if (OnGround && verticalSpeed <= 0)
        {
            verticalSpeed = -playerCC.stepOffset / Time.deltaTime;
        }
        else
        {
            verticalSpeed += Physics.gravity.y * Time.deltaTime;
        }


        l_Movement.y = verticalSpeed * Time.deltaTime;

        //CollisionFlags + controller Move
        if (canMove)
        {
            l_CollisionFlags = playerCC.Move(l_Movement);

        }
        else
        {

            moveTimer -= Time.deltaTime;
            if (moveTimer <= 0)
            {
                canMove = true;
                moveTimer = .2f;
            }
        }

        if ((l_CollisionFlags & CollisionFlags.Below) != 0)
        {
            OnGround = true;
            verticalSpeed = 0.0f;
        }
        else
        {
            OnGround = false;

        }
    }

    void hitFunction()
    {

    }

    void shieldFunction()
    {
        shieldActivated = true;

        shieldTimer -= Time.deltaTime;

        if (shieldTimer <= 0)
        {
            startShieldTimer = false;
            shieldActivated = false;
            shieldTimer = .5f;
        }
    }

    void performDash()
    {
        if (Input.GetButtonDown("Dash1") && dashCooldownBool == false && !shieldActivated)
        {
            currentDashTime = 0.0f;
            currentDashCooldown = 0.0f;

            DashingBool = true;

            dashCooldownBool = true;
        }


        if (dashCooldownBool)
        {
            currentDashCooldown += Time.deltaTime;
            if (currentDashCooldown >= dashCooldownTime)
            {
                dashCooldownBool = false;
            }
        }


        if (DashingBool)
        {
            canActivateShield = false;
            currentDashTime += Time.deltaTime;
            gameObject.GetComponentInChildren<Renderer>().material.color = Color.green;
            l_Speed = dashSpeed;

            if (currentDashTime >= maxDashTime)
            {
                DashingBool = false;
            }

        }

        if (DashingBool == false)
        {
            canActivateShield = true;
            l_Speed = iniSpeed;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Disc")
        {
            canMove = false;
            Debug.Log("hitdisc");
        }
        else
            canMove = true;
    }

}//end of class




using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController_p1 : MonoBehaviour
{
    private CharacterController playerCC;
    public Camera cam;

    [Header("Speed")]

    public float l_Speed;
    public float m_RunSpeed;
    private float iniSpeed;
    private float verticalSpeed;
    public bool OnGround;
    private Vector3 l_Movement;


    //For DASH
    [Header("Dash")]

    public Vector3 moveDirection;
    public float maxDashTime;
    public float dashSpeed;
    private float currentDashTime;
    public float dashReloadmaxTime;
    private float currentDashReloadTime;
    private bool DashingBool = false;
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
    public bool Player1HasDisc;
    public bool Player2HasDisc;
    public bool shieldActivated=false;
    private bool startShieldTimer = false;
    public bool canReturnDisc = false;
    private bool hasDisc;

    CollisionFlags l_CollisionFlags;

    [Header("Attaching")]
    public GameObject m_AttachingPosition;
    public bool m_AttachedObject;
    private Rigidbody m_ObjectAttached;
    public float m_AttachingObjectSpeed;
    private Quaternion m_AttachingObjectStartRotation;
    private bool m_AttachingObject;
    public float impulseForce = 50f;

    [Header("Restart")]

    //public RestartGame resetController;

    public Vector3 respawnPosition;
    

    Renderer rend;




    [Header("Disc")]

    public float shieldTimer = 1f;


    public Disc_Movement discController;
    public Disc_Movement discMovement;
    public Disc_Rotator rotator;
    public Disc_MeshRotation discRotation;


    // Use this for initialization
    void Start()
    { 
        playerCC = gameObject.GetComponent<CharacterController>();
       // discController = gameObject.GetComponent<Disc_Movement>();
        iniSpeed = l_Speed;
        canMove = true;

        respawnPosition = gameObject.transform.position;

        
    
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.forward = Vector3.right;

        float hor = Input.GetAxis("Horizontal0");
        float ver = Input.GetAxis("Vertical0");
        l_Movement = new Vector3(ver, 0, -hor);
        l_Movement = Vector3.ClampMagnitude(l_Movement, 1) * l_Speed * Time.deltaTime;

        if (Input.GetButton("Block0"))
            startShieldTimer = true;

        if(startShieldTimer)
        {
            shieldActivated = true;

            shieldTimer -= Time.deltaTime;

            if(shieldTimer <=0)
            {
                startShieldTimer = false;
                shieldActivated = false;
                shieldTimer = .5f;
            }
        }

        if (shieldActivated)
        {
            gameObject.GetComponentInChildren<Renderer>().material.color = Color.blue;
            canReturnDisc = true;
            rotator.canRotate = true;
        }
        else
        {
            rotator.canRotate = false;

            if (!goRed)
            gameObject.GetComponentInChildren<Renderer>().material.color = Color.white;
            else
            gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;

            canReturnDisc = false;

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


    void performDash()
    {
            if (Input.GetButtonDown("Dash0") && dashCooldownBool == false && !shieldActivated)
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
            currentDashTime += Time.deltaTime;
            gameObject.GetComponentInChildren<Renderer>().material.color = Color.green;

            //rend.material.SetColor("_Color", Color.green);
            l_Speed = dashSpeed;

            if (currentDashTime >= maxDashTime)
            {
                DashingBool = false;
            }

        }

        if (DashingBool == false)
        {
            l_Speed = iniSpeed;
        }



        //DASH RELOAD

        if (discController.pointScored)
        {         
            pointScored();
        }


        //StartThrow();

    }

    public void attachDisc(GameObject goToAttach)
    {
        discController.gameObject.transform.position = goToAttach.transform.position;
        discController.gameObject.transform.parent = goToAttach.transform;
    }

    void pointScored()
    {                
          //gameObject.transform.position = respawnPosition;          
          
         
    }

   /* void StartThrow()
    {
        if (discController.pointScoredForPlayerOne)
        {
            if (Player1HasDisc)
            {
                attachDisc(m_AttachingPosition);
                discMovement.hasDisc = true;
                discRotation.canRotate = false;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                discController.transform.parent = null;
                Player1HasDisc = false;
                discController.pointScoredForPlayerOne = false;
                discMovement.hasDisc = false;
                discRotation.canRotate = true;
                discController.direction = rotator.gameObject.transform.forward;
            }

        }
    }*/

}//end of class




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc_Rotator : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject orientator;

    public Vector3 directionToPush;

    private float minAngle = -25;
    private float maxAngle = 25;

    public float rotSpeed;

    private Vector3 currentRot;

    private Vector3 initialRotation;

    /// <summary>
    /// /5
    /// </summary>
    /// 
    public float Rotation_Speed;
    public float Rotation_Friction; //The smaller the value, the more Friction there is. [Keep this at 1 unless you know what you are doing].
    public float Rotation_Smoothness; //Believe it or not, adjusting this before anything else is the best way to go.

    private float Resulting_Value_from_Input;
    private Quaternion Quaternion_Rotate_From;
    private Quaternion Quaternion_Rotate_To;
    /// <summary>
    /// /
    /// </summary>
    /// 
    public bool joyConnected = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //////////1
        //if(Input.GetAxis("Horizontal") > 0)
        //{
        //    transform.Rotate(transform.up * rotSpeed * Time.deltaTime);

        //}
        //else if(Input.GetAxis("Horizontal") < 0)
        //{
        //    transform.Rotate(transform.up * -rotSpeed * Time.deltaTime);

        //}

        /////////////2/////////DEFINITIVE

        //float hor = Input.GetAxis("AnalogX0");
        //float ver = Input.GetAxis("AnalogY0");
        //Vector3 rotation = new Vector3(ver, 0, -hor);
        //Quaternion newRotation = Quaternion.LookRotation(new Vector3(Mathf.Clamp(rotation.x,minAngle,maxAngle), 0, Mathf.Clamp(rotation.z, minAngle, maxAngle))) ;
        //////Debug.Log(rotation);
        ////Debug.Log("currentQuat "+currentQuaternion);

        //if (transform.rotation.y <= .57f && hor <0)
        //{
        //    Quaternion currentQuaternion = transform.rotation;
        //    float quaternionWvalue = currentQuaternion.w;
        //    transform.rotation = new Quaternion(0, .57f, 0, quaternionWvalue);
        //}
        //else if (transform.rotation.y >= .85f && hor>0f)
        //{
        //    Quaternion currentQuaternion = transform.rotation;
        //    float quaternionWvalue = currentQuaternion.w;
        //    transform.rotation = new Quaternion(0, .85f, 0, quaternionWvalue);

        //}
        //else
        //    transform.rotation = newRotation;
        /////////++++??
        //////float clampedRot = Mathf.Clamp(newRotation.y, minAngle, maxAngle);
        //////transform.rotation = new Quaternion(0, clampedRot, 0, 0);
        ///if
        ///
        if(joyConnected)
        {
        float hor = Input.GetAxis("AnalogX0");
        float ver = Input.GetAxis("AnalogY0");
        Vector3 rotation = new Vector3(ver, 0, -hor);
        Quaternion newRotation = Quaternion.LookRotation(new Vector3(rotation.x, 0, rotation.z));
        transform.rotation = newRotation;

        }
        else
        {
            if(Input.GetAxis("Rotator") > 0)
            {
                transform.Rotate(transform.up * rotSpeed * Time.deltaTime);

            }
            else if(Input.GetAxis("Rotator") < 0)
            {
                transform.Rotate(transform.up * -rotSpeed * Time.deltaTime);

            }
        }

        ////////3
        //Vector3 direction = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));
        //transform.rotation = Quaternion.LookRotation(direction, Vector3.up);


        ///////4
        //if (canRotate)
        //{
        //    currentRot.y += Input.GetAxis("Mouse X") * rotSpeed;
        //    currentRot.y = Mathf.Clamp(currentRot.y, minAngle, maxAngle);
        //    transform.localEulerAngles = new Vector3(0, currentRot.y, 0);

        //}
        //else
        //{
        //    transform.localEulerAngles = initialRotation;
        //}


        /////////////////5
        ///
        //Resulting_Value_from_Input += Input.GetAxis("AnalogX0") * Rotation_Speed * Rotation_Friction; //You can also use "Mouse X"
        //Quaternion_Rotate_From = transform.rotation;
        //Quaternion_Rotate_To = Quaternion.Euler(0, Resulting_Value_from_Input, 0);

        //transform.rotation = Quaternion.Lerp(Quaternion_Rotate_From, Quaternion_Rotate_To, Time.deltaTime * Rotation_Smoothness);

        //transform.rotation = Mathf.Clamp(transform.rotation, -10, 10);
    }
    float clampRot()
    {
        if (transform.rotation.y <= -25f)
        {
            return -25;
        }
        else if (transform.rotation.y >= 25f)
        {
            return 25;

        }
        else
            return transform.rotation.y;
        
    }


    public Vector3 returnCurrentDirection()
    {
        return transform.forward;
    }
    
}

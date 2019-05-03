using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc_Rotator2 : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotSpeed=10f;
    public bool joyConnected;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //float hor = Input.GetAxis("AnalogX1");
        //float ver = Input.GetAxis("AnalogY1");
        //Vector3 rotation = new Vector3(ver, 0, hor);
        //Quaternion newRotation = Quaternion.LookRotation(new Vector3(rotation.x, 0,rotation.z));
        //transform.rotation = newRotation;
        if (joyConnected)
        {
            float hor = Input.GetAxis("AnalogX1");
            float ver = Input.GetAxis("AnalogY1");
            Vector3 rotation = new Vector3(ver, 0, hor);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(rotation.x, 0, rotation.z));
            transform.rotation = newRotation;

        }
        else
        {
            if (Input.GetAxis("Rotator2") > 0)
            {
                transform.Rotate(transform.up * rotSpeed * Time.deltaTime);

            }
            else if (Input.GetAxis("Rotator2") < 0)
            {
                transform.Rotate(transform.up * -rotSpeed * Time.deltaTime);

            }
        }

    }

    public Vector3 returnCurrentDirection()
    {
        return transform.forward;
    }
}

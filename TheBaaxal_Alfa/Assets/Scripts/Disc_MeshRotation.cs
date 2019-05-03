using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc_MeshRotation : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotSpeed = 5;
    private Vector3 currentRot;
    public bool canRotate;
    void Start()
    {
        canRotate = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(canRotate)
        {
            currentRot.y +=  rotSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);

        }
    }
}

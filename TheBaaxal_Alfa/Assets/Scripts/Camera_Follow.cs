using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public float cameraDistance = 0.1f;

    public Camera cam;
    void Start()
    {
        transform.position = cam.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, gameObject.transform.position.y, gameObject.transform.position.z);

        gameObject.transform.position = newPos;
    }
}

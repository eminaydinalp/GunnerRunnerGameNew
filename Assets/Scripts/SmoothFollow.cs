using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{

    
    public Transform target;
    public float distance = 20.0f;
    public float height = 5.0f;
    public float xAngle = 25f;
    public float yAngle = -25f;

    public float heightDamping = 2.0f;
    public float movementDamping,value;



    private float rotationSnapTime = 0;




    private float usedDistance;

    float wantedRotationAngle;
    float wantedHeight;

    float currentRotationAngle;
    float currentHeight;

    Vector3 wantedPosition;


    private float yVelocity = 0.0F;
    private float zVelocity = 0.0F;
    public bool state;
    public static SmoothFollow Instance;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {


    }


    void LateUpdate()
    {

        if (!state)
        {

            wantedHeight = target.position.y + height;
            currentHeight = transform.position.y;

            wantedRotationAngle = yAngle;
            currentRotationAngle = transform.eulerAngles.y;

            currentRotationAngle = Mathf.SmoothDampAngle(currentRotationAngle, wantedRotationAngle, ref yVelocity, rotationSnapTime * Time.deltaTime);

            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

            wantedPosition = target.position;
            wantedPosition.y = currentHeight;
            usedDistance = Mathf.SmoothDampAngle(usedDistance, distance, ref zVelocity, usedDistance * Time.deltaTime);

            wantedPosition += Quaternion.Euler(0, currentRotationAngle, 0) * new Vector3(0, 0, -usedDistance);

            transform.position = Vector3.Lerp(transform.position, wantedPosition, movementDamping);
            //transform.position = wantedPosition;
            transform.localEulerAngles = new Vector3(xAngle, yAngle, transform.localEulerAngles.z);
        }
    }


    public void SetTarget(GameObject newTarget)
    {
        if (newTarget.transform != target)
        {
            movementDamping = value;
            this.target = newTarget.transform;
            Invoke(nameof(a), 0.2f);
        }
    }
    void a()
    {
        movementDamping = 1;
    }

}

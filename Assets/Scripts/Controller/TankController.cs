using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class TankWheel
{
    public WheelCollider WheelCol;
    public enum Side
    {
        Left,
        Right
    }
    public Side side;
    public TankWheel(WheelCollider WheelCol, byte side)
    {
        this.WheelCol = WheelCol;
        if (side == 0)
        {
            this.side = Side.Left;
        }
        else
        {
            this.side = Side.Right;
        }
    }
}

public class TankController : MonoBehaviour
{

    public List<TankWheel> TWheels;

    public float rotateOnStandTorque = 1500.0f;
    public float motorTorque = 1500.0f;
    public float rotateOnStandBrakeTorque = 500.0f;
    public float maxSpeedKMH = 0f;

    public AnimationCurve speed;

    public int speedd = 0;

    public float rotationSpeedInDegrees;

    private float maxSpeed;
    private float rotationSpeed;

    public Transform Center;

    private Rigidbody rigid;

    public float time = 0f;
    // Start is called before the first frame update
    void Start()
    {
        maxSpeed = maxSpeedKMH / 3.6f;
        rotationSpeed = rotationSpeedInDegrees * 0.017f;
        rigid = GetComponent<Rigidbody>();

        rigid.centerOfMass = Center.localPosition;

        Transform childTrans = transform.Find("WheelColliders");
        int children = childTrans.childCount;
        for (int i = 0; i < children; ++i)
        {
            byte side = 0;
            if (childTrans.GetChild(i).name.Contains("right"))
            {
                side = 1;
            }
            TWheels.Add(new TankWheel(childTrans.GetChild(i).gameObject.GetComponent<WheelCollider>(), side));
        }
        string name = childTrans.GetChild(0).name + " 0 ";
        Debug.Log(name);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float axel = Input.GetAxis("Vertical");
        float steering = Input.GetAxis("Horizontal");

        rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, maxSpeed);

        rigid.angularVelocity = Vector3.ClampMagnitude(rigid.angularVelocity, rotationSpeed);

        foreach (TankWheel TWheel in TWheels)
        {


            switch (TWheel.side)
            {
                case TankWheel.Side.Left:
                    MotorForce(TWheel.WheelCol, axel, steering);
                    break;
                case TankWheel.Side.Right:
                    MotorForce(TWheel.WheelCol, axel, -steering);
                    break;
            }
        }
    }

    public float minOnStayStiffness = 0.06f; 
    public float minOnMoveStiffness = 0.05f;   

    public void MotorForce(WheelCollider WCol, float axel, float steering)
    {

        WheelFrictionCurve fc = WCol.sidewaysFriction;  

        if (WCol.rpm > 0 && axel < 0)
        {
            WCol.brakeTorque = 1000f;
            WCol.motorTorque = 0f;
        }
        else if (WCol.rpm < 0 && axel > 0)
        {
            WCol.brakeTorque = 1000f;
            WCol.motorTorque = 0f;
        }

        if (axel == 0 && steering == 0)
        {
            WCol.brakeTorque = 1000f;
            WCol.motorTorque = 0f;
        }
        else if (axel == 0.0f)
        {
            WCol.brakeTorque = 0;
            WCol.motorTorque = steering * rotateOnStandTorque;

            fc.stiffness = 1.0f + minOnStayStiffness - Mathf.Abs(steering);
        }
        else
        { 

            WCol.brakeTorque = 0;  
            WCol.motorTorque = axel * motorTorque;  

            if (steering < 0)
            { 
                WCol.brakeTorque = 0f;
                WCol.motorTorque = steering * motorTorque * 2.0f;
                fc.stiffness = 1.0f + minOnMoveStiffness - Mathf.Abs(steering);  
            }

            if (steering > 0)
            { 
                WCol.motorTorque = steering * motorTorque;
                fc.stiffness = 1.0f + minOnMoveStiffness - Mathf.Abs(steering); 
            }
        }
        if (fc.stiffness > 1.5f) fc.stiffness = 1.5f;
        WCol.sidewaysFriction = fc;
    }
}
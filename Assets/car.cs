using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class car : MonoBehaviour
{
    public Rigidbody rigid; // car's rigidbody
    public WheelCollider wheel1, wheel2, wheel3, wheel4; // wheel colliders
    public float driveSpeed, steerSpeed;
    float horizontalInput, verticalInput;

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    void FixedUpdate() // used physics related actions
    {
        float motor = -Input.GetAxis("Vertical") * driveSpeed;
        wheel1.motorTorque = motor;
        wheel2.motorTorque = motor;
        wheel3.motorTorque = motor;
        wheel4.motorTorque = motor;
        wheel1.steerAngle = steerSpeed * horizontalInput;
        wheel2.steerAngle = steerSpeed * horizontalInput;
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
public class Car : MonoBehaviour
{
	public Rigidbody rigid; // car's rigidbody
	public WheelCollider wheel1, wheel2, wheel3, wheel4; // wheel colliders

	// Steering (degrees per second)
	public float maxSteerAngle = 30f;   // full lock
	public float steerRate = 90f;   // toward target 
	public float returnRate = 150f; // recentering wheels

	// Drive
	public float driveSpeed = 800f;
	public float brakePower = 3000f;
	public float decelerationPower = 1000f;
	const float threshold = 0.5f;

	// Input management
	InputAction inputMove;
	InputAction inputAccelerate;
	InputAction inputBrake;

	// Variables
	Vector2 moveValue;
	float currentSteer;
	float currentSpeed;

	private void Start()
	{
		inputMove       = InputSystem.actions.FindAction("Move");
		inputAccelerate = InputSystem.actions.FindAction("Accelerate");
		inputBrake      = InputSystem.actions.FindAction("Brake");
	}

	void Update()
	{
		moveValue = inputMove.ReadValue<Vector2>();
	}

	void FixedUpdate() // loop used physics related actions
	{
		// Move forwards/backwards
		Drive();

		// Steer
		Steer();

        // Flip back up if tilted
        float tilt = Vector3.Angle(transform.up, Vector3.up);
        if (tilt > 45f)
        {
            Vector3 torqueAxis = Vector3.Cross(transform.up, Vector3.up);
            rigid.AddTorque(torqueAxis * 5f, ForceMode.Acceleration);
        }
    }

	void Drive()
	{
        currentSpeed = Vector3.Dot(rigid.linearVelocity, -transform.forward);
        float motor = 0;
        float brake = 0;

        if (inputAccelerate.IsPressed())
        {
            if (currentSpeed > -threshold) motor = -driveSpeed; //forward
            else brake = brakePower;                            //brake
        }
        else if (inputBrake.IsPressed())
        {
            if (currentSpeed > threshold) brake = brakePower;   //forward
            else motor = driveSpeed;                            //reverse
        }
        else
        {
            brake = decelerationPower;
        }

        // Spin wheels
        foreach (var wheel in new[] { wheel1, wheel2, wheel3, wheel4 })
        {
            wheel.motorTorque = motor;
            wheel.brakeTorque = brake;
        }
    }

	void Steer()
	{
        float targetSteer = maxSteerAngle * Mathf.Round(moveValue.x);
        float rate = Mathf.Approximately(moveValue.x, 0f) ? returnRate : steerRate;
        currentSteer = Mathf.MoveTowards(currentSteer, targetSteer, rate * Time.fixedDeltaTime);

        wheel1.steerAngle = currentSteer;
        wheel2.steerAngle = currentSteer;
    }


}

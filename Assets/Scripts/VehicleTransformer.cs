using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class VehicleTransformer : MonoBehaviour
{
    private enum VehicleState { Car, Boat };

    [SerializeField] private GameObject car;
    [SerializeField] private GameObject boat;
    [SerializeField] private WheelCollider[] wheels;

    InputAction inputShift;

    private VehicleState currentState = VehicleState.Car;

    private void Start()
    {
        SetMode(currentState);

        inputShift = InputSystem.actions.FindAction("Shift");
    }

    private void Update()
    {
        if(inputShift.WasReleasedThisFrame())
        {
            // Set mode to next one
            int enumLength = VehicleState.GetNames(typeof(VehicleState)).Length;
            int nextState = ((int)currentState + 1) % enumLength;
            SetMode((VehicleState)nextState);

            //Debug.Log()
        }
    }

    private void SetMode(VehicleState state)
    {
        // Activate/Deactivate children of Vehicle hierarchy based on type
        car.SetActive(state == VehicleState.Car);
        boat.SetActive(state == VehicleState.Boat);

        // Never forget who you truly are (so you can shift properly)
        currentState = state;
    }
}

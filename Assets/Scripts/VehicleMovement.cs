using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleMovement : MonoBehaviour {
    private PlayerInput playerInput;

    [SerializeField] private float speedY = 1.0f;
    [SerializeField] private float speedX = 1.0f;

    [Space(5)]
    [SerializeField] private float accelerationAmount = 1.5f;
    [SerializeField] private float decelerationAmount = 0.5f;
    private float currentAcceleration = 0f;

    [Space(5)]
    [SerializeField] private int laneQuantity = 3;
    [SerializeField] private float laneWidth = 5f;

    [Space(5)]
    [SerializeField] int startingLane = 3;
    private int currentLane;
    [SerializeField] private Transform[] lanes;

    private void Awake() {
        playerInput = new PlayerInput();
        StartingVehiclePosition();
    }

    void Update() {
        // VERTICAL INPUT
        if (playerInput.VehicleMovement.MoveUp.WasPerformedThisFrame()) {
            MoveUp();
        } else if (playerInput.VehicleMovement.MoveDown.WasPerformedThisFrame()) {
            MoveDown();
        }

        // HORIZONTAL INPUT
        currentAcceleration = playerInput.VehicleMovement.Acceleration.ReadValue<Vector2>().x;
        MoveX();
        SwitchLanes();
    }

    private void StartingVehiclePosition() {
        currentLane = startingLane;
        currentLane = Mathf.Clamp(currentLane, 0, lanes.Length - 1);

        this.transform.position = new Vector3(this.transform.position.x, lanes[currentLane].position.y);
    }

    private void MoveX() {
        if (currentAcceleration > 0) {
            currentAcceleration *= accelerationAmount;
        }

        Vector3 targetPosition = transform.right * (speedX + currentAcceleration) * Time.deltaTime;
        this.transform.position += targetPosition;
    }

    private void MoveUp() {
        Debug.Log("Move Up");

        if ((currentLane - 1) < 0) {
            return;
        }

        currentLane--;
    }

    private void MoveDown() {
        Debug.Log("Move Down");

        if ((currentLane + 1) >= lanes.Length) {
            return;
        }

        currentLane++;
    }

    private void SwitchLanes() {
        Vector3 targetPosition = new Vector3(this.transform.position.x, lanes[currentLane].position.y);
        this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, speedY * Time.deltaTime);
    }

    private void OnEnable() {
        playerInput.Enable();
    }

    private void OnDisable() {
        playerInput.Disable();
    }
}
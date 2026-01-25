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
    }

    private void StartingVehiclePosition() {
        currentLane = startingLane;
        currentLane = Mathf.Clamp(currentLane, 0, lanes.Length - 1);

        this.transform.position = new Vector3(this.transform.position.x, lanes[currentLane].position.y);
    }

    private void MoveX() {
        if (currentAcceleration < 0) {
            currentAcceleration *= decelerationAmount;
        } else if (currentAcceleration > 0) {
            currentAcceleration *= accelerationAmount;
        }

        Vector3 targetPosition = transform.right * (speedX + currentAcceleration) * Time.deltaTime;
        this.transform.position += targetPosition;
    }

    private void MoveY() {
        int direction = Mathf.RoundToInt(playerInput.VehicleMovement.Acceleration.ReadValue<Vector2>().x);

        Debug.Log("DirectionX: " + direction);

        currentLane += direction;
        currentLane = Mathf.Clamp(currentLane, -laneQuantity, laneQuantity);

        Vector3 targetPosition = transform.right * (currentLane + laneWidth);
        this.transform.position += targetPosition;

        for (int i = 0; i < laneQuantity; i++) {
            Vector2 laneStart = transform.right * (i + laneWidth);
            Vector2 laneEnd = new Vector2(laneStart.x, laneStart.y + 10f);

            Debug.DrawLine(laneStart, laneEnd, Color.red);
        }

        for (int i = 0; i > -laneQuantity; i--) {
            Vector2 laneStart = transform.right * (i - laneWidth);
            Vector2 laneEnd = new Vector2(laneStart.x, laneStart.y + 10f);

            Debug.DrawLine(laneStart, laneEnd, Color.yellow);
        }
    }

    private void MoveUp() {
        Debug.Log("Move Up");

        if ((currentLane - 1) < 0) {
            return;
        }

        currentLane--;

        Vector3 targetPosition = new Vector3(this.transform.position.x, lanes[currentLane].position.y);
        StartCoroutine(SwitchLane(targetPosition));
    }

    private void MoveDown() {
        Debug.Log("Move Down");

        if ((currentLane + 1) >= lanes.Length) {
            return;
        }

        currentLane++;

        Vector3 targetPosition = new Vector3(this.transform.position.x, lanes[currentLane].position.y);
        StartCoroutine(SwitchLane(targetPosition));
    }

    private IEnumerator SwitchLane(Vector3 lane) {
        float elapsedTime = 0f;

        while (elapsedTime < speedY) {
            this.transform.position = Vector3.Lerp(this.transform.position, lane, (elapsedTime / speedY));
            MoveX();

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        this.transform.position = new Vector3(this.transform.position.x, lane.y);
    }

    private void OnEnable() {
        playerInput.Enable();
    }

    private void OnDisable() {
        playerInput.Disable();
    }
}
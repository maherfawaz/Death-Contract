using UnityEngine;
using UnityEngine.UIElements;

public class Missile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float distance = 50f;

    [SerializeField] private float damageAmount = 20f;
    [SerializeField] private float positionPredictionAmount = 15f;

    private Vector3 destination;
    private Transform playerTransform;

    private bool isFired = false;

    private void Awake() 
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start() 
    {
        // CHECK IF PLAYER IS FOUND:

        if (!playerTransform) 
        {
            playerTransform = new GameObject().transform;
            Debug.LogWarning("Missile couldn't find player!");
        }
    }

    private void Update() 
    {
        if (isFired) 
        {
            Flight();
        }
    }

    private void LookAtPlayer() 
    {
        Debug.Log("Lock On!");

        destination = new Vector2(playerTransform.position.x + positionPredictionAmount, playerTransform.position.y);

        Vector3 direction = destination - this.transform.position;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= 90f;

        this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Flight() 
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, destination * 5, speed * Time.deltaTime);
        //this.transform.Translate(-transform.up * speed * Time.deltaTime);
    }

    public void Explode() 
    {
        Destroy(this.gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.gameObject.CompareTag("Player")) {

            LookAtPlayer();
            isFired = true;

        }
    }
}

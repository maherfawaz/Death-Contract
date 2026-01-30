using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UIElements;

public class Missile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float distance = 50f;

    [SerializeField] private int damageAmount = 1;
    [SerializeField] private float positionPredictionAmount = 15f;

    private Vector3 destination;
    private Transform playerTransform;

    private bool isFired = false;

    private bool isLockedIn = false;

    [Space(5)]
    [SerializeField] private AudioClip explosionSFX;
    [SerializeField] private GameObject explosion;

    private void Awake() 
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        destination = new Vector3(this.transform.position.x, this.transform.position.y + distance);
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

    private void Flight() 
    {
        this.transform.position += (transform.up * speed * Time.deltaTime);
    }

    public void Explode() 
    {
        GameObject newExplosion = Instantiate(explosion, this.transform.position, Quaternion.identity);
        newExplosion.SetActive(true);

        SoundEffectsManager.instance.PlayAudioClip(explosionSFX);

        Destroy(this.gameObject);
    }

    public int GetDamageAmount() {
        return damageAmount;
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.gameObject.CompareTag("Player")) {

            isFired = true;
            Debug.Log("Missile fired!");
        }
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class RecklessDriver : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float distance = 30f;
    [SerializeField] private int damageAmount = 25;

    private CircleCollider2D activationTrigger;
    private bool isDriving = false;

    private Vector2 destination;

    [Space(5)]
    [SerializeField] private GameObject explosion;
    [SerializeField] private AudioClip crashSFX;

    private void Awake() 
    {
        activationTrigger = GetComponent<CircleCollider2D>();    
    }

    void Start()
    {
        activationTrigger.enabled = true;
        destination = new Vector2(this.transform.position.x - distance, this.transform.position.y);
    }

    void Update()
    {
        if (isDriving) 
        {
            Drive();
        }
    }

    private void Drive() 
    {
        // DRIVE FORWARD
        this.transform.position = Vector2.MoveTowards(this.transform.position, destination, speed * Time.deltaTime);
    }

    public void Explode() 
    {
        GameObject newExplosion = Instantiate(explosion, this.transform.position, Quaternion.identity);
        newExplosion.SetActive(true);

        SoundEffectsManager.instance.PlayAudioClip(crashSFX);
        Debug.Log("Car Exploded!");

        Destroy(this.gameObject);
    }

    public int GetDamageAmount() 
    {
        return damageAmount;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        
        if (collision.gameObject.CompareTag("Player")) {

            isDriving = true;
            activationTrigger.enabled = false;

            //Debug.Log("Beep Beep Bitch!");
        }
    }
}

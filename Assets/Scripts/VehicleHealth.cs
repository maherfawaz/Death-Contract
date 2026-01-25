using UnityEngine;

public class VehicleHealth : MonoBehaviour {
    [SerializeField] private float maxHealth = 100;
    private float currentHeath;

    private bool isDeath = false;

    private void Awake() {
        currentHeath = maxHealth;
    }

    private void TakeDamage(int damageAmount) {
        currentHeath -= damageAmount;
        currentHeath = Mathf.Clamp(currentHeath, 0, maxHealth);

        isDeath = currentHeath <= 0;
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Mine")) 
        {
            TakeDamage(collision.GetComponent<Mine>().GetDamageAmount());
            collision.GetComponent<Mine>().Explode();

            Debug.Log("You hit a mine!");
        }

        if (collision.gameObject.CompareTag("Reckless Driver")) 
        {
            TakeDamage(collision.GetComponentInParent<RecklessDriver>().GetDamageAmount());
            collision.GetComponentInParent<RecklessDriver>().Explode();

            Debug.Log("You hit a car!");
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class VehicleHealth : MonoBehaviour {
    [SerializeField] private float maxHealth = 100;
    private float currentHeath;

    private bool isDeath = false;

    [Space(5)]
    [SerializeField] private float respawnTime = 3f;
    private VehicleMovement vehicleMovement;

    private int currentSceneIndex;

    [Space(5)]
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private TextMeshProUGUI deathScreenRespawnCountdown;

    private void Awake() {
        currentHeath = maxHealth;
        vehicleMovement = GetComponent<VehicleMovement>();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (deathScreen) { deathScreen.SetActive(false); }
    }

    private void CheckIfDeath() {

        if (isDeath)
        {
            vehicleMovement.enabled = false;

            if (deathScreen) { deathScreen.SetActive(true); }

            StartCoroutine(DeathSequence());
        }
    }

    private IEnumerator DeathSequence() {

        Debug.Log("You died!");

        float elapsedTime = 0f;

        while (elapsedTime < respawnTime) {

            float timeRemaining = respawnTime - elapsedTime;
            elapsedTime += Time.deltaTime;

            if (deathScreenRespawnCountdown) { deathScreenRespawnCountdown.text = $"Respawning in {timeRemaining}"; }
            Debug.Log($"Respawing in: {timeRemaining}");
            yield return null;
        }

        // RELOAD CURRENT SCENE
        SceneManager.LoadScene(currentSceneIndex);
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
            CheckIfDeath();
        }

        if (collision.gameObject.CompareTag("Reckless Driver")) 
        {
            TakeDamage(collision.GetComponentInParent<RecklessDriver>().GetDamageAmount());
            collision.GetComponentInParent<RecklessDriver>().Explode();

            Debug.Log("You hit a car!");
            CheckIfDeath();
        }
    }
}

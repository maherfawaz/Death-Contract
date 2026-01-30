using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor.Build.Content;

public class VehicleHealth : MonoBehaviour {
    [SerializeField] private float maxHealth = 100;
    private float currentHeath;

    private bool isDeath = false;

    [Space(5)]
    [SerializeField] private float respawnTime = 3f;
    [SerializeField] private float deathDuration = 1f;
    private VehicleMovement vehicleMovement;

    private int currentSceneIndex;

    [Space(5)]
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private TextMeshProUGUI deathScreenRespawnCountdown;

    [Space(5)]
    [SerializeField] private GameObject victoryScreen;

    [Space(5)]
    [SerializeField] private GameObject[] heartsGO;
    [SerializeField] private Stack<GameObject> heartStack;

    private void Awake() {
        currentHeath = maxHealth;
        vehicleMovement = GetComponent<VehicleMovement>();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (deathScreen) { deathScreen.SetActive(false); }
        if (victoryScreen) {  victoryScreen.SetActive(false); }

        AddHeartsToStack();
    }

    private void AddHeartsToStack() {

        if (heartsGO == null) { return; }

        heartStack = new Stack<GameObject>();

        foreach (GameObject heart in heartsGO) {
            heartStack.Push(heart);
        }
    }

    private void Victory() {

        vehicleMovement.enabled = false;

        if ( victoryScreen) { victoryScreen.SetActive(true); }
    }

    private void CheckIfDeath() {

        if (isDeath)
        {
            vehicleMovement.enabled = false;

            StartCoroutine(DeathSequence());
        }
    }

    private IEnumerator DeathSequence() {

        Debug.Log("You died!");

        yield return new WaitForSeconds(deathDuration);

        if (deathScreen) { deathScreen.SetActive(true); }

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

        if (heartStack.Count > 0) {
            GameObject heart = heartStack.Pop();
            heart.SetActive(false);
        }
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

        if (collision.gameObject.CompareTag("Missile")) {
            TakeDamage(collision.GetComponentInParent<Missile>().GetDamageAmount());
            collision.GetComponentInParent<Missile>().Explode();

            Debug.Log("You hit a missile!");
            CheckIfDeath();
        }

        if (collision.gameObject.CompareTag("Finish Line")) {

            Victory();
            Debug.Log("You survived!");
        }
    }
}

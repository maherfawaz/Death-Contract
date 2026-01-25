using System.Collections;
using UnityEngine;

public class Mine : MonoBehaviour {
    [SerializeField] private int damageAmount = 10;

    [Space(5)]
    [SerializeField] private GameObject explosion;
    [SerializeField] private float explosionExpansionSpeed = 1f;
    [SerializeField] private float explosionFadeoutDuration = 5f;

    private bool isActivated = false;

    private void Awake() {
        explosion.SetActive(false);
    }

    public void Explode() {
       
        if (isActivated) 
        {
            return;
        }

        StartCoroutine(ExplosionSequence());
    }

    private IEnumerator ExplosionSequence() {

        isActivated = true;

        Vector3 explosionScale = explosion.transform.localScale;

        explosion.SetActive(true);
        explosion.transform.localScale = Vector3.zero;

        float elapsedTime = 0f;

        while (elapsedTime < explosionExpansionSpeed) {
            explosion.transform.localScale = Vector3.Lerp(this.transform.localScale, explosionScale, (elapsedTime / explosionExpansionSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(explosionFadeoutDuration);

        Destroy(this.gameObject);
    }

    public int GetDamageAmount() {
        return damageAmount;
    }
}

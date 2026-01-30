using System.Collections;
using System.Linq.Expressions;
using UnityEngine;

public class Mine : MonoBehaviour {
    [SerializeField] private int damageAmount = 10;

    [Space(5)]
    [SerializeField] private GameObject explosion;
    [SerializeField] private float explosionDuration = 5f;

    private bool isActivated = false;

    [Space(5)]
    [SerializeField] private AudioClip explodeSFX;

    [SerializeField] private SpriteRenderer mineSprite;
    [SerializeField] private SpriteRenderer mineSprite2;

    private void Awake() {
        explosion.SetActive(false);
        mineSprite = GetComponentInChildren<SpriteRenderer>();
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

        mineSprite.enabled = false;
        mineSprite2.enabled = false;

        SoundEffectsManager.instance.PlayAudioClip(explodeSFX);

        Vector3 explosionScale = explosion.transform.localScale;

        GameObject newExplosion =  Instantiate(explosion, this.transform.position, Quaternion.identity);
        newExplosion.transform.parent = this.transform;
        newExplosion.SetActive(true);

        yield return new WaitForSeconds(explosionDuration);

        Destroy(this.gameObject);

        yield return null;

    }

    public int GetDamageAmount() {
        return damageAmount;
    }
}

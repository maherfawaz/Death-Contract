using UnityEngine;

// Currently used for Music
public class DontDestroyOnLoad : MonoBehaviour
{
    [Header("Dynamic")]
    public string objectTag;

    void Start() {
        objectTag = gameObject.tag;
        GameObject[] objs = GameObject.FindGameObjectsWithTag(objectTag);

        if (objs.Length > 1) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }
    }
}

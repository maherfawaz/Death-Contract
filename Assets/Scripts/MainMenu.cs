using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Update() {
        if (Keyboard.current.sKey.wasPressedThisFrame) {
            SceneManager.LoadScene(1);
        }
    }
}

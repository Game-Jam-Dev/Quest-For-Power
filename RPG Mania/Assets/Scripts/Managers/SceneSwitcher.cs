using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {
    private int outskirtsScene = 3;

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(outskirtsScene);
        }
    }

}
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {
    public string outskirtsScene = "Outskirts";

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(outskirtsScene);
        }
    }

}
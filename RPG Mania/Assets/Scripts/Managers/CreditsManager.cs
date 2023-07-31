using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour {
    [SerializeField] private Button back;
    private int titleScene = 0;

    private void Start() {
        back.onClick.AddListener(() => SceneManager.LoadScene(titleScene));
    }
}
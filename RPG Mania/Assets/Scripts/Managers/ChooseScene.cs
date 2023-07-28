using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseScene : MonoBehaviour {
    [SerializeField] Button throne, wilds, title;

    private void Awake() {
        throne.onClick.AddListener(() => SceneManager.LoadScene("Throne Room"));
        wilds.onClick.AddListener(() => SceneManager.LoadScene("Wilds"));
        title.onClick.AddListener(() => SceneManager.LoadScene("Title Screen"));
    }
}
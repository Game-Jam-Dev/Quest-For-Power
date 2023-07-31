using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseScene : MonoBehaviour {
    [SerializeField] Button throne, wilds, outskirts, title;

    private void Awake() {
        throne.onClick.AddListener(() => SceneManager.LoadScene("Throne Room"));
        wilds.onClick.AddListener(StartWilds);
        outskirts.onClick.AddListener(() => SceneManager.LoadScene("Outskirts"));
        title.onClick.AddListener(() => SceneManager.LoadScene("Title Screen"));
    }

    private void StartWilds()
    {
        GameManager.instance.SetPlayerExperience(0);
        GameManager.instance.SetPlayerSkills(new List<int>{0,0,0,0,0});
        SceneManager.LoadScene("Wilds");
    }
}
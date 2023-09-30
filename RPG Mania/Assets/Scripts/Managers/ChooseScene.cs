using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseScene : MonoBehaviour {
    [SerializeField] Button intro, throne, wilds, outskirts, title;

    private void Awake() {
        intro.onClick.AddListener(() => SceneManager.LoadScene("Intro Cutscene"));
        throne.onClick.AddListener(() => SceneManager.LoadScene("Throne Room"));
        wilds.onClick.AddListener(StartWilds);
        outskirts.onClick.AddListener(StartOutskirts);
        title.onClick.AddListener(() => SceneManager.LoadScene("Title Screen"));
    }

    private void StartWilds()
    {
        GameManager.instance.SetPlayerLevel(1);
        GameManager.instance.SetPlayerExperience(0);
        GameManager.instance.SetPlayerSkills(new List<int>{0,0,0,0,0});
        SceneManager.LoadScene("Wilds");
    }

    private void StartOutskirts()
    {
        GameManager.instance.SetPlayerLevel(10);
        GameManager.instance.SetPlayerExperience(0);
        GameManager.instance.SetPlayerSkills(new List<int>{0,0,0,0,0});
        GameManager.instance.SetVisitedWilds(true);
        SceneManager.LoadScene("Outskirts");
    }
}
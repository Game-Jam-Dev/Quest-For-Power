using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI logTextPrefab;
    [SerializeField] private GameObject logContainer;
    private Queue<TextMeshProUGUI> logTexts = new();

    public int logMax = 7;

    public void SetText(string log)
    {
        TextMeshProUGUI logText = Instantiate(logTextPrefab, logContainer.transform);
        logText.text = log;
        logTexts.Enqueue(logText);

        if (logTexts.Count > logMax)
        {
            Destroy(logTexts.Peek().gameObject);
            logTexts.Dequeue();
        }
    }
}
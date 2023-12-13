using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour {
    [SerializeField] private GameObject comboObject;
    public Sprite active, highlighted, inactive;

    private List<GameObject> totalCombos = new();
    private List<Image> activeCombos = new();
    private List<Image> highlightCombos = new();
    private int activeCombo = 0;
    

    public void SetComboLength(int length)
    {
        if (length == activeCombos.Count) return;

        activeCombos.Clear();

        int i = 0;

        for (i = 0; i < totalCombos.Count; i++)
        {
            GameObject combo = totalCombos[i];

            if (length > i)
            {
                combo.SetActive(true);

                Image image = combo.GetComponent<Image>();
                image.sprite = inactive;
                activeCombos.Add(image);
            } else 
                combo.SetActive(false);
        }

        while (i > length)
        {
            GameObject combo = Instantiate(comboObject, transform);
            totalCombos.Add(combo);

            Image image = combo.GetComponent<Image>();
            image.sprite = inactive;
            activeCombos.Add(image);

            i++;
        }
    }

    // public void HighlightCombo(int count)
    // {
    //     foreach (Image i in highlightCombos)
    //     {
    //         i.sprite = inactive;
    //     }

    //     highlightCombos.Clear();

    //     int max = Mathf.Min(count + activeCombo, activeCombos.Count);

    //     for (int i = activeCombo; activeCombo < max; i++)
    //     {
    //         Image image = activeCombos[i];
    //         image.sprite = highlighted;

    //         highlightCombos.Add(image);
    //     }
    // }

    // public void SelectCombo()
    // {
    //     foreach (Image i in highlightCombos)
    //     {
    //         i.sprite = active;
    //     }

    //     activeCombo += highlightCombos.Count;
    //     highlightCombos.Clear();
    // }

    public void SelectCombo(int count)
    {
        int max = Mathf.Max(count + activeCombo, activeCombos.Count);

        for (int i = activeCombo; activeCombo < max; i++)
        {
            Image image = activeCombos[i];
            image.sprite = active;
        }
    }

    public void ResetCombo()
    {
        foreach (Image i in activeCombos)
        {
            i.sprite = inactive;
        }

        activeCombo = 0;
    }

    public int ComboLength() { return activeCombo; }

    public bool MaxCombo() { return activeCombo == activeCombos.Count; }
}
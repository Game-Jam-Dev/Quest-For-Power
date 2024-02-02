using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuContainer : MonoBehaviour {
    [SerializeField] private GameObject leftArrow, rightArrow;
    public List<List<GameObject>> menuButtons = new();

    public int activeIndex = 0;

    private void Start() {
        leftArrow.GetComponent<Button>().onClick.AddListener(() => MoveLeft(this));
        rightArrow.GetComponent<Button>().onClick.AddListener(() => MoveRight(this));
    }

    private void OnEnable() {
        ArrowCheck();
    }

    private void OnDisable() {
        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
    }

    public void SetButtons(List<GameObject> buttons)
    {
        menuButtons.Clear();
        activeIndex = 0;

        int c = 0;
        int r = 0;

        List<GameObject> buttonRow = new();

        for (int i = 0; i < buttons.Count; i++)
        {
            GameObject button = buttons[i];

            buttonRow.Add(button);
            buttonRow[c].SetActive(false);

            c++;

            if (c >= 3)
            {
                menuButtons.Add(buttonRow);
                buttonRow = new();
                r++;
                c = 0;
            }

            Debug.Log(c + " " + r);
        }

        if (c != 0)
        {
            menuButtons.Add(buttonRow);
        }

        ToggleButtons(0);
    }

    public static void MoveLeft(MenuContainer menu)
    {
        if (menu.activeIndex == 0 || !menu.gameObject.activeSelf) return;

        menu.ToggleButtons(menu.activeIndex, false);
        menu.activeIndex--;
        menu.ToggleButtons(menu.activeIndex);

        menu.ArrowCheck();
    }

    public static void MoveRight(MenuContainer menu)
    {
        if (menu.activeIndex >= menu.menuButtons.Count - 1 || !menu.gameObject.activeSelf) return;

        menu.ToggleButtons(menu.activeIndex, false);
        menu.activeIndex++;
        menu.ToggleButtons(menu.activeIndex);

        menu.ArrowCheck();
    }

    private void ToggleButtons(int index, bool active = true)
    {
        if (index < 0 || index >= menuButtons.Count) return;

        foreach (GameObject b in menuButtons[index])
        {
            b.SetActive(active);
        }
    }

    private void ArrowCheck()
    {
        if (activeIndex <= 0)
            leftArrow.SetActive(false);
        else
            leftArrow.SetActive(true);

        if (activeIndex >= menuButtons.Count - 1)
            rightArrow.SetActive(false);
        else
            rightArrow.SetActive(true);
    }
}
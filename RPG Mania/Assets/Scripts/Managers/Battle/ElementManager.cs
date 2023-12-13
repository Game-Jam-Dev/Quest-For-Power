using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ElementManager : MonoBehaviour {
    [SerializeField] private Image image;
    public Sprite water, fire, wind, earth;
    private Sprite[] spriteList;

    public enum Element{
        None,
        Water,
        Fire,
        Wind,
        Earth,
    }

    private void Awake() {
        spriteList = new Sprite[4] { water, fire, wind, earth };
    }

    public void SetElement(Element element)
    {
        if ((int)element > 0)
        {
            image.sprite = spriteList[(int)element - 1];
            image.color = Color.white;
            
        } else 
        {
            image.color = Color.clear;
        }

        
    }
}
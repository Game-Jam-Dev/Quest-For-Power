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
        Debug.Log(element);
        if ((int)element > 0)
        {
            image.sprite = spriteList[(int)element - 1];
            image.color = Color.white;
            
        } else 
        {
            image.color = Color.clear;
        }
    }

    public void Highlight()
    {
        image.color = Color.yellow;
    }

    public void Unhighlight()
    {
        image.color = Color.white;
    }

    public static Element GetElement(SkillAction skill)
    {
        return skill.Name switch
        {
            "Water" => Element.Water,
            "Fire" => Element.Fire,
            "Wind" => Element.Wind,
            "Earth" => Element.Earth,
            _ => Element.None,
        };
    }

    public static int CalculateEffectiveDamage(Element attack, Element defend, int damage)
    {
        if (attack == Element.None || defend == Element.None) return damage;

        if (attack == defend) return damage/2;

        switch (attack)
        {
            case Element.Water:
            if (defend == Element.Fire) damage += damage/2;

            else if (defend == Element.Earth) damage -= damage/2;
            break;

            case Element.Fire:
            if (defend == Element.Wind) damage += damage/2;

            else if (defend == Element.Water) damage -= damage/2;
            break;

            case Element.Wind:
            if (defend == Element.Earth) damage += damage/2;

            else if (defend == Element.Fire) damage -= damage/2;
            break;

            case Element.Earth:
            if (defend == Element.Water) damage += damage/2;

            else if (defend == Element.Wind) damage -= damage/2;
            break;
        }

        return damage;
    }
}
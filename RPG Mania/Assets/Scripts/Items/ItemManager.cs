using System.Linq;
using UnityEngine;

public class ItemManager {
    public static ItemManager Instance { get; private set; }
    private readonly Item[] items;

    public ItemManager()
    {
        items = Resources.LoadAll<Item>("Items");

        // sort items by child class Potion, Essence, StatChanger, then Bomb
        System.Array.Sort(items, (a, b) =>
        {
            if (a is Potion && !(b is Potion)) return -1;
            if (a is Essence && !(b is Potion || b is Essence)) return -1;
            if (a is StatChanger && !(b is Potion || b is Essence || b is StatChanger)) return -1;
            return 1;
        });
    }

    public static ItemManager GetInstance()
    {
        Instance ??= new ItemManager();

        return Instance;
    }

    public Item GetItem(int index)
    {
        if (index < items.Length) return items[index];

        else return null;
    }

    public Item[] GetItems() { return items; }

    public static void UseItem(Item item, BattleManager battleManager)
    {
        switch (item)
        {
            case Potion potion:
                potion.Use(battleManager.player);
                break;
            case Essence essence:
                if (essence.target == Item.Target.Single)
                    essence.Use(battleManager.characterToAttack);
                else
                    essence.Use(new CharacterBattle[] { battleManager.player }.Concat(battleManager.enemies).ToArray());
                break;
            case StatChanger statChanger:
                if (statChanger.target == Item.Target.Self)
                    statChanger.Use(battleManager.player);
                else
                    statChanger.Use(battleManager.characterToAttack);
                break;
        }

        GameManager.instance.RemoveItem(item);
    }
}
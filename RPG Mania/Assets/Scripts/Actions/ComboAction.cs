using UnityEngine;

public class ComboAction
{
    public delegate bool ActionDelegate(CharacterBattle self, CharacterBattle target, int comboDepth);

    public string Name { get; private set; }
    public int Cost { get; private set; }
    public ActionDelegate Action { get; private set; }

    public ComboAction(string name, int cost, ActionDelegate action)
    {
        Name = name;
        Cost = cost;
        Action = action;
    }

    public static bool DoAttack(CharacterBattle self, CharacterBattle target, int comboDepth, string attackName, float accuracy, float damageMultiplier)
    {
        self.SetAnimationTrigger(attackName);

        bool hitAttack = AttackCheck(self, target, accuracy, comboDepth);

        if (hitAttack) HitEnemy(self, target, damageMultiplier);

        return hitAttack;
    }

    private static bool AttackCheck(CharacterBattle self, CharacterBattle target, float moveAccuracy, int comboDepth)
    {
        float r = Random.Range(0,1f);
        float a  = (moveAccuracy * self.accuracy) - target.evasion + (comboDepth * .05f);
        self.hitTarget = r < a;

        return self.hitTarget;
    }

    private static void HitEnemy(CharacterBattle self, CharacterBattle target, float d)
    {
        target.SetAnimationTrigger("Attacked");

        int damage = (int)(self.attack * d - target.defense);
        if (damage < 0) damage = 0;

        damage = SkillCheck(self, target, damage);

        DamageCheck(target, damage);
    }

    private static int SkillCheck(CharacterBattle self, CharacterBattle target, int damage)
    {
        if (self.activeSkill != null)
            damage = self.activeSkill.Action(self, target, damage);

        return damage;
    }

    private static void DamageCheck(CharacterBattle target, int damage)
    {

        damage = CritMultiply(2, 2, damage);

        target.Attacked(damage);
    }

    private static int CritMultiply(int criticalChance, float damageBoost, int damage)
    {

        if (Random.Range(0, 100) < criticalChance) return (int)(damageBoost * damage);

        else return damage;
    }
}

public class SkillAction
{
    public delegate void ActionDelegate(CharacterInfo self, CharacterInfo target, int damage);

    public string Name { get; private set; }
    public int Cost { get; private set; }
    public ActionDelegate Action { get; private set; }

    public SkillAction(string name, int cost, ActionDelegate action)
    {
        Name = name;
        Cost = cost;
        Action = action;
    }
}
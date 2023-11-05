public class SkillAction
{
    public delegate int ActionDelegate(CharacterBattle self, CharacterBattle target, int damage);

    public string Name { get; private set; }
    public ActionDelegate Action { get; private set; }

    public SkillAction(string name, ActionDelegate action)
    {
        Name = name;
        Action = action;
    }
}
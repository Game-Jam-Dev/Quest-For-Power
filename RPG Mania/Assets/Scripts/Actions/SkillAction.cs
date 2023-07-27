public class SkillAction
{
    public delegate int ActionDelegate(CharacterInfo self, CharacterInfo target, int damage);

    public string Name { get; private set; }
    public ActionDelegate Action { get; private set; }

    public SkillAction(string name, ActionDelegate action)
    {
        Name = name;
        Action = action;
    }
}
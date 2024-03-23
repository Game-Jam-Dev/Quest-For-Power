using Unity.VisualScripting;

public class SkillAction
{
    public delegate int ActionDelegate(CharacterBattle self, CharacterBattle target, int damage);

    public string Name { get; private set; }
    public ActionDelegate Action { get; private set; }

    public string Description { get; private set; }

    public SkillAction(string name, ActionDelegate action, string description)
    {
        Name = name;
        Action = action;
        Description = description;
    }
}
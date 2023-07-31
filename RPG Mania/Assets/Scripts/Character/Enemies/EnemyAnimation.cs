using UnityEngine;

public class EnemyAnimation : MonoBehaviour {
    private Animator anim;
    private SpriteRenderer sr;
    private SkillList.Element element;

    public bool isAttacking = false;

    private void Start()
    {
        TryGetComponent<Animator>(out anim);
        TryGetComponent<SpriteRenderer>(out sr);

        AssignElement(element);
    }

    public void AssignElement(SkillList.Element e)
    {
        if (anim != null) anim.SetInteger("Element", (int)e);

        element = e;
    }

    private void OnEnable() {
        AssignElement(element);
    }

    public void Attacked(bool attacked)
    {
        if (anim != null)
        {
            anim.SetBool("Attacked", attacked);
        }
    }

    public void SetUpTrigger(string triggerName)
    {
        if (triggerName == "Light Attack" || triggerName == "Medium Attack" || triggerName == "Heavy Attack")
        {
            anim.SetTrigger("Attack");
            isAttacking = true;
        } else if (triggerName == "Attacked")
        {
            Attacked(true);
        }
    }

    public void ResetTrigger()
    {
        if (anim != null)
        {
            isAttacking = false;
            anim.ResetTrigger("Attack");
        }
    }

    public Animator GetAnimator() {return anim;}
}
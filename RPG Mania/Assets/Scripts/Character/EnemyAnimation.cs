using UnityEngine;

public class EnemyAnimation : MonoBehaviour {
    private Animator anim;
    private SpriteRenderer sr;

    public bool isAttacking = false;

    private void Start()
    {
        TryGetComponent<Animator>(out anim);
        TryGetComponent<SpriteRenderer>(out sr);
    }

    public void AssignElement(SkillList.Element e)
    {
        if (anim != null) anim.SetInteger("Element", (int)e);
    }

    public void Attacked(bool attacked)
    {
        if (anim != null) anim.SetBool("Attacked", attacked);
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
            anim.ResetTrigger("Attack");
            isAttacking = false;
        }
    }

    public Animator GetAnimator() {return anim;}
}
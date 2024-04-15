using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    public Animator battleAnimator;
    protected SpriteRenderer sr;
    protected bool isFighting = false;

    public string currentTrigger = "";
    //public bool isAttacking = false;
    public bool isIdle = true;

    AnimatorClipInfo[] animatorinfo;
    string current_animation;

    protected virtual void Start() {
        TryGetComponent(out battleAnimator);
        TryGetComponent(out sr);
    }

    private void Update()
    {
        if (battleAnimator != null) 
        { 
            if (CheckIfAnimation("Idle", battleAnimator) || CheckIfAnimationIsDone(battleAnimator))
            {
                ResetTrigger();
            }
        }
    }

    public void SwitchToCombat()
    {
        if (battleAnimator != null)
        {
            sr.flipX = false;
            //anim.SetBool("Combat", true);
            isFighting = true;
            isIdle = true;
        }
    }

    public void SwitchFromCombat()
    {
        //if (anim != null)
        //{
        //    anim.SetBool("Combat", false);
        //    isFighting = false;
        //} 
    }

    public void SetElement(ElementManager.Element element)
    {
        //anim.SetInteger("Element", (int)element);
        if ((int)element == 0)
        {
            battleAnimator.SetBool("Air", false);
            battleAnimator.SetBool("Earth", false);
            battleAnimator.SetBool("Fire", false);
            battleAnimator.SetBool("Water", false);
        }
        else if ((int)element == 1)
        {
            battleAnimator.SetBool("Water", true);
        }
        else if ((int)element == 2)
        {
            battleAnimator.SetBool("Fire", true);
        }
        else if ((int)element == 3)
        {
            battleAnimator.SetBool("Air", true);
        }
        else if ((int)element == 4)
        {
            battleAnimator.SetBool("Earth", true);
        }
    }

    public bool CheckIfAnimation(string stance, Animator anim)
    {
        animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
        if (animatorinfo.Length == 0)
        {
            return false;
        }
        current_animation = animatorinfo[0].clip.name;
        if (current_animation.Contains(stance))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckIfAnimationIsDone(Animator anim)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > .925)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetUpTrigger(string triggerName)
    {
        //Debug.Log(triggerName);
        if (battleAnimator != null && TriggerExists(triggerName, battleAnimator))
        {

            //if (triggerName == "Light Attack" || triggerName == "Medium Attack" || triggerName == "Heavy Attack" || triggerName == "Absorb")
            //{
            //    battleAnimator.SetTrigger("Attack");
            //    isAttacking = true;
            //}
            currentTrigger = triggerName;
            battleAnimator.SetTrigger(triggerName);
            isIdle = false;
        }
    }

    public void ResetTrigger()
    {
        if (battleAnimator != null && TriggerExists(currentTrigger, battleAnimator))
        {
            battleAnimator.ResetTrigger(currentTrigger);
            //isAttacking = false;
            isIdle = true;
        }
    }

    private bool TriggerExists(string triggerName, Animator anim) 
    {
        int hash = Animator.StringToHash(triggerName);
        for (int i = 0; i < anim.parameterCount; i++)
        {
            AnimatorControllerParameter param = anim.GetParameter(i);
            if (param.nameHash == hash && param.type == AnimatorControllerParameterType.Trigger)
                return true;
        }
        return false;
    }

    public void EscapeFromEnemy()
    {
        float escapeDistanceMultiplier = .2f;

        Transform camera = Camera.main.transform;

        Vector3 direction = camera.position - transform.position;

        transform.position += new Vector3(direction.x, 0, direction.z) * escapeDistanceMultiplier;
    }
    
    //public Animator GetAnimator() {return anim;}
}
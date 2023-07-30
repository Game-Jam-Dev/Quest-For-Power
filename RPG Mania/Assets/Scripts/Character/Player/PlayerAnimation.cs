using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    protected Camera mainCamera;
    protected Animator anim;
    protected SpriteRenderer sr;

    public string currentTrigger = "";
    public bool isAttacking = false;

    protected virtual void Start() {
        mainCamera = Camera.main;

        TryGetComponent<Animator>(out anim);
        TryGetComponent<SpriteRenderer>(out sr);
    }

    public void SwitchToCombat()
    {
        if (anim != null)
        {
            sr.flipX = false;
            anim.SetBool("Combat", true);
        }
    }

    public void SwitchFromCombat()
    {
        if (anim != null) anim.SetBool("Combat", false);
    }

    public void SetUpTrigger(string triggerName)
    {
        if (anim != null && TriggerExists(triggerName, anim))
        {
            currentTrigger = triggerName;
            isAttacking = true;
            anim.SetTrigger(currentTrigger);
        }
    }

    public void ResetTrigger()
    {
        if (anim != null)
        {
            anim.ResetTrigger(currentTrigger);
            isAttacking = false;
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
    
    public Animator GetAnimator() {return anim;}
}
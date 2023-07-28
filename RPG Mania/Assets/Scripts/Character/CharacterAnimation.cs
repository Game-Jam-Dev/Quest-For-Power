using UnityEngine;

public class CharacterAnimation : MonoBehaviour {

    protected Camera mainCamera;
    protected Animator anim;
    protected SpriteRenderer sr;

    public string currentTrigger = "";
    public bool isAttacking = false;

    protected void Start() {
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
        if (anim != null)
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
    
    public Animator GetAnimator() {return anim;}
}
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    public Animator battleAnimator;
    protected SpriteRenderer sr;
    protected bool isFighting = false;

    public string currentTrigger = "";
    public bool isAttacking = false;
    public bool isIdle = true;
    public bool needsJumping = false;
    public Vector3 direction;
    public Vector3 targetPosition;
    AnimatorClipInfo[] animatorinfo;
    string current_animation;
    float speed = 10f;
    float proportionToJump = 5 / 7;
    Vector3 jumpHeight = Vector3.forward;

    protected virtual void Start() {
        TryGetComponent(out battleAnimator);
        TryGetComponent(out sr);
    }

    private void Update()
    {
        if (battleAnimator != null) 
        { 
            if (CheckIfAnimation("Idle", battleAnimator))
            {
                ResetTrigger(currentTrigger);
                isIdle = true;
            }
        }

        //if (battleAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > proportionToJump && needsJumping &&
        //    CheckIfAnimation("Jump", battleAnimator))
        //{
        //    if (Vector3.Distance(targetPosition, transform.position) > .025)
        //    {
        //        var step = speed * Time.deltaTime;
        //        transform.position += direction * step;

        //        //if (battleAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <=  1.5f * proportionToJump)
        //        //{
        //        //    transform.position += jumpHeight * step/2.5f;
        //        //}
        //        //else
        //        //{
        //        //    transform.position -= jumpHeight * step/2.5f;
        //        //}
        //    }
        //    else
        //    {
        //        needsJumping = false;
        //    }
        //}
    }

    public void SwitchToCombat()
    {
        if (battleAnimator != null)
        {
            sr.flipX = false;
            //anim.SetBool("Combat", true);
            isFighting = true;
            isIdle = true;
            direction = Vector3.zero;
        }
    }

    public void SwitchFromCombat()
    {
        isFighting = false;
    }

    public void ToggleNormalAttack(Vector3 argumentTargetPosition, bool moveAllTheWay)
    {
        if (battleAnimator != null)
        {
            battleAnimator.SetBool("Normal Attack", !isAttacking);
            isAttacking = !isAttacking;
            //needsJumping = true;
            direction = argumentTargetPosition - transform.position;
            if (moveAllTheWay) 
            {
                targetPosition = argumentTargetPosition;
                //proportionToJump = 7 / 5;
            }
            else
            {
                direction = (argumentTargetPosition - transform.position);
                targetPosition = argumentTargetPosition - .33f * direction;
                //proportionToJump *= Vector2.Distance(transform.position, argumentTargetPosition);
            }
        }
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

    public void ResetTrigger(string triggerName)
    {
        if (battleAnimator != null && TriggerExists(triggerName, battleAnimator))
        {
            battleAnimator.ResetTrigger(triggerName);
            //isAttacking = false;
            //isIdle = true;
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
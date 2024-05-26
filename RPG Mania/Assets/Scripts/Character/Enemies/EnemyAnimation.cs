using System.Collections;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour {
    private Animator anim;
    private SpriteRenderer sr;
    private ElementManager.Element element;

    public float combatPositionHeightAdjustment = 1;
    private float combatPositionHeight, originalPositionHeight;
    public bool isAttacking = false;
    public bool isBlocking = false;
    public bool isAttacked = false;
    public bool isFighting = false;
    public bool isDying = false;
    public bool isDead = false;

    AnimatorClipInfo[] animatorinfo;
    string current_animation;    

    private void Start()
    {
        TryGetComponent(out anim);
        TryGetComponent(out sr);

            AssignElement(element);

        //originalPositionHeight = transform.position.y;
        //combatPositionHeight = originalPositionHeight + combatPositionHeightAdjustment;
    }

    private void Update()
    {
        
        if (CheckIfAnimation("ATTACK"))
        {
            isAttacking = true;
        }
        else if (isAttacking)
        {
            isAttacking = false;
            ResetTrigger("Attack");
        }
        if (CheckIfAnimation("DAMAGED"))
        {
            isAttacked = true;
        }
        else if (isAttacked)
        {
            isAttacked = false;
            ResetTrigger("Attacked");
        }
        if (CheckIfAnimation("BLOCK"))
        {
            isBlocking = true;
        }
        else if (isBlocking)
        {
            isBlocking = false;
            ResetTrigger("Block");
        }
        if (CheckIfAnimation("DEATH") && !CheckIfAnimationIsDone())
        {
            isDying = true;
        }
        else if (isDying)
        {
            isDying = false;
            anim.ResetTrigger("Dying");
            isDead = true;
        }
    }

    public bool CheckIfAnimation(string stance)
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

    public bool CheckIfAnimationIsDone()
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

    private void LateUpdate() {
        //    Transform camera = Camera.main.transform;

        //    Vector3 direction = camera.position - transform.position;

        //    if (!isFighting)
        //        direction.x = direction.z = 0;

        //    if (direction != Vector3.zero) {
        //        Quaternion lookRotation = Quaternion.LookRotation(-direction);
        //        transform.rotation = lookRotation;
        //    }
    }

    public void StartFighting()
    {
        isFighting = true;

        //transform.position = new(transform.position.x, combatPositionHeight, transform.position.z);
    }

    public void StopFighting()
    {
        isFighting = false;

        //transform.position = new(transform.position.x, originalPositionHeight, transform.position.z);
        // transform.rotation = Quaternion.LookRotation(Vector3.zero);
    }

    public void AssignElement(ElementManager.Element e)
    {
        if (anim != null && (int)e != 0) anim.SetInteger("Element", (int)e);

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

    public IEnumerator Blink()
    {
        float blinktime = 0.5f;

        sr.color = Color.clear;

        yield return new WaitForSeconds(blinktime);

        sr.color = Color.white;
    }

    public void SetUpTrigger(string triggerName)
    {
        if (triggerName == "Light Attack" || triggerName == "Medium Attack" || triggerName == "Heavy Attack")
        {
            anim.SetTrigger("Attack");
            isAttacking = true;
        } 
        else if (triggerName == "Attacked")
        {
            anim.SetTrigger("Attacked");
            isAttacked = true;
        }
        else if (triggerName == "Blocked")
        {
            anim.SetTrigger("Block");
            isBlocking = true;
        }
        else if (triggerName == "Dying")
        {
            anim.SetTrigger("Dying");
            isDying = true;
        }
        else if (triggerName == "Dead")
        {
            isDead = true;
        }
    }

    public void ResetTrigger(string triggerName)
    {
        if (anim != null)
        {
            anim.ResetTrigger(triggerName);
        }
    }

    public Animator GetAnimator() {return anim;}
}
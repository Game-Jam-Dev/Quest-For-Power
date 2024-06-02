using System;
using System.Collections;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour {
    private Animator anim;
    private SpriteRenderer sr;
    private ElementManager.Element element;

    public float combatPositionHeightAdjustment = 1;
    public bool isAttacking = false;
    public bool isBlocking = false;
    public bool isAttacked = false;
    public bool isFighting = false;
    public bool isDying = false;
    public bool isDead = false;

    AnimatorClipInfo[] animatorinfo;
    string current_animation;

    float blinktime = 0.25f;
    int numBlinks = 9;

    private string currentState;
    public float attackDuration;
    public float damagedDuration;
    public float blockDuration;
    public float deathDuration;

    private void Start()
    {
        TryGetComponent(out anim);
        TryGetComponent(out sr);

        AssignElement(element);
        UpdateAnimClipTimes();
    }

    public void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            //Debug.Log("Clip name");
            //Debug.Log(clip.name);
            if (clip.name.Contains("attack"))
            {
                attackDuration = clip.length;
                //Debug.Log(attackDuration);
            }
            else if (clip.name.Contains("damaged"))
            {
                damagedDuration = clip.length;
                //Debug.Log(damagedDuration);
            }
            else if (clip.name.Contains("block"))
            {
                blockDuration = clip.length;
                //Debug.Log(blockDuration);
            }
            else if (clip.name.Contains("death"))
            {
                deathDuration = clip.length;
                //Debug.Log(deathDuration);
            }
        }
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        //Debug.Log("Current state: " + currentState);
        //Debug.Log("New state: " + newState);

        //Debug.Log("Current animation (b4 play): " + GetCurrentAnimation());

        anim.Play(newState);

        //Debug.Log("Current animation (after play): " + GetCurrentAnimation());

        currentState = newState;
        
        if (newState.Contains("attack"))
        {
            //Debug.Log("Attack state recognized");
            //Debug.Log("Delay duration: " + attackDuration);
            isAttacking = true;
            //Debug.Log("attacking flag: " + isAttacking);

            //Debug.Log("Current animation (b4 invoke): " + GetCurrentAnimation());

            Invoke("ResetFlags", attackDuration);
        }
        else if (newState.Contains("block"))
        {
            //Debug.Log("block state recognized");
            //Debug.Log("Delay duration: " + blockDuration);
            isBlocking = true;
            //Debug.Log("blocking flag: " + isBlocking);

            //Debug.Log("Current animation (b4 invoke): " + GetCurrentAnimation());

            //delay = anim.GetCurrentAnimatorStateInfo(0).length;
            //isBlocking = true;
            Invoke("ResetFlags", blockDuration);
        }
        else if (newState.Contains("damaged"))
        {
            //Debug.Log("damaged state recognized");
            //Debug.Log("Delay duration: " + damagedDuration);
            isAttacked = true;
            //Debug.Log("attacked flag: " + isAttacked);

            //Debug.Log("Current animation (b4 invoke): " + GetCurrentAnimation());


            //delay = anim.GetCurrentAnimatorStateInfo(0).length;
            //isAttacked = true;
            Invoke("ResetFlags", damagedDuration);
        }
        else if (newState.Contains("dying"))
        {
            //Debug.Log("dying state recognized");
            //Debug.Log("Delay duration: " + deathDuration);
            isDying = true;
            //Debug.Log("blocking flag: " + isAttacked);

            //Debug.Log("Current animation (b4 invoke): " + GetCurrentAnimation());


            //delay = anim.GetCurrentAnimatorStateInfo(0).length;
            //isDying = true;
            Invoke("Dead", deathDuration);
        }
    }

    void Dead()
    {
        isDying = false;
        isDead = true;
    }

    public void ResetFlags()
    {
        isAttacking = false;
        isBlocking = false;
        isAttacked = false;
        //Debug.Log("Reseting flags");
        ChangeAnimationStateWithElement(element, "idle");
    }

    void ChangeAnimationStateWithElement(ElementManager.Element element, string newState)
    {
        if (element == ElementManager.Element.Earth)
        {
            //Debug.Log("Earth_" + newState);
            ChangeAnimationState("Earth_" + newState);
        }
        else if (element == ElementManager.Element.Fire)
        {
            //Debug.Log("Fire_" + newState);
            ChangeAnimationState("Fire_" + newState);

        }
        else if (element == ElementManager.Element.Water)
        {
            //Debug.Log("Water_" + newState);
            ChangeAnimationState("Water_" + newState);
        }
        else if (element == ElementManager.Element.Wind)
        {
            //Debug.Log("Wind_" + newState);
            ChangeAnimationState("Wind_" + newState);
        }
    }

    public void PlayAttack()
    {
        ChangeAnimationStateWithElement(element, "attack");
    }

    public void PlayDamaged()
    {
        ChangeAnimationStateWithElement(element, "damaged");
    }

    public void PlayBlock()
    {
        ChangeAnimationStateWithElement(element, "block");
    }

    public void PlayDeath()
    {
        ChangeAnimationStateWithElement(element, "death");
    }

    private string GetCurrentAnimation()
    {
        animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
        if (animatorinfo.Length == 0)
        {
            return "Not playing anything";
        }
        current_animation = animatorinfo[0].clip.name;
        return current_animation;
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

        if (anim != null && (int)element != 0)
        {
            ToggleAnimMovement(true);
        };

        //transform.position = new(transform.position.x, combatPositionHeight, transform.position.z);
    }

    public void StopFighting()
    {
        isFighting = false;
        ChangeAnimationStateWithElement(element, "idle");
        ToggleAnimMovement(false);
        //transform.position = new(transform.position.x, originalPositionHeight, transform.position.z);
        // transform.rotation = Quaternion.LookRotation(Vector3.zero);
    }

    public void AssignElement(ElementManager.Element e)
    {
        //Debug.Log(this.gameObject.name);
        //Debug.Log(e);
        element = e;
        if (anim != null && (int)element != 0)
        {
            ChangeAnimationStateWithElement(element, "idle");
            ToggleAnimMovement(false);
        };
        
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
        for (int i = 0; i < numBlinks; i++)
        {
            

            sr.color = Color.clear;

            yield return new WaitForSeconds(blinktime);

            sr.color = Color.white;
        }            
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

    public void ToggleAnimMovement(bool b)
    {
        anim.speed = b ? 1 : 0;
    }
}
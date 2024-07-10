using System.Collections.Generic;
using TMPro;
using Unity.Barracuda;
using Unity.VisualScripting;

//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static Unity.Barracuda.TextureAsTensorData;

public class PlayerCombatAnimation : MonoBehaviour {

    public Animator anim;
    protected SpriteRenderer sr;
    protected bool isFighting = false;

    public string currentTrigger = "";
    public bool needsJumping = false;
    public Vector3 direction;
    public Vector3 targetPosition;
    AnimatorClipInfo[] animatorinfo;
    string current_animation;
    public RuntimeAnimatorController combatController;

    private string currentState;

    public float damagedDuration;
    public float absorbDuration;
    public float elementalLightAttackDuration;
    public float lightAttackDuration;
    public float elementalMediumAttackDuration;
    public float mediumAttackDuration;
    public float elementaHeavyAttackDuration;
    public float heavyAttackDuration;

    public bool isAttacking = false;
    public bool isAbsorbing = false;
    public bool isIdle = true;
    public bool isAttacked = false;

    public ElementManager.Element element;
    private List<string> States = new List<string>();

    protected virtual void Start() {
        TryGetComponent(out anim);
        TryGetComponent(out sr);
    }

    public void SwitchToCombat()
    {
        if (anim != null)
        {
            anim.runtimeAnimatorController = combatController;
            isFighting = true;
            isIdle = true;
            direction = Vector3.zero;
            ChangeAnimationState("Idle");
            UpdateAnimClipTimes();
        }
    }

    public void SwitchFromCombat()
    {
        isFighting = false;
    }

    public void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            //Debug.Log("Clip name");
            //Debug.Log(clip.name);
            States.Add(clip.name);

            if (clip.name.Contains("Light"))
            {
                if (clip.name.Contains("Air") | clip.name.Contains("Earth") | clip.name.Contains("Fire") | clip.name.Contains("Water"))
                {
                    elementalLightAttackDuration = clip.length;
                }
                else
                {
                    lightAttackDuration = clip.length;
                }
            }
            else if (clip.name.Contains("Medium"))
            {
                if (clip.name.Contains("Air") | clip.name.Contains("Earth") | clip.name.Contains("Fire") | clip.name.Contains("Water"))
                {
                    elementalMediumAttackDuration = clip.length;
                }
                else
                {
                    mediumAttackDuration = clip.length;
                }
            }
            else if (clip.name.Contains("Heavy"))
            {
                if (clip.name.Contains("Air") | clip.name.Contains("Earth") | clip.name.Contains("Fire") | clip.name.Contains("Water"))
                {
                    elementaHeavyAttackDuration = clip.length;
                }
                else
                {
                    heavyAttackDuration = clip.length;
                }
            }
            else if (clip.name.Contains("Damaged"))
            {
                damagedDuration = clip.length;
                //Debug.Log(damagedDuration);
            }
            else if (clip.name.Contains("Absorb"))
            {
                absorbDuration = clip.length;
                //Debug.Log(blockDuration);
            }
            //else if (clip.name.Contains("Death"))
            //{
            //    deathDuration = clip.length;
            //    //Debug.Log(deathDuration);
            //}
        }
    }

    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        Debug.Log("Current state: " + currentState);
        Debug.Log("New state: " + newState);

        anim.Play(newState);

        currentState = newState;

        if (newState.Contains("Light_Attack"))
        {
            isAttacking = true;
            if (newState.Contains("Air") | newState.Contains("Earth") | newState.Contains("Fire") | newState.Contains("Water"))
            {
                Invoke("ResetFlags", elementalLightAttackDuration);
            }
            else
            {
                Invoke("ResetFlags", lightAttackDuration);
            }
        }
        else if (newState.Contains("Medium_Attack"))
        {
            isAttacking = true;
            if (newState.Contains("Air") | newState.Contains("Earth") | newState.Contains("Fire") | newState.Contains("Water"))
            {
                Invoke("ResetFlags", elementalMediumAttackDuration);
            }
            else
            {
                Invoke("ResetFlags", mediumAttackDuration);
            }
        }
        else if (newState.Contains("Heavy_Attack"))
        {
            isAttacking = true;
            if (newState.Contains("Air") | newState.Contains("Earth") | newState.Contains("Fire") | newState.Contains("Water"))
            {
                Invoke("ResetFlags", elementaHeavyAttackDuration);
            }
            else
            {
                Invoke("ResetFlags", heavyAttackDuration);
            }
        }

        else if (newState.Contains("Absorb"))
        {
            isAbsorbing = true;
            Invoke("ResetFlags", absorbDuration);
        }

        else if (newState.Contains("Damaged"))
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
        else if (newState.Contains("Death"))
        {
            //Debug.Log("dying state recognized");
            //Debug.Log("Delay duration: " + deathDuration);
            //isDying = true;
            //Debug.Log("blocking flag: " + isAttacked);

            //Debug.Log("Current animation (b4 invoke): " + GetCurrentAnimation());


            //delay = anim.GetCurrentAnimatorStateInfo(0).length;
            //isDying = true;
            //Invoke("Dead", deathDuration);
        }
    }

    public void ResetFlags()
    {
        isAttacking = false;
        isAttacked = false;
        //Debug.Log("Reseting flags");
        ChangeAnimationStateWithElement(element, "Idle");
    }

    public void ResetFlagsWithElement(ElementManager.Element element)
    {
        isAttacking = false;
        isAttacked = false;
        //Debug.Log("Reseting flags");
        ChangeAnimationStateWithElement(element, "Idle");
    }

    public void ChangeAnimationStateWithElement(ElementManager.Element element, string newState)
    {
        if (element == ElementManager.Element.Earth)
        {
            //Debug.Log("Earth_" + newState);
            ChangeAnimationState(newState + "_Earth");
        }
        else if (element == ElementManager.Element.Fire)
        {
            //Debug.Log("Fire_" + newState);
            ChangeAnimationState(newState + "_Fire");

        }
        else if (element == ElementManager.Element.Water)
        {
            //Debug.Log("Water_" + newState);
            ChangeAnimationState(newState + "_Water");
        }
        else if (element == ElementManager.Element.Wind)
        {
            //Debug.Log("Wind_" + newState);
            ChangeAnimationState(newState + "_Air");
        }
        else
        {
            ChangeAnimationState(newState);
        }
    }

    public void SetElement(ElementManager.Element element)
    {
        this.element = element;
        //anim.SetInteger("Element", (int)element);
         if ((int)element != 0)
        {
            ChangeAnimationStateWithElement(element, "Idle");
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
        if (anim != null && TriggerExists(triggerName, anim))
        {

            //if (triggerName == "Light Attack" || triggerName == "Medium Attack" || triggerName == "Heavy Attack" || triggerName == "Absorb")
            //{
            //    anim.SetTrigger("Attack");
            //    isAttacking = true;
            //}
            currentTrigger = triggerName;
            anim.SetTrigger(triggerName);
            isIdle = false;
        }
    }

    public void ResetTrigger(string triggerName)
    {
        if (anim != null && TriggerExists(triggerName, anim))
        {
            anim.ResetTrigger(triggerName);
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
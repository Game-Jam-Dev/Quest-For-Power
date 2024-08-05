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
    public float deathDuration;

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
            direction = Vector3.zero;
            ChangeAnimationState("idle");
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
            // Debug.Log("Clip name");
            // Debug.Log(clip.name);
            States.Add(clip.name);

            if (clip.name.Contains("light"))
            {
                if (clip.name.Contains("air") | clip.name.Contains("earth") | clip.name.Contains("fire") | clip.name.Contains("water"))
                {
                    elementalLightAttackDuration = clip.length;
                    // Debug.Log(elementalLightAttackDuration);
                }
                else
                {
                    lightAttackDuration = clip.length;
                    // Debug.Log(lightAttackDuration);
                }
            }
            else if (clip.name.Contains("medium"))
            {
                if (clip.name.Contains("air") | clip.name.Contains("earth") | clip.name.Contains("fire") | clip.name.Contains("water"))
                {
                    elementalMediumAttackDuration = clip.length;
                    // Debug.Log(elementalMediumAttackDuration);
                }
                else
                {
                    mediumAttackDuration = clip.length;
                    // Debug.Log(mediumAttackDuration);
                }
            }
            else if (clip.name.Contains("heavy"))
            {
                if (clip.name.Contains("air") | clip.name.Contains("earth") | clip.name.Contains("fire") | clip.name.Contains("water"))
                {
                    elementaHeavyAttackDuration = clip.length;
                    // Debug.Log(elementaHeavyAttackDuration);
                }
                else
                {
                    heavyAttackDuration = clip.length;
                    // Debug.Log(heavyAttackDuration);
                }
            }
            else if (clip.name.Contains("damaged"))
            {
                damagedDuration = clip.length;
                // Debug.Log(damagedDuration);
            }
            else if (clip.name.Contains("absorb"))
            {
                absorbDuration = clip.length;
                // Debug.Log(absorbDuration);
            }
            else if (clip.name.Contains("death"))
            {
                deathDuration = clip.length;
                // Debug.Log(deathDuration);
            }
            else
            {
                // Debug.Log(clip.length);
            }
        }
    }

    public float ChangeAnimationState(string newState)
    {
        if (currentState == newState & currentState != "idle") return 0;

        // Debug.Log("Current state: " + currentState);
        // Debug.Log("New state: " + newState);

        anim.Play(newState);

        currentState = newState;

        if (newState.Contains("light"))
        {
            if (newState.Contains("air") | newState.Contains("earth") | newState.Contains("fire") | newState.Contains("water"))
            {
                return elementalLightAttackDuration;
            }
            else
            {
                return lightAttackDuration;
            }
        }
        else if (newState.Contains("medium"))
        {
            if (newState.Contains("air") | newState.Contains("earth") | newState.Contains("fire") | newState.Contains("water"))
            {
                return elementalMediumAttackDuration;
            }
            else
            {
                return mediumAttackDuration;
            }
        }
        else if (newState.Contains("heavy"))
        {
            if (newState.Contains("air") | newState.Contains("earth") | newState.Contains("fire") | newState.Contains("water"))
            {
                return elementaHeavyAttackDuration;
            }
            else
            {
                return heavyAttackDuration;
            }
        }

        else if (newState.Contains("absorb"))
        {
            return absorbDuration;
        }

        else if (newState.Contains("damaged"))
        {
            return damagedDuration;
        }
        else if (newState.Contains("death"))
        {
            return deathDuration;
        }
        else if (newState.Contains("idle"))
        {
            return 0;
        }
        else
            Debug.Log("ChangeAnimationState function did not get valid input: " + newState);
            return 0;
    }

    public float ChangeAnimationStateWithElement(ElementManager.Element element, string newState)
    {
        if (element == ElementManager.Element.Earth)
        {
            //Debug.Log("Earth_" + newState);
            return ChangeAnimationState("earth_" + newState);
        }
        else if (element == ElementManager.Element.Fire)
        {
            //Debug.Log("Fire_" + newState);
            return ChangeAnimationState("fire_" + newState);

        }
        else if (element == ElementManager.Element.Water)
        {
            //Debug.Log("Water_" + newState);
            return ChangeAnimationState("water_" + newState);
        }
        else if (element == ElementManager.Element.Air)
        {
            //Debug.Log("Wind_" + newState);
            return ChangeAnimationState("air_" + newState);
        }
        else
        {
            // Debug.Log("ChangeAnimationStateWithElement - element: " + element);
            // Debug.Log("ChangeAnimationStateWithElement - new state: " + newState);
            return ChangeAnimationState(newState);
        }
    }

    public void SetElement(ElementManager.Element element)
    {
        this.element = element;
        //anim.SetInteger("Element", (int)element);
         if ((int)element != 0)
        {
            ChangeAnimationStateWithElement(element, "idle");
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

    public float GetCurrentAnimationDuration() 
    {
        return anim.GetCurrentAnimatorStateInfo(0).length;
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

    public void ResetState()
    {
        currentState = "idle";
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
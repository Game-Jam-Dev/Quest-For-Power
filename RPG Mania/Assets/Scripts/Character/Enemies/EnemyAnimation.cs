using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
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

    private string _currentAnimState;
    private Action _onAnimationFinishedHandler;
    private Dictionary<string, AnimationClip> _animationClips = new(); 
        
    private void Start()
    {
        TryGetComponent(out anim);
        TryGetComponent(out sr);

        ReadClips();

        AssignElement(element);
    }

    public void ReadClips()
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            _animationClips.Add(CreateStateNameFromClipName(clip.name), clip);
        }
    }

    private string CreateStateNameFromClipName(string clipName)
    {
        var tokens = clipName.Split("_");
        if (tokens.Length == 3)
        {
            return tokens[2].ToLower() + "_" + tokens[1].ToLower();
        }

        Debug.LogWarning("clip with unknown name format found "+clipName);
        return clipName;
    }

    void ChangeAnimationState(string newState)
    {
        if (_currentAnimState == newState) return;

        if (PlayAnimation(newState, HandleAnimationFinished))
        {
            if (newState.Contains("attack"))
            {
                isAttacking = true;
            }
            else if (newState.Contains("damaged"))
            {
                isAttacked = true;
            }
            else if (newState.Contains("block"))
            {
                isBlocking = true;
            }
            else if (newState.Contains("dying"))
            {
                isDying = true;
            }
        }
    }

    private void HandleAnimationFinished()
    {
        Debug.Log(gameObject.name+ ": animation finished "+_currentAnimState);
        if (isDying)
        {
            isDying = false;
            isDead = true;
        }
        else
        {
            ResetFlags();
        }     
    }
    
    public void ResetFlags()
    {
        isAttacking = false;
        isBlocking = false;
        isAttacked = false;
        ChangeAnimationStateWithElement(element, "idle");
    }

    private bool PlayAnimation(string newAnimStateName, Action onFinish)
    {
        if (_animationClips.TryGetValue(newAnimStateName, out AnimationClip clip))
        {
            if (_onAnimationFinishedHandler != null)
            {
                CancelInvoke("OnAnimationFinished");
                _onAnimationFinishedHandler = null;
            }
        
            Debug.Log(gameObject.name+ ": change animation to "+newAnimStateName);
        
            _onAnimationFinishedHandler = clip.isLooping ? null : onFinish;

            anim.Play(newAnimStateName);
            _currentAnimState = newAnimStateName;

            if (_onAnimationFinishedHandler != null)
            {
                Invoke("OnAnimationFinished", clip.length);
            }
            return true;
        }

        Debug.LogError("no clip found for animation state "+newAnimStateName);
        return false;
    }

    //called by unity after when the animation is finished. (Implemented with the invoke Methode)
    private void OnAnimationFinished()
    {
        _onAnimationFinishedHandler.Invoke();
        _onAnimationFinishedHandler = null;
    }
    
    void ChangeAnimationStateWithElement(ElementManager.Element newElement, string newState)
    {
        ChangeAnimationState(newElement.ToString().ToLower()+"_"+newState);
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

    
    public void StartFighting()
    {
        isFighting = true;

        if (anim != null && (int)element != 0)
        {
            ToggleAnimMovement(true);
            ChangeAnimationStateWithElement(element, "idle");
        }

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
        }
    }

    private void OnEnable()
    {
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

    public Animator GetAnimator()
    {
        return anim;
    }

    public void ToggleAnimMovement(bool b)
    {
        anim.speed = b ? 1 : 0;
    }
}
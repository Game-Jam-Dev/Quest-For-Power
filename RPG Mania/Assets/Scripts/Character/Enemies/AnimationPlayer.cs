using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationPlayer : MonoBehaviour
{
    public AnimatorController animatorController;
    
    private Animator anim;
    private SpriteRenderer sr;
    private ElementManager.Element element;

    AnimatorClipInfo[] animatorinfo;
    string current_animation;

    float blinktime = 0.25f;
    int numBlinks = 9;

    private string _currentAnimState;
    private Action _onAnimationFinishedHandler;
    private Dictionary<string, AnimationClip> _animationClips = new(); 
        
    private void Awake()
    {
        TryGetComponent(out anim);
        TryGetComponent(out sr);

        ReadClips();
        
        anim.runtimeAnimatorController = animatorController;
    }

    public void ReadClips()
    {
        foreach (var state in animatorController.layers[0].stateMachine.states)
        {
            _animationClips.Add(state.state.name, (AnimationClip)state.state.motion);
        }
    }
    
    public void PlayAnimation(string newAnimStateName)
    {
        PlayAnimation(newAnimStateName, () => {});
    }
    
    public void PlayAnimation(string newAnimStateName, Action onFinish)
    {
        if (!_animationClips.TryGetValue(newAnimStateName, out AnimationClip clip))
        {
            Debug.LogError("no clip found for animation state " + newAnimStateName);
            return;
        }

        if (_currentAnimState == newAnimStateName && clip.isLooping) return;
        
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
    }

    // public IEnumerator Blink()
    // {
    //     for (int i = 0; i < numBlinks; i++)
    //     {
    //         sr.color = Color.clear;
    //
    //         yield return new WaitForSeconds(blinktime);
    //
    //         sr.color = Color.white;
    //     }
    // }

    public void ToggleAnimMovement(bool b)
    {
        anim.speed = b ? 1 : 0;
    }
    
    //called by unity after when the animation is finished. (Implemented with the invoke Methode)
    private void OnAnimationFinished()
    {
        _onAnimationFinishedHandler.Invoke();
        _onAnimationFinishedHandler = null;
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
}
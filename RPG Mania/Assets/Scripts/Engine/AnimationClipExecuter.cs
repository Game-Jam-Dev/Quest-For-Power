using System;
using System.Collections;
using System.Collections.Generic;
using Engine;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationClipExecuter : MonoBehaviour
{
    public static int NonLoopedPlayingAnimCount;
    
    public AnimatorController animatorController;
    
    private Animator _anim;

    private string _currentAnimState;
    private Action _onAnimationFinishedHandler;
    private Dictionary<string, AnimationClip> _animationClips = new();

    private ActiveMarker _noneLoopingAnimMarker = new (() => NonLoopedPlayingAnimCount++, () => NonLoopedPlayingAnimCount--);

    private void Awake()
    {
        TryGetComponent(out _anim);

        ReadClips();
        
        _anim.runtimeAnimatorController = animatorController;
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

        _noneLoopingAnimMarker.Cancel();

        // Debug.Log(gameObject.name+ ": change animation to "+newAnimStateName);
    
        _onAnimationFinishedHandler = clip.isLooping ? null : onFinish;

        _anim.Play(newAnimStateName);
        _currentAnimState = newAnimStateName;

        _noneLoopingAnimMarker.Set(!clip.isLooping);
        
        Debug.Log("starting NonLoopedPlayingAnimCount "+NonLoopedPlayingAnimCount);
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
    
    private void OnAnimationFinished()
    {
        _onAnimationFinishedHandler.Invoke();
        _onAnimationFinishedHandler = null;

        _noneLoopingAnimMarker.Cancel();
    }
}
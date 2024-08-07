using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(AnimationClipExecuter))]
public class CharacterAnimation : MonoBehaviour
{
    private AnimationClipExecuter _animClipExecuter;
    
    private ElementManager.Element _element;

    private void Awake()
    {
        TryGetComponent(out _animClipExecuter);
    }
    
    public void PlayAttack()
    {
        _animClipExecuter.PlayAnimation(ConcatElementAnimState(_element, "attack"), () => Switch2Idle());
    }

    public void PlayDamaged()
    {
        _animClipExecuter.PlayAnimation(ConcatElementAnimState(_element, "damaged"), () => Switch2Idle());
    }

    public void PlayBlock()
    {
        _animClipExecuter.PlayAnimation(ConcatElementAnimState(_element, "block"), () => Switch2Idle());
    }

    public void PlayDeath()
    {
        _animClipExecuter.PlayAnimation(ConcatElementAnimState(_element, "death"));
    }
    
    public void PlayElementBasedCustomThanIdle(string animState)
    {
        _animClipExecuter.PlayAnimation(ConcatElementAnimState(_element, animState), () => Switch2Idle());
    }

    public void PlayCustomThanIdle(string animState)
    {
        _animClipExecuter.PlayAnimation(animState, () => Switch2Idle());
    }

    public void Switch2Idle()
    {
        _animClipExecuter.PlayAnimation(ConcatElementAnimState(_element, "idle"));
    }
    
    public void AssignElement(ElementManager.Element e)
    {
        _element = e;
        Switch2Idle();
    }
    
    private string ConcatElementAnimState(ElementManager.Element newElement, string baseState)
    {
        return newElement.ToString().ToLower()+"_"+baseState;
    }
}
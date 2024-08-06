using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(AnimationPlayer))]
public class EnemyAnimation : MonoBehaviour
{
    private AnimationPlayer _animPlayer;
    
    private ElementManager.Element _element;

    private void Awake()
    {
        TryGetComponent(out _animPlayer);

        AssignElement(_element);
    }
    
    public void PlayAttack()
    {
        _animPlayer.PlayAnimation(ConcatElementAnimState(_element, "attack"), () => Switch2Idle());
    }

    public void PlayDamaged()
    {
        _animPlayer.PlayAnimation(ConcatElementAnimState(_element, "damaged"), () => Switch2Idle());
    }

    public void PlayBlock()
    {
        _animPlayer.PlayAnimation(ConcatElementAnimState(_element, "block"), () => Switch2Idle());
    }

    public void PlayDeath()
    {
        _animPlayer.PlayAnimation(ConcatElementAnimState(_element, "death"));
    }
    
    public void Switch2Idle()
    {
        _animPlayer.PlayAnimation(ConcatElementAnimState(_element, "idle"));
    }

    public void StartFighting()
    {
        if (_animPlayer != null && (int)_element != 0)
        {
            ToggleAnimMovement(true);
            Switch2Idle();
        }

        //transform.position = new(transform.position.x, combatPositionHeight, transform.position.z);
    }

    public void StopFighting()
    {
        Switch2Idle();
        ToggleAnimMovement(false);
        //transform.position = new(transform.position.x, originalPositionHeight, transform.position.z);
        // transform.rotation = Quaternion.LookRotation(Vector3.zero);
    }

    public void AssignElement(ElementManager.Element e)
    {
        //Debug.Log(this.gameObject.name);
        //Debug.Log(e);
        _element = e;
        if (_animPlayer != null && (int)_element != 0)
        {
            Switch2Idle();
            ToggleAnimMovement(false);
        }
    }

    private void OnEnable()
    {
        AssignElement(_element);
    }
    
    public void ToggleAnimMovement(bool b)
    {
        _animPlayer.ToggleAnimMovement(b);
    }
    
    private string ConcatElementAnimState(ElementManager.Element newElement, string baseState)
    {
        return newElement.ToString().ToLower()+"_"+baseState;
    }
}
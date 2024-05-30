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

    float blinktime = 0.25f;
    int numBlinks = 9;

    //Animation states
    
    const string EARTH_IDLE = "Earth_idle";
    const string FIRE_IDLE = "Fire_idle";
    const string WATER_IDLE = "Water_idle";
    const string WIND_IDLE = "Wind_idle";

    const string EARTH_DAMAGED = "Earth_damaged";
    const string FIRE_DAMAGED = "Fire_damaged";
    const string WATER_DAMAGED = "Water_damaged";
    const string WIND_DAMAGED = "Wind_damaged";

    const string EARTH_ATTACK = "Earth_attack";
    const string FIRE_ATTACK = "Fire_attack";
    const string WATER_ATTACK = "Water_attack";
    const string WIND_ATTACK = "Wind_attack";

    const string EARTH_BLOCK = "Earth_block";
    const string FIRE_BLOCK = "Fire_block";
    const string WATER_BLOCK = "Water_block";
    const string WIND_BLOCK = "Wind_block";

    const string EARTH_DEATH = "Earth_death";
    const string FIRE_DEATH = "Fire_death";
    const string WATER_DEATH = "Water_death";
    const string WIND_DEATH = "Wind_death";

    private string currentState;
    float delay;

    private void Start()
    {
        TryGetComponent(out anim);
        TryGetComponent(out sr);

            AssignElement(element);

        //originalPositionHeight = transform.position.y;
        //combatPositionHeight = originalPositionHeight + combatPositionHeightAdjustment;
    }

    //private void Update()
    //{
        
    //    if (CheckIfAnimation("ATTACK"))
    //    {
    //        isAttacking = true;
    //    }
    //    else if (isAttacking)
    //    {
    //        isAttacking = false;
    //        ResetTrigger("Attack");
    //    }
    //    if (CheckIfAnimation("DAMAGED"))
    //    {
    //        isAttacked = true;
    //    }
    //    else if (isAttacked)
    //    {
    //        isAttacked = false;
    //        ResetTrigger("Attacked");
    //    }
    //    if (CheckIfAnimation("BLOCK"))
    //    {
    //        isBlocking = true;
    //    }
    //    else if (isBlocking)
    //    {
    //        isBlocking = false;
    //        ResetTrigger("Block");
    //    }
    //    if (CheckIfAnimation("DEATH") && !CheckIfAnimationIsDone())
    //    {
    //        isDying = true;
    //    }
    //    else if (isDying)
    //    {
    //        isDying = false;
    //        anim.ResetTrigger("Dying");
    //        isDead = true;
    //    }
    //}

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        Debug.Log(newState);
        Debug.Log(newState.Contains("attack"));

        anim.Play(newState);

        Debug.Log(GetCurrentAnimation());

        currentState = newState;
        
        if (newState.Contains("attack"))
        {
            Debug.Log("what");
            delay = anim.GetCurrentAnimatorStateInfo(0).length;
            Debug.Log(delay);
            isAttacking = true;
            Debug.Log("attacking flag");
            Debug.Log(isAttacking);

            Debug.Log(GetCurrentAnimation());

            Invoke("ResetFlags", delay);
        }
        else if (newState.Contains("block"))
        {
            delay = anim.GetCurrentAnimatorStateInfo(0).length;
            isBlocking = true;
            Invoke("ResetFlags", delay);
        }
        else if (newState.Contains("damaged"))
        {
            delay = anim.GetCurrentAnimatorStateInfo(0).length;
            isAttacked = true;
            Invoke("ResetFlags", delay);
        }
        else if (newState.Contains("dying"))
        {
            delay = anim.GetCurrentAnimatorStateInfo(0).length;
            isDying = true;
            Invoke("Dead", delay);
        }
    }

    void Dead()
    {
        isDying = false;
        isDead = true;
    }

    void ResetFlags()
    {
        isAttacking = false;
        isBlocking = false;
        isAttacked = false;
        Debug.Log("Reset flags");
        ChangeAnimationStateWithElement(element, "idle");
    }

    void ChangeAnimationStateWithElement(ElementManager.Element element, string newState)
    {
        if (element == ElementManager.Element.Earth)
        {
            ChangeAnimationState("Earth_" + newState);
        }
        else if (element == ElementManager.Element.Fire)
        {
            ChangeAnimationState("Fire_" + newState);

        }
        else if (element == ElementManager.Element.Water)
        {
            ChangeAnimationState("Water_" + newState);
        }
        else if (element == ElementManager.Element.Wind)
        {
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
        element = e;

        if (anim != null && (int)e != 0)
        {
            ChangeAnimationStateWithElement(e, "idle");
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
}
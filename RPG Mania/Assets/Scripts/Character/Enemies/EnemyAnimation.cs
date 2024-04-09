using UnityEngine;

public class EnemyAnimation : MonoBehaviour {
    private Animator anim;
    private SpriteRenderer sr;
    private ElementManager.Element element;

    public float combatPositionHeightAdjustment = 1;
    private float combatPositionHeight, originalPositionHeight;
    public bool isAttacking = false;
    public bool isFighting = false;

    private void Start()
    {
        TryGetComponent(out anim);
        TryGetComponent(out sr);

        if (element != null) 
        {
            AssignElement(element);
        }        

        //originalPositionHeight = transform.position.y;
        //combatPositionHeight = originalPositionHeight + combatPositionHeightAdjustment;
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
        if (anim != null & e != null) anim.SetInteger("Element", (int)e);

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

    public void SetUpTrigger(string triggerName)
    {
        if (triggerName == "Light Attack" || triggerName == "Medium Attack" || triggerName == "Heavy Attack")
        {
            anim.SetTrigger("Attack");
            isAttacking = true;
        } else if (triggerName == "Attacked")
        {
            Attacked(true);
        }
    }

    public void ResetTrigger()
    {
        if (anim != null)
        {
            isAttacking = false;
            anim.ResetTrigger("Attack");
        }
    }

    public Animator GetAnimator() {return anim;}
}
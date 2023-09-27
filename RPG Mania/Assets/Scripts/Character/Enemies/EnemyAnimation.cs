using UnityEngine;

public class EnemyAnimation : MonoBehaviour {
    private Animator anim;
    private SpriteRenderer sr;
    private SkillList.Element element;

    public float combatPositionHeightAdjustment = 1;
    private float combatPositionHeight;
    public bool isAttacking = false;
    public bool isFighting = false;

    private void Start()
    {
        TryGetComponent(out anim);
        TryGetComponent(out sr);

        AssignElement(element);

        combatPositionHeight = transform.position.y + combatPositionHeightAdjustment;
    }

    private void LateUpdate() {
        Transform camera = Camera.main.transform;

        Vector3 direction = camera.position - transform.position;

        if (!isFighting)
            direction.x = direction.z = 0;

        if (direction != Vector3.zero) {
            Quaternion lookRotation = Quaternion.LookRotation(-direction);
            transform.rotation = lookRotation;
        }
    }

    public void StartFighting()
    {
        isFighting = true;

        transform.position = new(transform.position.x, combatPositionHeight, transform.position.z);
    }

    public void AssignElement(SkillList.Element e)
    {
        if (anim != null) anim.SetInteger("Element", (int)e);

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
using UnityEngine;

public class EnemyAnimation : MonoBehaviour {
    private Animator anim;
    private SpriteRenderer sr;

    private void Start() {
        TryGetComponent<Animator>(out anim);
        TryGetComponent<SpriteRenderer>(out sr);
    }

    public void AssignElement(SkillList.Element e)
    {
        if (anim != null) anim.SetInteger("Element", (int)e);
    }
}
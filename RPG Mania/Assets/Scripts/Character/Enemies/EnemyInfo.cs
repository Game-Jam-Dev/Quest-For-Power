using UnityEngine;

public class EnemyInfo : CharacterInfo {
    public int level = 1;
    public float detectRange = .5f;
    protected WildsManager wildsController;
    protected GameObject player;
    protected bool isAttacking;

    [SerializeField] protected EnemyAnimation ea;

    protected override void Start()
    {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player");
        wildsController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<WildsManager>();
    }

    public override ComboAction PickEnemyCombo(int currentComboLength)
    {
        switch (combo - currentComboLength)
        {
            case 1:
            return GetCombo(0);
            case 2:
            return GetCombo(Random.Range(0,2));
            default:
            return GetCombo(Random.Range(0,3));
        }
    }

    public override void SetUpTrigger(string triggerName)
    {
        ea.SetUpTrigger(triggerName);
    }

    public override void Recover()
    {
        ea.Attacked(false);
    }

    public override bool GetIsAttacking()
    {
        return ea.isAttacking;
    }

    public override Animator GetAnimator()
    {
        return ea.GetAnimator();
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            wildsController.EncounterEnemy(gameObject);
        }
    }

    public int XPFromKill(int playerLevel)
    {
        int xp = 5;

        return xp;
    }
}
using UnityEngine;

public class EnemyInfo : CharacterInfo {
    public int level = 1;
    public float detectRange = .5f;
    public bool isAlive = true;
    public int id = 0;
    public string scene = "";
    protected WildsManager wildsManager;
    protected GameObject player;
    protected bool isAttacking;

    [SerializeField] protected EnemyAnimation ea;

    protected override void Start()
    {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player");
        wildsManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<WildsManager>();
    }

    public void InitializeEnemy(int id)
    {
        this.id = id;
        isAlive = GameManager.instance.CheckEnemyDeath(scene, id);
    }

    public override void PrepareCombat()
    {
        level = GameManager.instance.GetPlayerLevel();
        
        SetStats();
        ea.StartFighting();
    }

    protected virtual void SetStats(){}

    public override void Kill()
    {
        isAlive = false;
        GameManager.instance.SetEnemyDeath(scene, id);
        Destroy(gameObject);
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

    public override void SetAnimationTrigger(string triggerName)
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

    protected virtual void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Find the rotation for the enemy to face the player

            float rotationZ;
            Vector3 distance = other.transform.position - transform.position;
            
            if (Mathf.Abs(distance.x) > Mathf.Abs(distance.z))
            {
                if (distance.x > 0) rotationZ = 90;

                else rotationZ = 270;
            } 
            else 
            {
                if (distance.z > 0) rotationZ = 180;

                else rotationZ = 0; 
            }

            wildsManager.EncounterEnemy(gameObject, rotationZ);
        }
    }

    public void SetRotationForBattleCamera()
    {
        ea.StartFighting();
    }

    public int XPFromKill(int playerLevel)
    {
        int xp = 5;

        return xp;
    }
}
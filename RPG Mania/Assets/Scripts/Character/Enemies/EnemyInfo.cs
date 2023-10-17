using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyInfo : CharacterInfo {
    public int level = 1;
    public float detectRange = .5f;
    private bool isAlive = true;
    private int id = 0;
    private int scene = 0;
    protected WildsManager wildsManager;
    protected GameObject player;
    protected bool isAttacking;

    [SerializeField] protected EnemyAnimation ea;
    [SerializeField] private TextMeshPro damageDisplay;

    protected override void Start()
    {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player");
        wildsManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<WildsManager>();
    }

    public void InitializeEnemy(int id)
    {
        this.id = id;
        scene = SceneManager.GetActiveScene().buildIndex;
        isAlive = GameManager.instance.CheckEnemyDeath(scene, id);
    }

    public override void PrepareCombat()
    {
        level = GameManager.instance.GetPlayerLevel();
        
        SetStats();
        damageDisplay.text = maxHealth.ToString();
        damageDisplay.enabled = true;
        ea.StartFighting();
    }

    protected virtual void SetStats(){}

    public override void Kill()
    {
        isAlive = false;
        GameManager.instance.SetEnemyDeath(scene, id);
        Destroy(gameObject);
    }

    public override void Attacked(int damage)
    {
        base.Attacked(damage);

        damageDisplay.text = health.ToString();
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

    public bool GetIsAlive()
    {
        return isAlive;
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
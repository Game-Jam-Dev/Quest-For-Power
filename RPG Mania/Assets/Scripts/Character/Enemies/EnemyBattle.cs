using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyBattle : CharacterBattle {

    [SerializeField]
    GameObject FirstAttack, SecondAttack, ThirdAttack, TotalCost, ShieldUIImage, ShieldUIValue;

    public int level = 1;
    public float detectRange = .75f;
    private bool isAlive = true;
    private int id = 0;
    private int scene = 0;
    protected WildsManager wildsManager;
    protected GameObject player;
    protected bool isAttacking;

    [SerializeField] protected EnemyAnimation ea;
    private EnemyBattleManager enemyBattleManager;
    private GameObject cursorDisplay;

    public bool itemDrops = true;
    public Item itemDrop;

    public int absorbs = 0;

    [SerializeField] GameObject comboOrder;

    public int firstComboValue = 1;
    public int secondComboValue = 2;
    public int thirdComboValue = 3;
    [SerializeField] public int shieldDefault;
    public int currentShieldsValue;
    public bool stunned = false;
    public int stunCount = 0;
    [SerializeField] public int stunLimit;
    [SerializeField] public Sprite ShieldSprite, BrokenShieldSprite;
    public int defaultDefense;
    float currentTime;

    bool inCombat = false;
    public float resetTimer = 1f;

    protected override void Start()
    {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player");
        if (GameObject.FindGameObjectWithTag("Canvas").GetComponent<WildsManager>() != null ) 
        {
            wildsManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<WildsManager>();
        }        
        cursorDisplay = transform.GetChild(0).GetChild(0).gameObject;
        cursorDisplay.SetActive(false);
        ResetShields();

        if (itemDrops) itemDrop = ItemManager.GetInstance().GetRandomItem();

    }

    private void Update()
    {
        if (currentTime != null && currentTime != 0)
        {
            if (Time.time >= currentTime + 5)
            {
                currentTime = 0;
                this.GetComponent<CapsuleCollider>().enabled = true;
            }
        }
    }

    public void InitializeEnemy(int id)
    {
        this.id = id;
        scene = SceneManager.GetActiveScene().buildIndex;
        isAlive = GameManager.instance.CheckEnemyDeath(scene, id);

        GenerateComboWeakness();
        defaultDefense = defense;

        if (!isAlive)
        {
            Defeated();
            Kill();
        }
    }

    public void ResetShields()
    {
        currentShieldsValue = shieldDefault;
        stunned = false;
        ShieldUIValue.GetComponent<TextMeshProUGUI>().text = currentShieldsValue.ToString();
        ShieldUIImage.GetComponent<Image>().sprite = ShieldSprite;
    }

    public void ReduceShields()
    {
        currentShieldsValue--;
        if (currentShieldsValue == 0)
        {
            ToggleStun();
        }
        else if (currentShieldsValue >= 1)
        {
            ShieldUIValue.GetComponent<TextMeshProUGUI>().text = currentShieldsValue.ToString();
        }
    }

    public void ToggleStun() 
    { 
        stunned = !stunned;
        if (stunned)
        {
            defense = Mathf.RoundToInt(defaultDefense * .7f);
            stunCount = 0;
            ShieldUIImage.GetComponent<Image>().sprite = BrokenShieldSprite;
            ShieldUIValue.GetComponent<TextMeshProUGUI>().text = "";
        }
        else
        {
            defense = defaultDefense;
            ResetShields();
        }
    }

    public void ChangeStunCounter()
    {
        stunCount++;
        if (stunLimit < stunCount) 
        {
            ToggleStun();
        }
    }

    public void GenerateUIQuestionMarkers()
    {
        FirstAttack.GetComponent<TextMeshProUGUI>().text = "?";  
        SecondAttack.GetComponent<TextMeshProUGUI>().text = "?";
        ThirdAttack.GetComponent<TextMeshProUGUI>().text = "?";
        TotalCost.GetComponent<TextMeshProUGUI>().text = "?";
    }

    public void GenerateComboWeakness()
    {
        int positionRevealed = Random.Range(0, 3);
        firstComboValue = Random.Range(1, 4);
        secondComboValue = Random.Range(1, 4);
        thirdComboValue = Random.Range(1, 4);
        if (positionRevealed == 0)
        {
            FirstAttack.GetComponent<TextMeshProUGUI>().text = firstComboValue.ToString();
        }
        else if (positionRevealed == 1)
        {
            SecondAttack.GetComponent<TextMeshProUGUI>().text = secondComboValue.ToString();
        }
        else
        {
            ThirdAttack.GetComponent<TextMeshProUGUI>().text = thirdComboValue.ToString();
        }
        TotalCost.GetComponent<TextMeshProUGUI>().text = (firstComboValue + secondComboValue + thirdComboValue).ToString();
    }

    public override void PrepareCombat()
    {
        level = GameManager.instance.GetPlayerLevel();
        absorbs = 0;
        SetStats();
        ea.StartFighting();
        comboOrder.SetActive(true);
        ShieldUIImage.SetActive(true);
        ToggleCollider(false);
    }

    public void ResetFromFight()
    {
        gameObject.SetActive(true);
        ea.StopFighting();
        comboOrder.SetActive(false);
        ShieldUIImage.SetActive(false);

        StartCoroutine(ResetCombat());
    }

    private IEnumerator ResetCombat()
    {
        yield return new WaitForSeconds(resetTimer);
        ToggleCollider(true);
    }

    protected virtual void SetStats(){}

    public override void Kill()
    {
        isAlive = false;
        GameManager.instance.SetEnemyDeath(scene, id);
        //Destroy(gameObject);
    }

    public override void Defeated()
    {
        //gameObject.SetActive(false);
        ea.PlayDeath();
        comboOrder.SetActive(false);
        ShieldUIImage.SetActive(false);
        this.GetComponent<CapsuleCollider>().enabled = false;
    }

    // uncomment if enemy takes damage incorrectly
    // public override void Attacked(int damage)
    // {
    //     base.Attacked(damage);
    // }

    public override ComboAction PickEnemyCombo(int currentComboLength)
    {
        switch (combo - currentComboLength)
        {
            case 1:
            return GetCombo(0);
            case 2:
            return GetCombo(Random.Range(0, 2));
            default:
            return GetCombo(Random.Range(0, 3));
        }
    }

    public void ResetAnimationTags()
    {
        ea.ResetFlags();
    }

    public override void SetAnimationTrigger(string triggerName)
    {
        ea.SetUpTrigger(triggerName);
    }

    public void PlayAttackedAnimation()
    {
        ea.PlayDamaged();
    }

    public void PlayAttackAnimation()
    {
        ea.PlayAttack();
    }

    public void PlayBlockAnimation()
    {
        ea.PlayBlock();
    }

    public void PlayDeathAnimation()
    {
        ea.PlayDeath();
    }

    ////public override void Recover()
    ////{
    ////    ea.Attacked(false);
    ////}

    public bool GetIsReady()
    {
        if (!ea.isAttacking && !ea.isBlocking && !ea.isAttacked && !ea.isDying)
            return true;
        else if (ea.isDead)
            return true;
        else return false;
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

    public GameObject ConnectUI()
    {
        return cursorDisplay;
    }

    protected virtual void OnTriggerEnter(Collider other) 
    {
        // if (inCombat) return;

        Scene sceneObject = SceneManager.GetActiveScene();
        if (other.gameObject.CompareTag("Player") && isAlive && !ea.isDead && sceneObject.name != "Throne Room")
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

    virtual public int XPFromKill(int playerLevel)
    {
        int xp = 5;

        return xp;
    }

    public Item ItemDrop()
    {
        return itemDrop;
    }

    public void IncreaseAbsorbs()
    {
        absorbs++;
    }
    public void ToggleCollider(bool b)
    {
        GetComponent<CapsuleCollider>().enabled = b;
    }
}
using UnityEngine;

public class EnemyInfo : CharacterInfo {
    public int level = 1;
    public float detectRange = .5f;
    private WorldManager worldController;
    protected GameObject player;

    [SerializeField] protected EnemyAnimation ea;

    protected override void Start()
    {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player");
        worldController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<WorldManager>();
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

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            worldController.EncounterEnemy(gameObject);
        }
    }

    public int XPFromKill(int playerLevel)
    {
        int xp = 5;

        return xp;

        // int levelBonus = (level - playerLevel) * 2;

        // return Mathf.Max(xp + levelBonus, 0);
    }
}
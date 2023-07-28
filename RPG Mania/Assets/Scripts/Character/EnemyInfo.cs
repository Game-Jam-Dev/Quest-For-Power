using UnityEngine;

public class EnemyInfo : CharacterInfo {
    public int level = 1;
    public float detectRange = .5f;
    private WorldManager worldController;
    private Collider col;

    protected override void Start()
    {
        base.Start();

        worldController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<WorldManager>();
        col = GetComponent<CapsuleCollider>();
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

    public override void PrepareCombat()
    {
        col.enabled = false;

        base.PrepareCombat();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            worldController.EncounterEnemy(gameObject);
        }
    }

    public int XPFromKill(int playerLevel)
    {
        int xp = 2;

        int levelBonus = (level - playerLevel) * 2;

        return Mathf.Max(xp + levelBonus, 0);
    }
}
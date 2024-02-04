using UnityEngine;

public class HideEnemies : MonoBehaviour {
    [SerializeField] private GameObject enemy1, enemy2;
    public Vector3 enemy1Location, enemy2Location;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            enemy1.transform.position = enemy1Location;
            enemy2.transform.position = enemy2Location;

            enemy2.GetComponent<SpriteRenderer>().flipX = false;

            enemy1.SetActive(false);
            enemy2.SetActive(false);

            Destroy(gameObject);
        }
    }
}
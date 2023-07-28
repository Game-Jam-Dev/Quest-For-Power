using UnityEngine;

public class CharacterAnimationWilds : CharacterAnimation {
    private void LateUpdate() {
        Vector3 direction = mainCamera.transform.position - transform.position;
        direction.x = direction.z = 0;

        if (direction != Vector3.zero) {
            Quaternion lookRotation = Quaternion.LookRotation(-direction);
            transform.rotation = lookRotation;
        }
    }
}
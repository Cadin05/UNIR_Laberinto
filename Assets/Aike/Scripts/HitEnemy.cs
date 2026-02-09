using UnityEngine;
using UnityEngine.InputSystem;

public class HitEnemy : MonoBehaviour
{
    [SerializeField] float maxPushDistance = 3f;
    [SerializeField] float pushForce = 300f;
    [SerializeField] LayerMask layerMask = Physics.DefaultRaycastLayers;
    [SerializeField] private Camera playerCamera;

    private bool mustApplyForce;
    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            mustApplyForce = true;

        }
    }
    void FixedUpdate()
    {
        if (mustApplyForce)
        {
            mustApplyForce = false;
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxPushDistance, layerMask))
            {
                
                if (hit.collider.CompareTag("Enemy"))
                {
                    Rigidbody otherObjectRB = hit.rigidbody;
                    EnemyFollow enemyFollow = hit.collider.GetComponent<EnemyFollow>();
                    if ((otherObjectRB != null) && (enemyFollow != null))
                    {
                        otherObjectRB.AddForce(transform.forward * pushForce, ForceMode.Impulse);
                        enemyFollow.Hurt();
                    }
                    
                }

            }
        }
    }
}

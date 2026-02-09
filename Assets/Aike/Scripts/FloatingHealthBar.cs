using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    public Transform target;        // Assign the enemy object
    public Camera camera;
    
    public Slider slider;

    
    void LateUpdate()
    {
        if (target != null)
        {
            transform.LookAt(camera.transform.position);
        }
    }
    
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (slider != null)
            slider.value = currentHealth / maxHealth;
    }
}

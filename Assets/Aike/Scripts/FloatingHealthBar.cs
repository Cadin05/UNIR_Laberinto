using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    public Transform target;        // Assign the enemy object
    public Camera cam;
    
    public Slider slider;

    
    void LateUpdate()
    {
        if (target != null)
        {
            transform.LookAt(cam.transform.position);
        }
    }
    
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (slider != null)
            slider.value = currentHealth / maxHealth;
    }
}

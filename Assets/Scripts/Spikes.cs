using UnityEngine;

public class Spikes : MonoBehaviour, IRemotelyActivatable
{
    public void OnRemoteActivation()
    {
        foreach (Animator anim in GetComponentsInChildren<Animator>())
        {
            anim.SetTrigger("Activated");
        }
    }
}

using UnityEngine;

public class PlayParticleSystem : MonoBehaviour
{
    ParticleSystem particleSystemComp;

    private void Awake()
    {
        particleSystemComp = GetComponentInChildren<ParticleSystem>();
    }

    public void Play()
    {
        particleSystemComp.Play();
    }
}

using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public ParticleSystem ps;

    void OnEnable()
    {
        PlayerController.onThrustChange += SetParticleSystem;
    }

    void SetParticleSystem(bool play)
    {
        if (play)
            ps.Play();
        else
            ps.Stop();
    }

    void OnDisable()
    {
        PlayerController.onThrustChange -= SetParticleSystem;
    }
}
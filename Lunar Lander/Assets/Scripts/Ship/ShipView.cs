using UnityEngine;

public class ShipView : MonoBehaviour
{
    public ParticleSystem ps;

    void OnEnable()
    {
        ShipController.onThrustChange += SetParticleSystem;
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
        ShipController.onThrustChange -= SetParticleSystem;
    }
}
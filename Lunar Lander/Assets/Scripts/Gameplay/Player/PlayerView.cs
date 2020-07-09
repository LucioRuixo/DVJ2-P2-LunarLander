using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public ParticleSystem thrustPS;
    public ParticleSystem explosionPS;
    public ParticleSystem debrisPS;

    void OnEnable()
    {
        GameManager.onLevelSetting += ResetRocket;

        PlayerController.onThrustChange += SetParticleSystem;
        PlayerController.onLanding += CheckIfLandingFailed;
        PlayerController.onOutOfFuel += StopThrust;
    }

    void SetParticleSystem(bool play)
    {
        if (play)
            thrustPS.Play();
        else
            thrustPS.Stop();
    }

    void StopThrust()
    {
        if (thrustPS && thrustPS.isPlaying)
            thrustPS.Stop();
    }

    void ResetRocket()
    {
        if (explosionPS && explosionPS.isPlaying)
            explosionPS.Stop();

        if (debrisPS && debrisPS.isPlaying)
            debrisPS.Stop();
    }

    void CheckIfLandingFailed(bool landingSuccessful)
    {
        if (!landingSuccessful)
        {
            if (thrustPS.isPlaying) thrustPS.Stop();

            Explode();
        }
    }

    void Explode()
    {
        if (explosionPS) explosionPS.Play();
        if (debrisPS) debrisPS.Play();
    }

    void OnDisable()
    {
        GameManager.onLevelSetting -= ResetRocket;

        PlayerController.onThrustChange -= SetParticleSystem;
        PlayerController.onLanding -= CheckIfLandingFailed;
        PlayerController.onOutOfFuel -= StopThrust;
    }
}
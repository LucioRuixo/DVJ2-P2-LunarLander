using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public GameObject rocketModel;
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
        if (thrustPS.isPlaying)
            thrustPS.Stop();
    }

    void ResetRocket()
    {
        if (explosionPS.isPlaying)
            explosionPS.Stop();

        if (debrisPS.isPlaying)
            debrisPS.Stop();

        if (!rocketModel.activeSelf)
            rocketModel.SetActive(true);
    }

    void CheckIfLandingFailed(bool landingSuccessful)
    {
        if (!landingSuccessful)
        {
            if (thrustPS.isPlaying)
                thrustPS.Stop();

            Explode();
        }
    }

    void Explode()
    {
        if (rocketModel.activeSelf)
        {
            rocketModel.SetActive(false);

            explosionPS.Play();
            debrisPS.Play();
        }
    }

    void OnDisable()
    {
        PlayerController.onThrustChange -= SetParticleSystem;
        PlayerController.onLanding -= CheckIfLandingFailed;
        PlayerController.onOutOfFuel -= StopThrust;
    }
}
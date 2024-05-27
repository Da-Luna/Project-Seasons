using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleSystems : MonoBehaviour
{
    [SerializeField]
    ParticleSystem particleFootstep;
    [SerializeField]
    ParticleSystem particleTrail;

    #region FOOTSTEPS

    public void HandleFootstepParticle()
    {
        if (particleFootstep == null)
        {
            Debug.LogError("particleFootstep reference not set");
            return;
        }

        var emission = particleFootstep.emission;
        bool hasInput = PlayerInput.Instance.InputHorizontal != 0f;
        var loop = particleFootstep.main;

        if (hasInput)
        {
            if (!particleFootstep.gameObject.activeSelf)
            {
                particleFootstep.gameObject.SetActive(true);
            }

            loop.loop = true;
            emission.enabled = true;

            if (!particleFootstep.isPlaying)
                particleFootstep.Play();
        }
        else if (!hasInput)
        {
            if (loop.loop)
            {
                loop.loop = false;
            }
            else if (!HasRemainingParticles(particleFootstep))
            {
                emission.enabled = false;
                particleFootstep.Stop();

                particleFootstep.gameObject.SetActive(false);
            }

        }
    }


    bool HasRemainingParticles(ParticleSystem ps)
    {
        if (ps.particleCount != 0)
            return true;

        return false;
    }

    #endregion

    #region TRAIL PARTICLE

    public void HandleTrailParticle()
    {
        if (particleTrail == null)
            return;

        var emission = particleTrail.emission;

        if (PlayerInput.Instance.InputHorizontal != 0f && !particleTrail.emission.enabled)
        {
            emission.enabled = true;
            particleTrail.Play();
        }
        else if (PlayerInput.Instance.InputHorizontal == 0f && particleTrail.emission.enabled)
        {
            emission.enabled = false;
            particleTrail.Stop();
        }
    }
    
    #endregion // TRAIL PARTICLE
}

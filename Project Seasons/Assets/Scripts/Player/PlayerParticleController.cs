using UnityEngine;

public class PlayerParticleController : MonoBehaviour
{
    [SerializeField, Tooltip("Particle system for footstep effects")]
    private ParticleSystem footstepParticle;

    [SerializeField, Tooltip("Particle system for trail effects")]
    private ParticleSystem trailParticle;

    private void OnEnable()
    {
        // Find and assign footstep particle system if not set
        if (footstepParticle == null)
        {
            footstepParticle = transform.Find("FootstepParticle").GetComponent<ParticleSystem>();
#if UNITY_EDITOR
            Debug.Log("PlayerParticleController: footstepParticle is NULL, local search with name **FootstepParticle** function was used");
#endif
        }

        // Find and assign trail particle system if not set
        if (trailParticle == null)
        {
            trailParticle = transform.Find("TrailParticle").GetComponent<ParticleSystem>();
#if UNITY_EDITOR
            Debug.Log("PlayerParticleController: trailParticle is NULL, local search with name **TrailParticle** function was used");
#endif
        }
    }

    public void PlayFootstepParticle()
    {
        if (!footstepParticle.emission.enabled)
        {
            var emission = footstepParticle.emission;
            emission.enabled = true;

            footstepParticle.Play();
        }
    }

    public void StopFootstepParticle()
    {
        if (footstepParticle.emission.enabled)
        {
            var emission = footstepParticle.emission;
            emission.enabled = false;

            footstepParticle.Stop();
        }
    }

    public void PlayTrailParticle()
    {
        if (!trailParticle.emission.enabled)
        {
            var emission = trailParticle.emission;
            emission.enabled = true;

            trailParticle.Play();
        }
    }

    public void StopTrailParticle()
    {
        if (trailParticle.emission.enabled)
        {
            var emission = trailParticle.emission;
            emission.enabled = false;

            trailParticle.Stop();
        }
    }
}

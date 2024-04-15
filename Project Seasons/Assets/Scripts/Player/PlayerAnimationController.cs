using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator playerAnimator;

    private void OnEnable()
    {
        if (playerAnimator == null)
            playerAnimator = transform.Find("SpriteHolder").GetComponent<Animator>();
    }

    /// <summary>
    /// Plays the specified animation.
    /// </summary>
    /// <param name="animName">The name of the animation to play.</param>
    public void PlayAnimation(string animName)
    {
        playerAnimator.Play(animName);
    }
}

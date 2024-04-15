using UnityEngine;

public class PlayerCollectablesController : MonoBehaviour
{
    // Number of coins collected by the player
    public int LocalCollectedCoins { get; private set; }

    private void OnEnable()
    {
        LocalCollectedCoins = 0;
    }

    /// <summary>
    /// Increases the number of collected coins by the specified amount.
    /// </summary>
    /// <param name="coinNo">The number of coins to collect.</param>
    public void CoinCollect(int coinNo)
    {
        LocalCollectedCoins += coinNo;
    }
}

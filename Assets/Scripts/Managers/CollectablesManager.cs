using UnityEngine;

public class CollectablesManager : MonoBehaviour
{
    public SFXManager sfxManager;

    public int coins = 0;

    int coinsComboCount = 0;
    Coroutine coinsComboCoroutine;

    public void Collect(string group)
    {
        switch (group)
        {
            case "coin":
                coins += 1;
                coinsComboCount += 1;

                // Restart the timer each time a coin is collected
                if (coinsComboCoroutine != null)
                {
                    StopCoroutine(coinsComboCoroutine);
                }
                coinsComboCoroutine = StartCoroutine(CoinsComboTimer());

                sfxManager.StartSound("coin", 0.5f, 1f + (0.1f * coinsComboCount));

                break;
        }
    }

    private System.Collections.IEnumerator CoinsComboTimer()
    {
        yield return new WaitForSeconds(0.5f);

        // If the coroutine finishes, reset the combo
        coinsComboCount = 0;
    }
}

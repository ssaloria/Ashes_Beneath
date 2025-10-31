using UnityEngine;

public class YouDiedTester : MonoBehaviour
{
    public PlayerDeathUI deathUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))   // press K to test
            deathUI?.ShowDeathScreen();
    }
}

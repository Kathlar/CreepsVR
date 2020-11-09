using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Debug Manager, allowing for special behaviors in development builds and editor.
/// </summary>
public class Debugs : Singleton<Debugs>
{
    private bool isDebug;

    protected override void SingletonAwake()
    {
        isDebug = Debug.isDebugBuild;
#if UNITY_EDITOR
        isDebug = true;
#endif
    }

    private void Update()
    {
        if (!isDebug) return;
        if (Inputs.RKey.WasPressed || InputsVR.LeftHand.secondaryButton.WasPressed)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (Inputs.PKey.WasPressed || InputsVR.LeftHand.primaryButton.WasPressed)
            Debug.Break();
    }
}

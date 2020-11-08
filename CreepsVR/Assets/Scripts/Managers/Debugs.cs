using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (Inputs.RKey.WasPressed || InputsVR.LeftHand.secondaryButton.WasPressed)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (Inputs.PKey.WasPressed || InputsVR.LeftHand.primaryButton.WasPressed)
            Debug.Break();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class QualityManager : Singleton<QualityManager>
{
    PostProcessVolume volume;
    PostProcessProfile profile;

    protected override void SingletonAwake()
    {

    }

    private void Start()
    {
        volume = Game.Player.mainCamera.GetComponent<PostProcessVolume>();
        volume.profile = profile = Instantiate(volume.profile);

        switch(QualitySettings.GetQualityLevel())
        {
            case 0:
                volume.enabled = false;
                break;
        }
    }
}

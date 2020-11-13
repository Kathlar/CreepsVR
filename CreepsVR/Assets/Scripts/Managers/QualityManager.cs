using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// Manager taking care of quality settings in game.
/// </summary>
public class QualityManager : Singleton<QualityManager>
{
#if UNITY_POST_PROCESSING_STACK_V2
    PostProcessVolume volume;
    PostProcessProfile profile;

    protected override void SingletonAwake()
    {

    }

    private void Start()
    {
        if (!Game.Player && Game.Player.mainCamera) return;
        volume = Game.Player.mainCamera.GetComponent<PostProcessVolume>();
        if(volume.profile)
            volume.profile = profile = Instantiate(volume.profile);

        switch (QualitySettings.GetQualityLevel())
        {
            case 0:
                if(volume)
                    volume.enabled = false;
                break;
        }
    }
#endif
}

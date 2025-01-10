using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class SubtitleControlBehaviour : PlayableBehaviour
{
    public string text= "";
    public bool DisplayAllAtOnce = true;
    public string voiceBank;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        try
        {
            SubtitleHandler.LoadNewVoiceBank(voiceBank);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        try { 
            SubtitleHandler.DisplaySnippet(text, DisplayAllAtOnce); 
        }
        catch(Exception e) {
            Debug.LogError(e);
        }
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        base.ProcessFrame(playable, info, playerData);
    }


}

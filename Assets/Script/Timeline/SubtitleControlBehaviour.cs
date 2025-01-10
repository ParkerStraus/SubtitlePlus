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
    private double duration;

    public override void OnPlayableCreate(Playable playable)
    {
        // Get the duration of the clip
        duration = playable.GetDuration();
    }

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
        if (!DisplayAllAtOnce) return;

        try { 
            SubtitleHandler.DisplaySnippet(text); 
        }
        catch(Exception e) {
            Debug.LogError(e);
        }
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (DisplayAllAtOnce) return;


        // Get the current time of the playable
        double currentTime = playable.GetTime();

        //Debug.Log(currentTime);
        // Calculate the percentage of completion
        double percentage = (currentTime / duration) * 100;
        SubtitleHandler.DisplayReveal(text, percentage);
        base.PrepareFrame(playable, info);
    }


}

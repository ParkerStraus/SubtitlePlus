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
    public double duration;
    public SubtitlePlayable clip;
    public string tmpText = "";

    public override void OnPlayableCreate(Playable playable)
    {
        base.OnPlayableCreate(playable);
        // Get the duration of the clip
        Debug.Log("Length: " + duration);
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        Debug.Log(clip.duration); // prints Infinity
        Debug.Log(playable.GetDuration()); // prints duration of clip
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

        // Calculate the percentage of completion
        double percentage = currentTime / playable.GetDuration();
        percentage = Mathf.Clamp((float)percentage, 0f, 1f); // Ensure percentage stays between 0 and 1

        // Calculate the new length of text to display
        int newLength = (int)(text.Length * percentage)+1;

        if (text.Substring(0, newLength) != tmpText)
        {
            // Find and display new characters
            string newText = text.Substring(tmpText.Length, newLength - tmpText.Length);
            SubtitleHandler.DisplaySnippet(newText);

            // Set new tmpText for later
            tmpText = text.Substring(0, newLength);
        }

        base.ProcessFrame(playable, info, playerData);
    }





}

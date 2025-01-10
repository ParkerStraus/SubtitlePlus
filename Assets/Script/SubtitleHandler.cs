using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

public static class SubtitleHandler
{
    // Start is called before the first frame update
    public static Subtitles main = null;

    public static void SummonSubtitles(string subtitleType)
    {
        if (main != null) return;
        GameObject copy = GameObject.Instantiate(Resources.Load<GameObject>("SubtitleObject/" + subtitleType), GameObject.Find("Canvas").transform);
        main = copy.GetComponent<Subtitles>();
        Debug.Log(main);
    }

    public static void DisplaySubtitles(string subtitles, string speaker, string VoiceBank, float DisplayTime, bool HideAfterwards = true, bool DisplayAll = false)
    {
        SummonSubtitles("_default");
        main.Clear();
        if (VoiceBank != null && VoiceBank != "") LoadNewVoiceBank(VoiceBank);
        else main.currentVoiceBank = null;
        main.DisplaySubtitles(HideAfterwards, DisplayAll, subtitles, speaker, DisplayTime);
    }

    public static void DisplaySubtitles(PlayableAsset pa, string speaker, string VoiceBank)
    {
        SummonSubtitles("_default");
        main.Clear();
        if (VoiceBank != null && VoiceBank != "") LoadNewVoiceBank(VoiceBank);
        else main.currentVoiceBank = null;
        main.PlayTimeline(pa, speaker);
    }

    public static void SetSpeaker(string speaker)
    {
        main.SetSpeaker(speaker);
    }

    public static void DisplaySnippet(string text, bool DisplayAtOnce)
    {
        if(DisplayAtOnce)
        {
            main.DisplaySnippet(text);
        }
        else
        {
            main.DisplaySubtitles(HideAfterwards: false, DisplayAll: false, text);
        }
    }

    public static void LoadNewVoiceBank(string voicebank)
    {
        if (voicebank == "") return;
        if (voicebank == "_random")
        {
            VoiceBank[] banks = Resources.LoadAll<VoiceBank>("VoiceBanks/");
            main.currentVoiceBank = banks[UnityEngine.Random.Range(0, banks.Length)];
        }
        else
        {
            VoiceBank vb = Resources.Load<VoiceBank>("VoiceBanks/" + voicebank);
            if(vb == null)
            {
                throw new System.Exception($"Voice bank not found in Resources/VoiceBanks/{voicebank}");
            }
            main.currentVoiceBank = vb;
        }
    }

    public static void DeleteSubtitles()
    {
        try {
            Debug.Log("Deleted");
            GameObject.Destroy(main.gameObject);
            main = null;
        }
        catch (Exception e){ }
    }

    public static void StartHideSubtitles()
    {
        main.AnimateHide();
    }

}

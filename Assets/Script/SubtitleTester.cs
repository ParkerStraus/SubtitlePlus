using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SubtitleTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //SubtitleHandler.DisplaySubtitles("The Fitness-gram Pacer Test is a multistage aerobic capacity test that progressively gets more difficult as it continues.", "SomeDude", "PunchOut");
    }

    public void SummonNewText(string text)
    {
        SubtitleHandler.DisplaySubtitles(text, "SomeDude", "_random", DisplayTime: 10f, DisplayAll: false);
    }

    public void HideText()
    {
        SubtitleHandler.StartHideSubtitles();
    }

    public void TimelineText(PlayableAsset pa)
    {
        SubtitleHandler.DisplaySubtitles(pa, "SomeDude", "");
    }

    //<blockwait=0.5></blockwait><blockwait=0.25>NO </blockwait><blockwait=0.25>ONE </blockwait><blockwait=0.75>MOURNS </blockwait><blockwait=0.25>THE </blockwait><blockwait=0.25>WIC</blockwait><blockwait=0.5>KED</blockwait>




}

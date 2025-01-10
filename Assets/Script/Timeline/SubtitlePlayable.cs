using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Playables;

public class SubtitlePlayable : PlayableAsset
{
    public string text = "";
    public bool DisplayAllAtOnce;
    public string voiceBank;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SubtitleControlBehaviour>.Create(graph);

        var subBehaviour = playable.GetBehaviour();
        //lightControlBehaviour.light = light.Resolve(graph.GetResolver());
        subBehaviour.text = text ;
        subBehaviour.DisplayAllAtOnce = DisplayAllAtOnce;
        subBehaviour.voiceBank = voiceBank;

        return playable;
    }

}

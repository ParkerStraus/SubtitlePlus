using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


[TrackBindingType(typeof(Subtitles))]
[TrackClipType(typeof(SubtitleControlBehaviour))]
public class SubtitleControlTrack : TrackAsset
{

    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        foreach (var clip in GetClips())
        {
            (clip.asset as SubtitlePlayable).parentTrack = this;
        }
        return base.CreateTrackMixer(graph, go, inputCount);
    }
}

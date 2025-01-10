using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="VoiceBank", menuName = "ScriptableObjects/Voice Bank", order = 1)]
public class VoiceBank : ScriptableObject
{
    public AudioClip[] Clips;
    public float LetterSpeed;
    public bool WaitTilDone;
    public bool MadeForPhoenetics;
    public bool RandomClip;
    public int ModulatableRange_min;
    public int ModulatableRange_max;


    public AudioClip GetClip(string text, int index)
    {
        if (RandomClip)
        {
            return Clips[Random.Range(0, Clips.Length)];
        }
        else
        {

            int charpos = text[index];
            if ((charpos >= 65 && charpos <= 90) || (charpos >= 97 && charpos <= 122)) return Clips[charpos % 32];
            else return null;
        }
    }

    public int GetModulate()
    {
        return Random.Range(ModulatableRange_min, ModulatableRange_max);
    }
}

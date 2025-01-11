using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using static Unity.VisualScripting.Member;

public class Subtitles : MonoBehaviour
{
    public TMP_Text MainText;
    public TMP_Text SpeakerText;
    public VoiceBank currentVoiceBank;
    public AudioSource audioSRC;
    public Animator Anim;
    [SerializeField] private RectTransform rectTransform;
    string tmpText = "";
    public PlayableDirector pd;



    public void DisplaySubtitles(bool HideAfterwards, bool DisplayAll, string text = "", string speaker = "", float DisplayTime = 5f)
    {
        SpeakerText.text = speaker;
        StopAllCoroutines();
        StartCoroutine(DisplayProcess(text, HideAfterwards, DisplayAll, DisplayTime));

    }

    IEnumerator DisplayProcess(string text, bool HideAfterwards, bool DisplayAll, float DisplayTime)
    {
        int i = 0;
        if (DisplayAll)
        {
            while (i < text.Length)
            {
                if (text[i] == '<') // Found the start of a tag
                {
                    int endIndex = text.IndexOf('>', i + 1);
                    if (endIndex != -1)
                    {
                        string tag = text.Substring(i, endIndex - i + 1);

                        print(tag);
                        // Handle <blockwait=...>
                        if (tag.StartsWith("<blockwait="))
                        {
                            bool pass = false;
                            float waitTime;
                            try
                            {
                                // Extract wait time
                                int startOfValue = 11; // "<blockwait=" has 10 characters
                                int lengthOfValue = endIndex - i - startOfValue;
                                string waitTimeStr = tag.Substring(startOfValue, lengthOfValue);
                                waitTime = float.Parse(waitTimeStr);
                                Debug.Log(waitTime);

                                // Find </blockwait> tag
                                int endTagIndex = text.IndexOf("</blockwait>", endIndex + 1);
                                Debug.Log("Found end tag at " + endTagIndex);
                                if (endTagIndex == -1)
                                {
                                    Debug.LogError("Missing </blockwait> tag.");
                                    yield break;
                                }

                                // Extract text to display
                                string textToDisplay = text.Substring(endIndex + 1, endTagIndex - endIndex - 1);

                                // Display text block
                                AddToText(textToDisplay, false);
                                i = endTagIndex + 12; // Move index past </blockwait>
                                pass = true;
                            }
                            catch (Exception ex)
                            {
                                Debug.LogError($"Error parsing <blockwait>: {ex.Message + ex.StackTrace} ");
                                yield break;
                            }
                            if (pass)
                            {
                                continue;
                            }
                        }
                        else if (tag.StartsWith("<shake="))
                        {
                            // Handle shake tag
                            Debug.Log(tag);
                            int startOfValue = 7;
                            int lengthOfValue = endIndex - i - startOfValue;

                            Debug.Log(lengthOfValue);
                            string values = tag.Substring(startOfValue, lengthOfValue - 1);
                            i += startOfValue + lengthOfValue + 1;
                            continue;
                        }
                        else
                        {
                            AddToText(tag, false);
                            i += tag.Length-1;
                            continue;
                        }


                        // Remove the tag from the original text
                        text = text.Remove(i, endIndex - i + 1);
                    }
                }
                else
                {
                    AddToText(text[i], false);
                }

                i++;

                // Update the displayed text
                MainText.text = tmpText;
            }
        }
        else
        {

            while (i < text.Length)
            {
                if (text[i] == '<') // Found the start of a tag
                {
                    int endIndex = text.IndexOf('>', i + 1);
                    if (endIndex != -1)
                    {
                        string tag = text.Substring(i, endIndex - i + 1);

                        // Handle <blockwait=...>
                        if (tag.StartsWith("<blockwait="))
                        {
                            bool pass = false;
                            float waitTime;
                            try
                            {
                                // Extract wait time
                                int startOfValue = 11; // "<blockwait=" has 10 characters
                                int lengthOfValue = endIndex - i - startOfValue;
                                string waitTimeStr = tag.Substring(startOfValue, lengthOfValue);
                                waitTime = float.Parse(waitTimeStr);
                                //Debug.Log(waitTime);

                                // Find </blockwait> tag
                                int endTagIndex = text.IndexOf("</blockwait>", endIndex + 1);
                                if (endTagIndex == -1)
                                {
                                    Debug.LogError("Missing </blockwait> tag.");
                                    yield break;
                                }

                                // Extract text to display
                                string textToDisplay = text.Substring(endIndex + 1, endTagIndex - endIndex - 1);
                                //Debug.Log("Found text: " + textToDisplay);

                                // Display text block
                                AddToText(textToDisplay);
                                i = endTagIndex + 12; // Move index past </blockwait>
                                pass = true;
                            }
                            catch (Exception ex)
                            {
                                Debug.LogError($"Error parsing <blockwait>: {ex.Message + ex.StackTrace} ");
                                yield break;
                            }
                            if (pass)
                            {
                                yield return new WaitForSeconds(waitTime);
                                continue;
                            }
                        }
                        else if (tag.StartsWith("<shake="))
                        {
                            // Handle shake tag
                            //Debug.Log(tag);
                            int startOfValue = 7;
                            int lengthOfValue = endIndex - i - startOfValue;

                            //Debug.Log(lengthOfValue);
                            string values = tag.Substring(startOfValue, lengthOfValue - 1);
                            Shake(values);
                            i += startOfValue + lengthOfValue + 1;
                            continue;
                        }
                        else
                        {
                            AddToText(tag, playSound: false) ;
                        }


                        // Remove the tag from the original text
                        text = text.Remove(i, endIndex - i);
                    }
                }
                else
                {
                    AddToText(text[i]);
                }

                i++;

                // Update the displayed text
                MainText.text = tmpText;

                // Wait for a short duration
                yield return new WaitForSeconds(currentVoiceBank == null ? 0.07f : currentVoiceBank.LetterSpeed);
            }
        }
        if (HideAfterwards)
        {

            yield return new WaitForSeconds(5f);
            AnimateHide();
        }
    }

    public void PlayTimeline(PlayableAsset pa, string speaker)
    {
        SpeakerText.text = speaker;
        pd.playableAsset = pa;
        pd.Play();
    }

    public void DisplaySnippet(string text)
    {
        StopAllCoroutines();
        int i = 0;
        while (i < text.Length)
        {
            if (text[i] == '<') // Found the start of a tag
            {
                int endIndex = text.IndexOf('>', i + 1);
                if (endIndex != -1)
                {
                    string tag = text.Substring(i, endIndex - i + 1);

                    //print(tag);
                    // Handle <blockwait=...>
                    if (tag.StartsWith("<shake="))
                    {
                        // Handle shake tag
                        //Debug.Log(tag);
                        int startOfValue = 7;
                        int lengthOfValue = endIndex - i - startOfValue;

                        //Debug.Log(lengthOfValue);
                        string values = tag.Substring(startOfValue, lengthOfValue);
                        i += startOfValue + lengthOfValue + 1;
                        Shake(values);
                        continue;
                    }
                    else if (tag.StartsWith("<speaker="))
                    {
                        // Handle shake tag
                        //Debug.Log(tag);
                        int startOfValue = tag.IndexOf('=', i + 1) + 1;
                        int lengthOfValue = endIndex - i - startOfValue;

                        //Debug.Log(lengthOfValue);
                        SpeakerText.text = tag.Substring(startOfValue, lengthOfValue);
                        i += startOfValue + lengthOfValue + 1;
                        continue;
                    }
                    else if (tag.StartsWith("<clea"))
                    {
                        Clear();
                        i += 7;
                        continue;
                    }
                    else if (tag.StartsWith("<exi"))
                    {
                        SubtitleHandler.DeleteSubtitles();
                        break;
                    }
                    else if (tag.StartsWith("<hid"))
                    {
                        AnimateHide();
                        break;
                    }
                    else
                    {
                        AddToText(tag);
                        i += tag.Length - 1;
                        continue;
                    }
                }
            }
            else
            {
                AddToText(text[i]);
            }

            i++;

            // Update the displayed text
            MainText.text = tmpText;
        }
    }



    public void SetSpeaker(string speaker)
    {
        SpeakerText.text = speaker;
    }
    public void Clear()
    {
        tmpText = "";
        MainText.text = "";
    }
    public void AnimateHide()
    {
        Anim.SetTrigger("Hide");
    }

    public void HidetoHandler()
    {
        SubtitleHandler.DeleteSubtitles();
    }


    void AddToText(string text, bool playSound = true)
    {
        tmpText += text;
        if(playSound)PlaySound();
        MainText.text = tmpText;
    }

    void AddToText(char character, bool playSound = true)
    {
        tmpText += character;
        if (playSound) PlaySound();
        MainText.text = tmpText;
    }

    void PlaySound()
    {
        if (currentVoiceBank == null) return;
        
        if (currentVoiceBank.WaitTilDone)
        {
            if (!audioSRC.isPlaying)
            {
                audioSRC.pitch = Mathf.Pow(1.05946f, currentVoiceBank.GetModulate());
                audioSRC.PlayOneShot(currentVoiceBank.GetClip(tmpText, tmpText.Length - 1));
            }
        }
        else
        {
            audioSRC.Stop();
            audioSRC.pitch = Mathf.Pow(1.05946f, currentVoiceBank.GetModulate());
            audioSRC.PlayOneShot(currentVoiceBank.GetClip(tmpText, tmpText.Length - 1));
        }
    }

    void Shake(string values)
    {
        Debug.Log("Shaking at "+values);
        string[] valueParsed = values.Split(',');
        float duration = 0.75f;
        try
        {
            float.TryParse(valueParsed[2], out duration);
        }
        catch(Exception e)
        {
            //Debug.Log(e);
        }
        rectTransform.DOShakeAnchorPos(duration, float.Parse(valueParsed[0])*10, int.Parse(valueParsed[1])*10, 90, false, true, ShakeRandomnessMode.Full);
    }
}

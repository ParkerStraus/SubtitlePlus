# SubtitlePlus
 A cool Subtitles package made for the fun in it

## Methods

 ## Escape Codes
```
 - <shake=x,y,z> Self explanatory. Shakes the box
 - <block=x></block> Displays the word in once chunk. Not by individual letters
 - <speaker=x> Changes the speakers name.
 - <clear> Clears the subtitle's main text.
 - <hide> Deletes the subtitles by animation
 - <exit> Deletes the subtitles without animation

```

## Example Implementation
```
public void SummonNewText(string text)
{
    SubtitleHandler.DisplaySubtitles(text, "SomeDude", "_random", DisplayTime: 10f, DisplayAll: false);
}
```

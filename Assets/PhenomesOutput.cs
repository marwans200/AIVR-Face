using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PhenomesOutput : MonoBehaviour
{
    public int currentMouthCode;
    public string sentence;
    public List<Phenominizor.MouthFrame> framesGen;

    public List<Phenominizor.MouthFrame> GetPhenomeFrames()
    {
        List<Phenominizor.MouthFrame> frames = Phenominizor.ToMouthFrames(sentence);
        return frames;
    }

    public void StartTest()
    {
        StartCoroutine(TestPlayback());
    }

    public IEnumerator TestPlayback()
    {
        framesGen = GetPhenomeFrames();

        foreach (var f in framesGen)
        {
            if(f.pauseAfter > 300) yield return new WaitForSeconds(0.1f);
            currentMouthCode = f.mouthCode;
            yield return new WaitForSeconds(f.pauseAfter / 1000f);
        }

        currentMouthCode = 0;
    }

}

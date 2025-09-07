using System;
using UnityEngine;

public class InterviewerController : MonoBehaviour
{
    public PhenomesOutput inPhenomes;

    [Serializable]
    public struct animprop
    {
        public string name;
        public Vector2 pos;
    }
    public animprop[] AnimPositions;
    public Vector2 currentAnimPosition;

    public Animator InterviewerAnimator;
    public float LerpSpeed = 1f;
    public void Update(){
        int cMC = inPhenomes.currentMouthCode;
        if(cMC < 0 || cMC >= AnimPositions.Length) return;
        currentAnimPosition =  inPhenomes.currentMouthCode == 0 ? new Vector2(0,0) : Vector2.Lerp(currentAnimPosition,AnimPositions[inPhenomes.currentMouthCode].pos,Time.deltaTime* LerpSpeed);
        InterviewerAnimator.SetFloat("Y", currentAnimPosition.y);
        InterviewerAnimator.SetFloat("X", currentAnimPosition.x);
    }
}

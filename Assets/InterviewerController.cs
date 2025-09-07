using UnityEngine;

public class InterviewerController : MonoBehaviour
{
    public PhenomesOutput inPhenomes;

    public Vector2[] AnimPositions;
    public Vector2 currentAnimPosition;

    public Animator InterviewerAnimator;
    public float LerpSpeed = 1f;
    public void Update(){
        currentAnimPosition = Vector2.Lerp(currentAnimPosition,AnimPositions[inPhenomes.currentMouthCode],Time.deltaTime* LerpSpeed);
        InterviewerAnimator.
    }
}

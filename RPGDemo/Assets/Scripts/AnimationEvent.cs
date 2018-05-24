using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour {

    public delegate void _AnimationEvent();
    public static event _AnimationEvent OnSlashAnimationHit;
    public static event _AnimationEvent OnSlashAnimationPlayerHit;

    void SlashAnimationHitEvent()
    {
        OnSlashAnimationHit();
    }

    void SlashAnimationPlayerHitEvent()
    {
        OnSlashAnimationPlayerHit();
    }





}

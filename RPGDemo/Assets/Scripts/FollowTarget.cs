using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {

    public Transform target;
    public float smoothTime;
    private Vector3 offset;

	// Use this for initialization
	void Start () {

        offset = transform.position - target.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        iTween.MoveUpdate(gameObject, iTween.Hash("position", target.position + offset, "time", smoothTime, "easetype", iTween.EaseType.easeInOutSine));
    }
}

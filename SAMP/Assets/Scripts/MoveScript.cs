﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MoveScript : MonoBehaviour {

	private GameObject go1,go2;
	private Vector3 dest1,dest2, rotationPoint;
	private float speed = 80, rotated = 0;
	private List<GameObject> queue;
	private Color prevColor, prevColor2;
	private Text score;

	// Use this for initialization
	void Start () {
		//score = GameObject.Find ("SwapCounter").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate ()
	{
		if (queue == null || queue.Count < 1)
			return;

		if (go1 == null || go2 == null)
			return;

		float step = speed * Time.deltaTime;
		rotated += step;

		//go1.transform.RotateAround (rotationPoint, Vector3.right, step);
		//go2.transform.RotateAround (rotationPoint, Vector3.right, step);

        //LeanTween.move(go1, dest1, 1.5f);
        //LeanTween.move(go2, dest2, 1.5f);
	}
	
	IEnumerator DoMoving()
	{
		for(int i = 0; i < queue.Count; i=i+2)
		{
			rotated = 0;
			go1 = queue[i];
			go2 = queue[i+1];

			changeColor(true);

			dest1 = go2.transform.position;
			dest2 = go1.transform.position;
			getRotationPoint();

			Debug.Log ("Dest1: " + dest1);
			Debug.Log ("Dest2: " + dest2);
			Debug.Log ("Rotation Point: " + rotationPoint);

			while(rotated < 180)
				yield return null;

			correctPositions();

			increaseCounter ();

			changeColor(false);
		}
		queue = null;
	}

	public void swap(List<GameObject> _queue)
	{
		queue = _queue;
        doSwap();
		//StartCoroutine(DoMoving());
	}

    private void doSwap()
    {
        for (int i = 0; i < queue.Count; i = i + 2)
        {
            go1 = queue[i];
            go2 = queue[i+1];

            changeColor(true);

            dest1 = go2.transform.position;
            dest2 = go1.transform.position;
            getRotationPoint();

            Vector3 temp1 = rotationPoint;
            temp1.y = temp1.y + 10;

            Vector3 temp2 = rotationPoint;
            temp2.y = temp2.y - 10;

            LeanTween.move(go1, new Vector3[] {dest2, temp1, temp1, dest1 }, 1.5f);
            LeanTween.move(go2, new Vector3[] {dest1, temp2, temp2, dest2 }, 1.5f);
        }
    }

	private void increaseCounter()
	{
		Text score = GameObject.Find ("SwapCounter").GetComponent<Text> ();

		if (score == null)
			return;

		score.GetComponent<SwapCounterScript> ().incCounter ();
	}

	private void changeColor(bool is_moving)
	{
		MoveHelperScript mhs = gameObject.AddComponent<MoveHelperScript> ();
		mhs.changeColor (go1, go2, is_moving, ref prevColor, ref prevColor2);
		//Destroy (GetComponent<MoveHelperScript>());
		Destroy (mhs);
	}

	private void getRotationPoint()
	{
		float distance = Mathf.Abs (go1.transform.position.z - go2.transform.position.z);
		float z = 0.0f;
		if (go1.transform.position.z > go2.transform.position.z)
			z = go1.transform.position.z - distance / 2;
		else
			z = go1.transform.position.z + distance / 2;

		rotationPoint = new Vector3(go1.transform.position.x,
			go1.transform.position.y,
			z);

	}

	private void correctPositions()
	{
		if (go1.transform.position != dest1 || go2.transform.position != dest2) 
		{
			go1.transform.rotation = Quaternion.identity;
			go1.transform.position = dest1;
			go2.transform.rotation = Quaternion.identity;
			go2.transform.position = dest2;
		}
	}
}

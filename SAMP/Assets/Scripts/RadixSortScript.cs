﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RadixSortScript : MonoBehaviour {

	private GameObject[] elementArray;
    private List<ArrayList> swappingQueue = new List<ArrayList> (); // 0 = GameObject, 1 = id
    private List<Vector3> bucket_positions = new List<Vector3> ();
	private List<BucketElementObject> bucket_element_objects = new List<BucketElementObject>();

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void startSort(GameObject[] array)
	{
		swappingQueue.Clear ();
		elementArray = array;
		setBucketPositions ();
		myRadixSort ();

		//if (swappingQueue != null && swappingQueue.Count >= 1) 
		//{
		//	RadixMoveScript m = gameObject.AddComponent<RadixMoveScript> ();
		//	m.swap (swappingQueue);
		//}

		if (bucket_element_objects != null && bucket_element_objects.Count >= 1)
		{
			RadixMoveScript m = gameObject.AddComponent<RadixMoveScript>();
			m.swap_new(bucket_element_objects);
		}
	}

	// Source: http://algorithmsandstuff.blogspot.co.at/2014/06/radix-sort-in-c-sharp.html
	private void myRadixSort()
	{
		bool isFinished = false;
		int digitPosition = 0;

		List<Queue<GameObject>> buckets = new List<Queue<GameObject>>();
		InitializeBuckets(buckets);

		while (!isFinished)
		{
			isFinished = true;

			foreach (GameObject go in elementArray)
			{
				int value = go.GetComponent<SingleElementScript> ().getElementId ();
				int bucketNumber = GetBucketNumber(value, digitPosition);
				if (bucketNumber > 0)
				{
					isFinished = false;
				}

				buckets[bucketNumber].Enqueue(go);
			}

			int i = 0;
			int bucket_number = 0;
			foreach (Queue<GameObject> bucket in buckets)
			{
				int bucket_size = bucket.Count;
				float y_position = -10; // 8
				int bucket_position = 0;
				while (bucket.Count > 0)
				{
					elementArray[i] = bucket.Dequeue();
					ArrayList list = new ArrayList ();

					if (bucket_size != elementArray.Length)
					{
						bucket_element_objects.Add(new BucketElementObject(elementArray[i], bucket_number, bucket_position));

						//Vector3 dest = bucket_positions [bucket_number];
						//dest.y = elementArray[i].transform.position.y + y_position;

						//list.Add (elementArray [i]);
						//list.Add (dest);
						//swappingQueue.Add (list);

						//y_position += 3;
					}
					bucket_position++;
					i++;
				}
				bucket_number++;
			}

			setToInitialPositions ();
			digitPosition++;
		}
	}

	private void setToInitialPositions()
	{
		for (int i = 0; i < elementArray.Length; i++) 
		{
			bucket_element_objects.Add(new BucketElementObject(elementArray[i], -1, i));

			//ArrayList list = new ArrayList ();
			//list.Add (elementArray [i]);
			//list.Add (bucket_positions [i]);
			//swappingQueue.Add (list);
		}
	}

	private int GetBucketNumber(int value, int digitPosition)
	{
		int bucketNumber = (value / (int)Math.Pow(10, digitPosition)) % 10;
		return bucketNumber;
	}

	private static void InitializeBuckets(List<Queue<GameObject>> buckets)
	{
		for (int i = 0; i < 10; i++)
		{
			Queue<GameObject> q = new Queue<GameObject>();
			buckets.Add(q);
		}
	}

	private void setBucketPositions()
	{
		for (int i = 0; i < elementArray.Length; i++) 
		{
			bucket_positions.Add (elementArray [i].transform.position);
		}
	}
}

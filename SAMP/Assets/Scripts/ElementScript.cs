﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElementScript : MonoBehaviour {

	private GameObject[] elementArray;
	private int locked;
//	private QuickSortScript q;
//	private HeapSortScript hs;
//	private GnomeSortScript gs;
//	private MergeSortScript ms;
	private RadixSortScript rs;
	public GameObject element;
	public GameObject sortingbox;
    private int y_offset = 15;

	// Use this for initialization
	void Start () {

		locked = 0;
//		q = gameObject.AddComponent<QuickSortScript> ();
//		hs = gameObject.AddComponent<HeapSortScript> ();
//		gs = gameObject.AddComponent<GnomeSortScript> ();
//		ms = gameObject.AddComponent<MergeSortScript> ();
		rs = gameObject.AddComponent<RadixSortScript> ();
        int size = getArraySize();
		spawnElements (size);
		//initElements (size);
        spawnElements (size);
	}
	
	// Update is called once per frame
	void Update () {

	}

    void spawnElements(int size)
	{
        spawnNewSortingBox(size);
	}

    void spawnNewSortingBox(int size)
    {
        //get number of existing sorting boxes
        int sortingbox_count = GameObject.FindGameObjectsWithTag("SortingBoxes").Length;
        Debug.Log("SORTING BOX COUNT: " + sortingbox_count);

        //spawn sorting box
        var sortingbox_go = Instantiate(sortingbox);

        //adjust location by count*y_offset
        if (sortingbox_count > 0)
            adjustSortingBoxLocation(sortingbox_count, sortingbox_go);

        //spawn elements
        GameObject[] sbox_elements = new GameObject[size];
        for (int i = 0; i < size; i++)
        {
            sbox_elements[i] = Instantiate(element, sortingbox_go.transform);
            if (sortingbox_count > 0)
                adjustElementsLocation(sortingbox_count, sbox_elements[i]);
        }

        //set element array for this sorting box
        sortingbox_go.GetComponent<SortingBoxScript>().setElementArray(sbox_elements);

        //setup element array
        elementArray = sbox_elements;
        setupElementArray(sbox_elements);
    }

    private void adjustSortingBoxLocation(int count, GameObject sortingbox_go)
    {
        Vector3 old_pos = sortingbox_go.transform.position;
        old_pos.y -= count * y_offset;
        Vector3 new_pos = new Vector3(old_pos.x, old_pos.y - count * y_offset, old_pos.z);

        sortingbox_go.transform.position = new_pos;
    }

    private void adjustElementsLocation(int count, GameObject element)
    {
        Vector3 old_pos = element.transform.position;
        old_pos.y -= count * y_offset;
        Vector3 new_pos = new Vector3(old_pos.x, old_pos.y - count * y_offset, old_pos.z);

        element.transform.position = new_pos;
    }

    void initElements(int size)
	{
		elementArray = GameObject.FindGameObjectsWithTag ("Elements");
		float position_z = -55.0f;
		float[] scale_array = fillScaleArray (elementArray.Length);
		int i = 0;
		foreach (GameObject go in elementArray) 
		{
			go.GetComponentInChildren<TextMesh>().text = (i).ToString ();
			go.GetComponent<SingleElementScript> ().setElementId (i);
			Rigidbody rb = go.GetComponentInChildren<Rigidbody>();
			rb.transform.localScale = new Vector3(scale_array[i],scale_array[i],scale_array[i]);
			setColor (go,scale_array[i]);
			go.transform.position = new Vector3(rb.position.x,rb.position.y,position_z);
			position_z += 5.0f;
			i++;
		}
		shuffleGameObjects ();
	}

    private int getArraySize()
    {
        GameObject empty = GameObject.Find ("EmptyGameObject");
        int size = 0;
        if (empty == null)
            size = 10;
        else
            size = empty.GetComponent<SliderUpdateScript> ().getElementSize ();
        Debug.Log (size.ToString());

        return size;
    }

    private void setupElementArray(GameObject[] elements)
    {
        float position_z = -55.0f;
        float[] scale_array = fillScaleArray (elements.Length);
        int i = 0;
        foreach (GameObject go in elements)
        {
            //set text & id
            go.GetComponentInChildren<TextMesh>().text = (i).ToString ();
            go.GetComponent<SingleElementScript> ().setElementId (i);

            //adjust rigidbody
            Rigidbody rb = go.GetComponentInChildren<Rigidbody>();
            rb.transform.localScale = new Vector3(scale_array[i],scale_array[i],scale_array[i]);
            setColor (go,scale_array[i]);
            go.transform.position = new Vector3(rb.position.x,rb.position.y,position_z);
            position_z += 5.0f;
            i++;
        }
        shuffleGameObjects ();
    }

	private void setColor(GameObject go, float scale)
	{
		float max_scale = 4.0F;
		int min_GB_color = 0;
		float multiplier = (255 - min_GB_color) / max_scale;
		float color = 1 - ((min_GB_color + scale * multiplier) / 255);
		//Debug.Log ("COLOR: " + color);

		foreach (Transform child in go.transform) 
		{
			if (child.tag.Equals ("BasicElement")) 
			{
				child.GetComponent<Renderer> ().material.color = new Color (1, color, color);
			}
		}
	}

	private float[] fillScaleArray(int size)
	{
		float max_scale = 4.0f;
		float min_scale = 1.0f;
		float inc = (max_scale - min_scale) / ((float)size-1);

		float[] scale_array = new float[size];
		for (int i = 0; i < scale_array.Length; i++) 
		{
			scale_array[i] = min_scale + inc*i;
		}

		//shuffleArray (scale_array);

		return scale_array;
	}

	private void shuffleArray(float[] scale_array)
	{
		for (int i = 0; i < scale_array.Length; i++) 
		{
			float tmp = scale_array[i];
			int r = Random.Range (i, scale_array.Length);
			scale_array[i] = scale_array[r];
			scale_array[r] = tmp;
		}
	}

	private void shuffleGameObjects()
	{
		for (int i = 0; i < elementArray.Length; i++) 
		{
			GameObject tmp = elementArray [i];
			Vector3 a_posi = elementArray [i].transform.position;

			int r = Random.Range (i, elementArray.Length);
			Vector3 b_posi = elementArray [r].transform.position;

			elementArray [i] = elementArray [r];
			elementArray [i].transform.position = a_posi;

			elementArray [r] = tmp;
			elementArray [r].transform.position = b_posi;
		}
	}

	private void printGameObjects()
	{
		for (int i = 0; i < elementArray.Length; i++) 
		{
//			Debug.Log (elementArray [i].name + " - Scale: " + elementArray [i].GetComponent<Rigidbody> ().transform.localScale
//				+ " - Position: " + elementArray [i].GetComponent<Rigidbody> ().position);
			Debug.Log (elementArray [i].name + " - Scale: " + elementArray [i].GetComponentInChildren<Rigidbody> ().transform.localScale
				+ " - Position: " + elementArray [i].GetComponentInChildren<Rigidbody> ().position);

		}
	}

	public void printElementIDs()
	{
		Debug.Log ("Element IDs");
		foreach (GameObject go in elementArray) 
		{
			Debug.Log (go.GetComponent<SingleElementScript> ().getElementId ());
		}
	}

	public void quickSort()
	{
		if (locked == 1)
			return;

		// TODO: this doesn't wait till animation has finished 
		//locked = 1;
		//q.startSort(elementArray, 0, elementArray.Length);
		//locked = 0;

        List<GameObject[]> elementArrays = getElementArrays();
        locked = 1;
        foreach (GameObject[] array in elementArrays)
        {
            QuickSortScript ss = gameObject.AddComponent<QuickSortScript> ();
            ss.startSort(array, 0, array.Length);
        }
        locked = 0;
	}

	public void heapSort()
	{
		if (locked == 1)
			return;

		// TODO: this doesn't wait till animation has finished 
//		locked = 1;
//		hs.startSort(elementArray);
//		locked = 0;

        List<GameObject[]> elementArrays = getElementArrays();
        locked = 1;
        foreach (GameObject[] array in elementArrays)
        {
            HeapSortScript ss = gameObject.AddComponent<HeapSortScript> ();
            ss.startSort(array);
        }
        locked = 0;
	}

	public void mergeSort()
	{
		if (locked == 1)
			return;

		// TODO: this doesn't wait till animation has finished 
//		locked = 1;
//		ms.startSort (elementArray);
//		locked = 0;

        List<GameObject[]> elementArrays = getElementArrays();
        locked = 1;
        foreach (GameObject[] array in elementArrays)
        {
            MergeSortScript ss = gameObject.AddComponent<MergeSortScript> ();
            ss.startSort(array);
        }
        locked = 0;
	}

	public void gnomeSort()
	{
		if (locked == 1)
			return;

		// TODO: this doesn't wait till animation has finished 
//		locked = 1;
//		gs.startSort(elementArray);
//		locked = 0;

        List<GameObject[]> elementArrays = getElementArrays();
        locked = 1;
        foreach (GameObject[] array in elementArrays)
        {
            GnomeSortScript ss = gameObject.AddComponent<GnomeSortScript> ();
            ss.startSort(array);
        }
        locked = 0;
	}

	public void radixSort()
	{
		if (locked == 1)
			return;

		// TODO: this doesn't wait till animation has finished 
//		locked = 1;
//		rs.startSort(elementArray);
//		locked = 0;

        List<GameObject[]> elementArrays = getElementArrays();
        locked = 1;
        foreach (GameObject[] array in elementArrays)
        {
            RadixSortScript ss = gameObject.AddComponent<RadixSortScript> ();
            ss.startSort(array);
        }
        locked = 0;
	}

    private List<GameObject[]> getElementArrays()
    {
        GameObject[] container = GameObject.FindGameObjectsWithTag("Container");
        List<GameObject[]> elementArrays = new List<GameObject[]>();

        for (int i = 0; i < container.Length; i++)
        {
            if (container[i].GetComponent<ElementContainerScript>().getHighlighted())
            {
                GameObject parent = container[i].transform.parent.gameObject;
                if (parent != null)
                {
                    elementArrays.Add(parent.GetComponent<SortingBoxScript>().getElementArray());
                    Debug.Log("Added element array");
                }
            }
        }
        return elementArrays;
    }
}

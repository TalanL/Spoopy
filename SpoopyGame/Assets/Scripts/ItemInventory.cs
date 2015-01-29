﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemInventory : MonoBehaviour
{
    static public bool isOpen;
    static public List<GameObject> objects;

    public Vector3 desiredLocalPlacement, unusedLocalPlacement;

    Camera viewing;
    public bool isDrawnOut;
    Vector3 lastPosition;
    public float tweenResistance = 3;
    public float viewAngleToTest = 15;

	// Use this for initialization
	void Start ()
    {
        viewing = GameObject.Find("LitCamera").camera;
        isOpen = false;
        objects = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (GameObject.Find("OVRCameraRig") == null)
            desiredLocalPlacement = new Vector3(0, 0.25f, 1);
        //Vector4 desInWorld = transform.parent.localToWorldMatrix * desiredLocalPlacement;
        Vector3 desInWorld = transform.parent.TransformPoint(desiredLocalPlacement);
        Vector3 thisPosition = transform.parent.position;
        isDrawnOut = (isOpen || (Vector3.Angle(viewing.transform.forward, new Vector3(desInWorld.x, desInWorld.y, desInWorld.z) - viewing.transform.position) < viewAngleToTest && (thisPosition - lastPosition).sqrMagnitude < 0.000000001 / Time.deltaTime));
        
        lastPosition = thisPosition;

        transform.localPosition = (transform.localPosition * tweenResistance + ((isDrawnOut) ? desiredLocalPlacement : unusedLocalPlacement)) / (tweenResistance + 1);

        if (isOpen)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                float angle = i / (float)objects.Count * 2 * Mathf.PI;
                objects[i].transform.position = transform.position + new Vector3(Mathf.Cos(angle), 0.5f, Mathf.Sin(angle));
            }
        }
	}

    static public void storeItem(ItemInteraction item)
    {
        objects.Add(item.gameObject);
    }
    static public void removeItem(ItemInteraction item)
    {
        objects.Remove(item.gameObject);
    }

    static public bool isInBag(ItemInteraction item)
    {
        bool found=false;
        for (int i = 0; i < objects.Count && !found; i++)
        {
            found = (objects[i] == item.gameObject);
        }
        return found;
    }
}

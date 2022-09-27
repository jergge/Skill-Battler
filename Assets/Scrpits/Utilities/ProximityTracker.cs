using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProximityTracker<T> 
        where T : MonoBehaviour 
{
    public List<T> list = new List<T>();

    Transform origin;
    float radius;
    float timeStep;
    int layerMask;
    GameManager gameManager => MonoBehaviour.FindObjectOfType<GameManager>();

    public bool debugLines = true;

    public ProximityTracker(Transform origin, float radius, float timeStep, int layerMask = Physics.AllLayers)
    {   
        this.origin = origin;
        this.radius = radius;
        this.timeStep = timeStep;
        this.layerMask = layerMask;

        //Debug.Log("Created new tracker");

        gameManager.StartCoroutine(Updater());
    }

    IEnumerator Updater()
    {
        while (true) {
            UpdateList();
            //Debug.Log("tracker refreshing...");
            if (debugLines)
                Debugs();

            yield return new WaitForSeconds(timeStep);
        }
    } 

    void UpdateList() 
    {
        list.Clear();

        var array = Physics.OverlapSphere(origin.position, radius, layerMask);
        //Debug.Log(array);
        foreach (Collider col in array) {
            T t;
            if (col.gameObject.TryGetComponent<T>(out t))
            {
                list.Add(t);
            }
        }

    }

    void Debugs() {
        foreach ( T e in list) {
            Debug.DrawLine(origin.position, e.transform.position, Color.black, timeStep);
            //Debug.Log(e.name);
        }
    }
}


public class ProximityTracker<T, U> 
        where T : MonoBehaviour
        where U : Delegate
{
    public List<T> list = new List<T>();

    Transform origin;
    float radius;
    float timeStep;
    string eventHook;
    U callback;
    int layerMask;

    bool validDelegate = false;

    GameManager gameManager => MonoBehaviour.FindObjectOfType<GameManager>();
    public bool debugLines = true;

    public ProximityTracker(Transform origin, float radius, float timeStep, string eventHook, U callback, int layerMask = Physics.AllLayers)
    {   
        this.origin = origin;
        this.radius = radius;
        this.timeStep = timeStep;
        this.eventHook = eventHook;
        this.callback = callback;
        this.layerMask = layerMask;

        //Debug.Log("Created new tracker");
        UpdateList();

        //Make sure the string methodname is available on the collected objects
        validDelegate = TestDelegate();

        //Just a MonoBehaviour to run the Coroutine, since this does not extend MonoBehaviour
        gameManager.StartCoroutine(Updater());
    }

    bool TestDelegate()
    {
        if ( list.Count > 0) {
            var t = list[0];

            return (t.GetType().GetEvent(eventHook) == null) ? false : true;
        }
        return false;
    }

    IEnumerator Updater()
    {
        while (true) {
            UpdateList();
            if (debugLines)
                Debugs();

            yield return new WaitForSeconds(timeStep);
        }
    } 

    void UpdateList() 
    {
        RemoveSubscriptions();
        list.Clear();

        var array = Physics.OverlapSphere(origin.position, radius, layerMask);
        foreach (Collider col in array) {
            T t;
            if (col.gameObject.TryGetComponent<T>(out t))
            {
                list.Add(t);
            }
        }
        AddSubscriptions();
    }

    void AddSubscriptions() {
        if (!validDelegate)
            return;

        foreach(T t in list) {
            t.GetType().GetEvent(eventHook).AddEventHandler(t, callback);
        }
    }

    void RemoveSubscriptions() {
        if (!validDelegate)
            return;

        foreach(T t in list) {
            t.GetType().GetEvent(eventHook).RemoveEventHandler(t, callback);
        }
    }

    void Debugs() {
        foreach ( T e in list) {
            Debug.DrawLine(origin.position, e.transform.position, Color.black, timeStep);
            //Debug.Log(e.name);
        }
    }
}
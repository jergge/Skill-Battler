using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastNotificationDisplayer : MonoBehaviour
{
    public GameObject listenForToastFrom;
    public Transform displayArea;
    public ToastNotification toastPrefab;
    //public float toastDuration = 5;

    List<ISendToastNotifications> listeningFrom = new List<ISendToastNotifications>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (var toaster in listenForToastFrom.GetComponentsInChildren<ISendToastNotifications>())
        {
            toaster.PushToast += RaiseNewToast;
            listeningFrom.Add(toaster);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RaiseNewToast(ToastNotificationInfo toast)
    {
        // Debug.Log("Raising new Toast");
        ToastNotification notification = GameObject.Instantiate(toastPrefab, displayArea.transform);
        notification.Configure(toast);
    }

    void Destroy()
    {
        foreach (var toaster in listeningFrom)
        {
            toaster.PushToast -= RaiseNewToast;
        }
    }
}

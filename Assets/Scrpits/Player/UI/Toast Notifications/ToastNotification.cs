using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToastNotification : MonoBehaviour
{
    public Image icon;
    public TMP_Text text;

    float remaingTime;

    public void Configure(ToastNotificationInfo info)
    {
        icon.sprite = info.sprite;
        text.text = info.message;
        remaingTime = info.duration;
    }

    void Update()
    {
        remaingTime -= Time.deltaTime;

        if (remaingTime <=0)
        {
            Destroy(gameObject);
        }
    }
}

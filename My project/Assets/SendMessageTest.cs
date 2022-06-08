using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessageTest : MonoBehaviour
{
    private void Start()
    {
        gameObject.transform.parent.SendMessage("Shift", 1);
    }
}

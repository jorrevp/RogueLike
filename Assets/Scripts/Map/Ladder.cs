using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public bool Up;

    private void Start()
    {
        GameManager.Get.AddLadder(this);
    }
}


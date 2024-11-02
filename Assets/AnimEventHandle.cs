using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventHandle : MonoBehaviour
{
    public static bool isPushAnimation = false;

    public void MirroringValue()
    {
        isPushAnimation = !isPushAnimation;
    }
}












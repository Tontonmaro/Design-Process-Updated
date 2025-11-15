using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public void lockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void unlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}

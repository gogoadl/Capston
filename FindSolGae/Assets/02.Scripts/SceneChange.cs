﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void ChangeMenuScene()
    {
        SceneManager.LoadScene("Menu Scene");
    }
    public void ChangeGameScene()
    {
        Cursor.visible = false;
        SceneManager.LoadScene("Main Scene");
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneFrame  {

    public SceneFrame(Scene scene)
    {
        m_scene = scene;
    }

    Scene m_scene;

    public Scene ContainedScene { get { return m_scene; } }

    public void SetSceneObjectsActive(bool value)
    {
        int rootcount = m_scene.rootCount;
        GameObject[] rootObjects = m_scene.GetRootGameObjects();
        foreach (GameObject o in rootObjects)
        {
            if (!o.CompareTag("Don't Suspend"))
            {
                o.SetActive(value);
            }
        }
    }



    public void Popped()
    {
        SetSceneObjectsActive(false);
        SceneManager.UnloadSceneAsync(m_scene);
    }

    public void PoppedInto()
    {
        SceneManager.SetActiveScene(m_scene);
        SetSceneObjectsActive(true);
    }
}

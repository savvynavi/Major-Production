using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneFrame  {

    public SceneFrame(Scene scene)
    {
        m_scene = scene;
    }

    Scene m_scene;

    Scene ContainedScene { get { return m_scene; } }

    void SetSceneObjectsActive(bool value)
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

    public void Pushed()
    {
        SceneManager.SetActiveScene(m_scene);
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

    public void PushedOnto()
    {
        SetSceneObjectsActive(false);
    }
}

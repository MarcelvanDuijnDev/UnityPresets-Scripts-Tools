﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadScenes : MonoBehaviour
{
    private bool _AsyncLoading = false;

    //Load/Reload Scenes
    public void LoadScene(int sceneid)
    {
        SceneManager.LoadScene(sceneid);
    }
    public void LoadScene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void AsyncReloadScene()
    {
        if (!_AsyncLoading)
        {
            _AsyncLoading = true;
            StartCoroutine(LoadSceneAsync(SceneManager.GetActiveScene().buildIndex));
        }
    }
    public void AsyncLoadScene(int sceneid)
    {
        if (!_AsyncLoading)
        {
            _AsyncLoading = true;
            StartCoroutine(LoadSceneAsync(sceneid));
        }
    }
    public void AsyncLoadScene(string scenename)
    {
        if (!_AsyncLoading)
        {
            _AsyncLoading = true;
            StartCoroutine(LoadSceneAsync(scenename));
        }
    }
    private IEnumerator LoadSceneAsync(string scenename)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scenename);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    private IEnumerator LoadSceneAsync(int sceneid)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneid);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    //Quit
    public void QuitApplication()
    {
        Application.Quit();
    }
}

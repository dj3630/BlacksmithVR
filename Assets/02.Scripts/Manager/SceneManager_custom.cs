using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager_custom : MonoBehaviour
{
    public void SceneChange()
    {
        StartCoroutine(Loading());
    }

    public void SceneExit()
    {
        StartCoroutine(Quitting());
    }

    private IEnumerator Loading()
    {
        yield return new WaitForSeconds(2.0F);
        SceneManager.LoadScene("Tutorial (COPY)");
    }

    private IEnumerator Quitting()
    {
        yield return new WaitForSeconds(2.0F);
        Application.Quit();
    }
}

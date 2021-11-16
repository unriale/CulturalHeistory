using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void OnEnable()
    {
        ThiefFound.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        ThiefFound.GameOver -= OnGameOver;
    }

    private void OnGameOver()
    {
        StartCoroutine(ReloadLevel());
    }

    IEnumerator ReloadLevel()
    {
        Fader fader = FindObjectOfType<Fader>();
        yield return fader.FadeOut();
        yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        yield return fader.FadeIn();
    }
}

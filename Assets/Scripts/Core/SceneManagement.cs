using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    private float _reloadDelay = 1.5f;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadMenu()
    {
        ResetGame();
        SceneManager.LoadScene(0);
    }

    public void ReloadGame()
    {
        ResetGame();
        StartGame();
    }

    private void ResetGame()
    {
        Time.timeScale = 1;
        var spawner = FindObjectOfType<PersistentObjectsSpawner>();
        if (spawner) spawner.DestroyPersistentGameObjects();
        else { Debug.LogError("PersistentObjectsSpawner was not found!!!"); }
        Coin.ReloadCoins();
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
        StartCoroutine(ReloadLevel(_reloadDelay));
    }

    private IEnumerator ReloadLevel(float delay)
    {
        Fader fader = FindObjectOfType<Fader>();
        yield return new WaitForSeconds(delay);
        yield return fader.FadeOut();
        yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        FindObjectOfType<Pause>().ChangeTreasuresUI();
        yield return fader.FadeIn();
    }
}

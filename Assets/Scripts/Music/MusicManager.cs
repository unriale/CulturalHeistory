using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] musics;

    private AudioSource _audioSource;

    private static MusicManager _instance;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else // mmmh ?
        {
            Destroy(this.gameObject);
        }
        _audioSource.clip = musics[0];
        _audioSource.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name.Equals("Central") && (!_audioSource.clip.name.Equals(musics[0].name)))
        {
            _audioSource.Stop();
            _audioSource.clip = musics[0];
            _audioSource.Play();
        }
        else if (SceneManager.GetActiveScene().name.Equals("Up") && (!_audioSource.clip.name.Equals(musics[1].name)))
        {
            _audioSource.Stop();
            _audioSource.clip = musics[1];
            _audioSource.Play();
        }
        else if (SceneManager.GetActiveScene().name.Equals("Right") && (!_audioSource.clip.name.Equals(musics[2].name)))
        {
            _audioSource.Stop();
            _audioSource.clip = musics[2];
            _audioSource.Play();
        }
        else if (SceneManager.GetActiveScene().name.Equals("Down") && (!_audioSource.clip.name.Equals(musics[3].name)))
        {
            _audioSource.Stop();
            _audioSource.clip = musics[3];
            _audioSource.Play();
        }
    }
}

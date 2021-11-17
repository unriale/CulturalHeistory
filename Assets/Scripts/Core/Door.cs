using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    enum DoorTo { Central, Up, Down, Right };
    enum DestinationIdentifier { A, B, C };

    [SerializeField] DoorTo doorTo;
    [SerializeField] DestinationIdentifier destination;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Player") && FindObjectOfType<Fader>().HasFadedIn())
        {
            StartCoroutine(Transition());
        }
    }

    private IEnumerator Transition()
    {
        Fader fader = FindObjectOfType<Fader>();
        DontDestroyOnLoad(gameObject);
        yield return fader.FadeOut();
        yield return SceneManager.LoadSceneAsync(doorTo.ToString());
        Door otherDoor = GetOtherDoor();
        Pause pause = FindObjectOfType<Pause>();
        pause.ChangeTreasuresUI();
        UpdatePlayer(otherDoor);
        yield return fader.FadeIn();
        Destroy(gameObject);
    }

    private void UpdatePlayer(Door otherDoor)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<CharacterController>().enabled = false; // used because controller overrides player's position
        player.transform.position = otherDoor.transform.GetChild(0).transform.position;
        player.GetComponent<CharacterController>().enabled = true;
    }

    private Door GetOtherDoor()
    {
        foreach (Door door in FindObjectsOfType<Door>())
        {
            if (door == this) continue;
            else if (door.destination == this.destination)
                return door;
        }
        return null;
    }
}

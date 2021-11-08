using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Tooltip("Time in seconds to delay destroy")]
    [SerializeField] float destroyDelay = 2f;
    [SerializeField] Treasure _treasure;
    public float PickupRadius = 2f;

    private bool isPickedUp = false;
    private Transform player;

    private void Awake()
    {
        _treasure.currentAmount = 0;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        TryToBePickedUp();
    }

    private void TryToBePickedUp()
    {
        if (PlayerIsFarAway()) return;
        if (Input.GetKeyDown(KeyCode.E))
            if (!isPickedUp) PickUpItem();
    }

    private bool PlayerIsFarAway()
    {
        return Vector3.Distance(gameObject.transform.position, player.transform.position) > PickupRadius;
    }

    private void PickUpItem()
    {
        print("Picking up animation");
        isPickedUp = true;
        // TODO: events for UI
        if (_treasure.currentAmount < _treasure.maxAmount) _treasure.currentAmount++; 
        // TODO: VFX + SFX
        Destroy(gameObject, destroyDelay);
    }
}

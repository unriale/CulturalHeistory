using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Tooltip("Time in seconds to delay destroy")]
    [SerializeField] float destroyDelay = 2f;
    [SerializeField] Treasure _treasure;
    [SerializeField] Slot slot;
    public float PickupRadius = 2f;

    private bool isPickedUp = false;
    private Transform player;

    private void Start()
    {
        _treasure.currentAmount = 0;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        slot.SetInitialTextAmount();
    }

    private void Update()
    {
        TryToBePickedUp();
    }

    private void TryToBePickedUp()
    {
        if (PlayerIsFarAway()) return;
        if (Input.GetKeyDown(KeyCode.E))
            if (!isPickedUp) StartCoroutine(PickUpItem());
    }

    private bool PlayerIsFarAway()
    {
        return Vector3.Distance(gameObject.transform.position, player.transform.position) > PickupRadius;
    }

    IEnumerator PickUpItem()
    {
        print("Picking up animation");
        isPickedUp = true;
        if (_treasure.currentAmount < _treasure.maxAmount) _treasure.currentAmount++;
        player.GetComponent<Movement>().DisableMovement();
        slot.UpdateAmount(GetAmountLeft(), destroyDelay);
        yield return new WaitForSeconds(destroyDelay);
        player.GetComponent<Movement>().EnableMovement();
        // TODO: VFX + SFX
        Destroy(gameObject);
    }

    private int GetAmountLeft()
    {
        return _treasure.maxAmount - _treasure.currentAmount;
    }
}

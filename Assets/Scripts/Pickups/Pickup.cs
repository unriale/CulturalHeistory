using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Pickup : MonoBehaviour
{
    [Header("Pickup settings")]
    [Tooltip("Time in seconds to delay destroy")]
    [SerializeField] float destroyDelay = 2f;
    [SerializeField] Treasure _treasure;
    [SerializeField] string slotTag = "";
    [SerializeField] string id = "";
    public float PickupRadius = 2f;

    [Header("Cursor settings")]
    public Texture2D pickupCursorTexture;
    public Texture2D gameCursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    private bool isPickedUp = false;
    private Transform player;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }

    private void Start()
    {
        //_treasure.currentAmount = 0;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        //GameObject.FindGameObjectWithTag(slotTag).GetComponent<Slot>().SetInitialTextAmount();
    }

    private void Update()
    {
        TryToBePickedUp();
    }

    private void TryToBePickedUp()
    {
        if (PlayerIsFarAway()) return;
        if (Input.GetKeyDown(KeyCode.E))
            if (!isPickedUp) {
                _audioSource.Play(); // Play PickUp SFX
                StartCoroutine(PickUpItem()); 
            }
    }

    private bool PlayerIsFarAway()
    {
        return Vector3.Distance(gameObject.transform.position, player.transform.position) > PickupRadius;
    }

    IEnumerator PickUpItem()
    {
        PlayerPrefs.SetInt(id, 1); // 1-picked up
        FindObjectOfType<WinCondition>().DecrementTreasures();
        // Play Steal animation HERE (UNCOMMENT THIS) - TODO: Implement the PlayStealAnimation method
        player.GetComponent<Movement>().PlayStealAnimation(transform.position, gameObject);
        // --------
        isPickedUp = true;
        if (_treasure.currentAmount < _treasure.maxAmount) _treasure.currentAmount++;
        player.GetComponent<Movement>().DisableMovement();
        if (GameObject.FindGameObjectWithTag(slotTag))
        {
            GameObject.FindGameObjectWithTag(slotTag).GetComponent<Slot>().UpdateAmount(GetAmountLeft(), destroyDelay);
        }
        yield return new WaitForSeconds(destroyDelay);
        player.GetComponent<Movement>().EnableMovement();
        // TODO: VFX + SFX // You can't play a SFX here, the object will be destroyed soon after
        Destroy(gameObject);
    }

    private int GetAmountLeft()
    {
        return _treasure.maxAmount - _treasure.currentAmount;
    }

    private void OnMouseEnter()
    {
        Cursor.SetCursor(pickupCursorTexture, hotSpot, cursorMode);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(gameCursorTexture, hotSpot, cursorMode);
    }
}

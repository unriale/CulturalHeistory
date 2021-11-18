using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerMenu : Movement
{
    private float horizontalMoveVal = 1;
    private float buttonPos;
    private float position1 = 0.3f;
    private float position2 = 2f;
    [SerializeField] RectTransform options;
    [SerializeField] RectTransform start;

    private void OnTriggerEnter(Collider other)
    {
        horizontalMoveVal = horizontalMoveVal == 1 ? -1 : 1;
        buttonPos = buttonPos == position1 ? position2 : position1;
        options.position = new Vector3(options.position.x, options.position.y, buttonPos);
        start.position = new Vector3(start.position.x, start.position.y, buttonPos);
    }

    protected override void Move()
    {
        if (!_canMove) return;
        float horizontalInput = horizontalMoveVal;
        float verticalInput = 0;

        if (base.Walking()) base.ChangeStateToWalk();
        else { ChangeStateToStay(); }

        StartMoving(horizontalInput, verticalInput);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerMenu : Movement
{
    private float horizontalMoveVal = 1;

    private void OnTriggerEnter(Collider other)
    {
        horizontalMoveVal = horizontalMoveVal == 1 ? -1 : 1;
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

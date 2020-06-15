﻿using UnityEngine;

public interface IRotateInput
{
    Vector2 GetTargetRotationPosition();

    bool LookAtMouse();
}

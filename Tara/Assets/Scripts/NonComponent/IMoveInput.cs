using UnityEngine;

public interface IMoveInput
{
    Vector2 GetInput();

    float GetSpeedMultiplier();
}

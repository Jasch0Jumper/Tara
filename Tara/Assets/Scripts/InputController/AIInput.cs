using UnityEngine;

[RequireComponent(typeof(IAIInput))]
public class AIInput : MonoBehaviour, IRotateInput, IMoveInput, IShootInput
{
    [SerializeField] [Range(1f, 5f)] private float fastSpeedMultiplier = 1f;
    [SerializeField] [Range(0.1f, 1f)] private float slowSpeedMultiplier = 1f;
    private float speedMultiplier = 1f;

    private PathFinder pathFinder;
    private IAIInput aIInput;

    private bool active = false;
    private bool isShooting;

    private Vector2 input;
    private Vector2 targetRotationPosition;

    private void Awake()
    {
        aIInput = GetComponent<IAIInput>();
        pathFinder = GetComponent<PathFinder>();
    }
    private void Start()
    {
        active = true;
    }
    private void Update()
    {
        if (aIInput.UseFastSpeed()) { speedMultiplier = fastSpeedMultiplier; }
        else if (aIInput.UseSlowSpeed()) { speedMultiplier = slowSpeedMultiplier; } 
        else { speedMultiplier = 1f; }
        

        targetRotationPosition = aIInput.GetTargetPosition();

        //pathFinder.PathFindTo(aIInput.GetTargetPosition());

        Vector2 targetInput = aIInput.GetTargetPosition() - transform.position;
        
        if (aIInput.HasArrived()) 
        { input = Vector2.zero; }
        else 
        { input = Vector2.ClampMagnitude(targetInput, 1f); }

        if (aIInput.CanShoot()) { isShooting = true; }
        else { isShooting = false; }
    }

    #region IInputController implementation
    public bool IsActive() { return active; }

    public Vector2 GetInput() { return input; }
    public Vector2 GetTargetRotationPosition() { return targetRotationPosition; }

    public float GetSpeedMultiplier() { return speedMultiplier; }

    public bool LookAtMouse() { return false; }

    public bool IsShooting() { return isShooting; }
    #endregion
}

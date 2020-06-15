using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Lifetime : MonoBehaviour
{
    public float time = 1f;
    [SerializeField] private UnityEvent onEndOfLifeTime = new UnityEvent();

    private void Start()
    {
        StartCoroutine(LifeTime());
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(time);

        onEndOfLifeTime?.Invoke();
    }
}

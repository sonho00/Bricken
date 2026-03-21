using UnityEngine;
using System.Collections;

public class BreakEffect : MonoBehaviour
{
    private float breakDelay;

    private void Awake()
    {
        breakDelay = GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;
    }

    private void OnEnable()
    {
        StartCoroutine(DisableAfterDelay());
    }

    private IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds(breakDelay);
        gameObject.SetActive(false);
    }
}

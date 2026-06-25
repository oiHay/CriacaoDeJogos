using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerSceneAnimation : MonoBehaviour
{
    [Header("Entry")]
    [SerializeField] private float offScreenZ = -20f;
    [SerializeField] private float entryZ = -4f;
    [SerializeField] private float duration = 1.5f;
    [SerializeField] private float delayToStart = 0.5f;
    [SerializeField] private Ease ease = Ease.OutCubic;

    [Header("Exit")] [SerializeField] private float exitZ = 12f;
    [SerializeField] private float exitDuration = 1.5f;
    [SerializeField] private Ease exitEase = Ease.InCubic;

    private void Start()
    {
        StartCoroutine(EntryAnim());
    }

    private IEnumerator EntryAnim()
    {
        yield return new WaitForSeconds(delayToStart);
        
        Vector3 startPos = transform.position;
        startPos.z = offScreenZ;
        transform.position = startPos;

        transform.DOMoveZ(entryZ, duration)
            .SetEase(ease);
    }

    public void PlayExitAnimation()
    {
        transform.DOMoveZ(exitZ, exitDuration)
            .SetEase(exitEase)
            .OnComplete(() => CustomSceneManager.LoadNextScene());
    }
}

using System;
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

    [Header("Exit")] 
    [SerializeField] private float exitZ = 12f;
    [SerializeField] private float exitDuration = 1.5f;
    [SerializeField] private Ease exitEase = Ease.InCubic;

    [Header("Return")] 
    [SerializeField] private Vector3 homePosition = new Vector3(0f, 0.6f, -5f);
    [SerializeField] private float returnDuration = 1f;
    [SerializeField] private Ease returnEase = Ease.OutCubic;

    [Header("Sound")] 
    [SerializeField] private AudioClip entryAudio;
    [SerializeField] private AudioClip exitAudio;

    public static PlayerSceneAnimation Instance { get; private set; }
    
    private void Awake() => Instance = this;
    
    public void PlayEnterAnimation(Action onComplete = null)
    {
        StartCoroutine(EntryAnim(onComplete));
    }

    private IEnumerator EntryAnim(Action onComplete)
    {
        yield return new WaitForSeconds(delayToStart);
        
        Vector3 startPos = transform.position;
        startPos.z = offScreenZ;
        transform.position = startPos;

        AudioManager.Instance.PlaySfx(entryAudio);
        
        transform.DOMoveZ(entryZ, duration)
            .SetEase(ease)
            .OnComplete(() => onComplete?.Invoke());
    }

    public void PlayExitAnimation(Action onComplete)
    {
        GameManager.Instance.ChangeState(GameState.ExitLevel);
        
        AudioManager.Instance.PlaySfx(exitAudio);
        
        transform.DOMoveZ(exitZ, exitDuration)
            .SetEase(exitEase)
            .OnComplete(() =>
            {
                if (onComplete != null)
                    onComplete.Invoke();
                else
                    CustomSceneManager.LoadNextScene();
            });
    }

    public void PlayerReturnAnimation(Action onComplete)
    {
        transform.DOMove(homePosition, returnDuration)
            .SetEase(returnEase)
            .OnComplete(() => onComplete?.Invoke());
    }
}

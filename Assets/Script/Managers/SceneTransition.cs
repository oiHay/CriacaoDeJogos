using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance { get; private set; }
    public static bool IsTransitioning => Instance != null && Instance._transitioning;

    [SerializeField] private Image overlay;
    [SerializeField] private float duration = 0.5f;

    private static readonly int Progress = Shader.PropertyToID("_Progress");
    private static readonly int Invert = Shader.PropertyToID("_Invert");

    private Material _material;
    private bool _transitioning;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        
        _material = Instantiate(overlay.material);
        overlay.material = _material;
        _material.SetFloat(Progress, 0f);
        _material.SetFloat(Invert, 0f);
        overlay.gameObject.SetActive(false);
    }

    public static void LoadScene(int buildIndex)
    {
        if (Instance != null && !Instance._transitioning)
            Instance.StartCoroutine(Instance.Transition(buildIndex));
        else if (Instance == null)
            SceneManager.LoadScene(buildIndex);
    }

    private IEnumerator Transition(int buildIndex)
    {
        _transitioning = true;
        overlay.gameObject.SetActive(true);
        
        if (GameManager.Instance != null) 
            GameManager.Instance.ChangeState(GameState.Transition);

        _material.SetFloat(Invert, 0f);
        yield return Animate(0f, 1f);          

        var loading = SceneManager.LoadSceneAsync(buildIndex);
        while (!loading.isDone)
            yield return null;

        _material.SetFloat(Progress, 0f);      
        _material.SetFloat(Invert, 1f);       
        yield return Animate(0f, 1f);       

        overlay.gameObject.SetActive(false);
        _transitioning = false;
    }

    private IEnumerator Animate(float from, float to)
    {
        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            _material.SetFloat(Progress, Mathf.Lerp(from, to, t/duration));
            yield return null;
        }
        _material.SetFloat(Progress, to);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoaderManager : MonoBehaviour
{
    public static LevelLoaderManager Instance { get; private set; }
    
    [Header("Transition")]
    public Animator transition;
    public float transitionTime = 1f;
    
    [Header("Loading Screen")]
    public GameObject loadingScreen;
    public Slider progressBar;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }
    
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadLevel(SceneManager.GetSceneByName(sceneName).buildIndex));
    }

    private IEnumerator LoadLevel(int levelIndex)
    {
        // Busca o Animator na cena atual se não estiver atribuído
        if (transition == null)
        {
            GameObject obj = GameObject.FindWithTag("Transition");
            if (obj != null)
                transition = obj.GetComponent<Animator>();
        }

        // 1. Play transition animation
        if (transition != null)
            transition.SetTrigger("Start");
        
        yield return new WaitForSeconds(transitionTime);

        // 2. Show loading screen
        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        // 3. Begin async load (but don't activate yet)
        AsyncOperation op = SceneManager.LoadSceneAsync(levelIndex);
        op.allowSceneActivation = false;

        // 4. Update progress bar while loading
        while (op.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);

            if (progressBar != null)
                progressBar.value = progress;

            yield return null;
        }

        // 5. Loading done — fill bar to 100% then activate
        if (progressBar != null)
            progressBar.value = 1f;

        yield return new WaitForSeconds(0.5f);

        // Limpa a referência para buscar de novo na próxima cena
        transition = null;

        op.allowSceneActivation = true;
    }
}

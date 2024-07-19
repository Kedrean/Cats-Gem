using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    public Animator animator;
    public string sceneToLoad;

    private void Start()
    {
        
    }

    public void FadeToScene(string sceneName)
    {
        sceneToLoad = sceneName;
        animator.SetBool("FadeOut", true);
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(sceneToLoad);
        // Reset the fade state after scene load
        animator.SetBool("FadeOut", false);
    }
}

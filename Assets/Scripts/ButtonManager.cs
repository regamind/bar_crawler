using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    private AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void NextScene(string Scene)
    {
        audioSource.Play();

        StartCoroutine(DelaySceneLoad(Scene));
              
    }

    IEnumerator DelaySceneLoad(string Scene)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadSceneAsync(Scene);
    }
}

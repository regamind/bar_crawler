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
        SceneManager.LoadSceneAsync(Scene);        
    }
}

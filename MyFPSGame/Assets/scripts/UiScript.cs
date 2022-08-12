using UnityEngine;
using UnityEngine.SceneManagement;

namespace StarterAssets{
public class UiScript : MonoBehaviour
{
    public void Play()
    {
        Cursor.lockState=CursorLockMode.Locked;
        SceneManager.LoadScene("Map_v1");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
}

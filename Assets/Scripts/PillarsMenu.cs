using UnityEngine;
using UnityEngine.SceneManagement;

public class PillarsMenu : MonoBehaviour
{
    public void OnReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}

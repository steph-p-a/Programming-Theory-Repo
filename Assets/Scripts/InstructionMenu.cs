using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionMenu : MonoBehaviour
{
    public void OnReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}

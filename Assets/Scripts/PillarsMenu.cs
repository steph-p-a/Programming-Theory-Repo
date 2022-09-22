using UnityEngine;
using UnityEngine.SceneManagement;

public class PillarsMenu : MonoBehaviour
{
    [SerializeField] GameObject encapsulation;
    [SerializeField] GameObject abstraction;
    [SerializeField] GameObject inheritance;
    [SerializeField] GameObject polymorphism;
    private void Start()
    {
        SelectPanel(true, false, false, false);
    }

    private void SelectPanel(bool enc, bool abs, bool inh, bool poly)
    {
        encapsulation.SetActive(enc);
        abstraction.SetActive(abs);
        inheritance.SetActive(inh);
        polymorphism.SetActive(poly);
    }

    public void OnEncapsulationClicked()
    {
        SelectPanel(true, false, false, false);
    }
    public void OnAbstractionClicked()
    {
        SelectPanel(false, true, false, false);
    }

    public void OnInheritanceClicked()
    {
        SelectPanel(false, false, true, false);
    }

    public void OnPolymorphismClicked()
    {
        SelectPanel(false, false, false, true);
    }

    public void OnReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}

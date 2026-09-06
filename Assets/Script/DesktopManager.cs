using UnityEngine;

public class DesktopManager : MonoBehaviour
{
    public GameObject windowFileManager;
    public GameObject windowTerminal;

    void Start()
    {
        windowFileManager.SetActive(false);
        windowTerminal.SetActive(false);
    }

    public void BukaFileManager() { windowFileManager.SetActive(true); }
    public void TutupFileManager() { windowFileManager.SetActive(false); }
    
    public void BukaTerminal() { windowTerminal.SetActive(true); }
    public void TutupTerminal() { windowTerminal.SetActive(false); }
}
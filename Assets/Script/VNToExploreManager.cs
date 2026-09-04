using UnityEngine;

public class VNToExploreManager : MonoBehaviour
{
    public GameObject dialogSystemUI;
    public MonoBehaviour houseClickScript;
    public MonoBehaviour cameraPivotScript; 

    public void SelesaiVN()
    {
        dialogSystemUI.SetActive(false);
        
        if(houseClickScript != null) houseClickScript.enabled = true;
        if(cameraPivotScript != null) cameraPivotScript.enabled = true;
    }
}
using UnityEngine;

public class HouseClickTransition : MonoBehaviour
{
    public string namaSceneTujuan = "Template Indoor";
    public SceneFader fader;

    private void OnMouseDown()
    {
       
        if (!enabled) return; 

        if (fader != null)
        {
            fader.MulaiPindahScene(namaSceneTujuan);
        }
    }
}
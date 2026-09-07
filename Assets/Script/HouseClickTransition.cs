using UnityEngine;

public class HouseClickTransition : MonoBehaviour
{
    public string namaSceneTujuan = "Stage 1";
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
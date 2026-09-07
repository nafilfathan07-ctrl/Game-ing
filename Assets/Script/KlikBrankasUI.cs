using UnityEngine;

public class KlikBrankasUI : MonoBehaviour
{
    // Nggak pakai public lagi, biar nggak usah repot di Inspector
    private BrankasSystem brankasSystem; 
    private int jumlahKlik = 0;

    void Start()
    {
        // Script ini bakal otomatis nyari BrankasSystem yang nempel di objek yang sama!
        brankasSystem = GetComponent<BrankasSystem>();
    }

    void OnMouseDown()
    {
        jumlahKlik++;

        // KLIK 1: Biarkan SmoothZoomController kamu yang bekerja.
        
        // KLIK 2: Munculkan UI Numpad
        if (jumlahKlik == 2)
        {
            if (brankasSystem != null)
            {
                brankasSystem.BukaUIPin();
            }
            else
            {
                Debug.LogWarning("Script BrankasSystem belum dipasang di objek ini!");
            }
        }
    }

    public void ResetHitunganKlik()
    {
        jumlahKlik = 0;
    }
}
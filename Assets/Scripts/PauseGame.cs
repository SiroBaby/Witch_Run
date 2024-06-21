using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    private bool isPause = false;
    public void TogglePause () {
        isPause = !isPause;
        if (isPause){
            Time.timeScale = 0;
            //này thêm thử để check coi code được push lên chưa
        }
    }
}

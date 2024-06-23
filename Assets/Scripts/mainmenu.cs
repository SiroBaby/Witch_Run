using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour
{
    public void PlayGame ()
    {
        SceneManager.LoadScene ("SampleScene");
    }
    public void Shop () 
    {
        SceneManager.LoadScene ("Shop");
    }
    public void Setting () {
        SceneManager.LoadScene ("Setting");
    }
}

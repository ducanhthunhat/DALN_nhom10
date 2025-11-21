using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISellectLevel : UICanvas
{
    public void OnClickLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void OnClickLevel2()
    {
        SceneManager.LoadScene("Level2");

    }

    public void OnClickLevel3()
    {
        SceneManager.LoadScene("Level3");

    }
}

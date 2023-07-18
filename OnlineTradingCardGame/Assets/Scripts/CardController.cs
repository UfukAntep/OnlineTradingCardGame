using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardController : MonoBehaviour
{
   
    public void CameraRefresh()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}

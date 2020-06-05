using System.Collections;
using TMPro;
using UnityEngine;

//script used to manage the intro panel for each scene
//it toggles on and off the information displayed
//based on a coroutine (10 seconds)
public class IntroManager : MonoBehaviour
{
    [SerializeField] private GameObject introCanvas;
       
    void Start()
    {
        StartCoroutine(DisplayInfo());
    }

    private IEnumerator DisplayInfo()
    {
        yield return new WaitForSeconds(10);
        introCanvas.SetActive(false);
    }

    public void Hide()
    {
        introCanvas.SetActive(false);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject uiMenuRoot;
    public GameObject uiMM;
    public GameObject uiOption;
    public GameObject uiAP;
    public GameObject uiIGM;

    // Start is called before the first frame update
    void Start()
    {
        uiMenuRoot.SetActive(true);
        uiMM.SetActive(true);
        uiOption.SetActive(false);
        uiAP.SetActive(false);
        uiIGM.SetActive(false);
    }

    public void ShowOption() {
        uiMM.SetActive(false);
        uiOption.SetActive(true);
    }

    public void ShowMM() {
        uiMM.SetActive(true);
        uiOption.SetActive(false);
    }

    public void ShowUIAP() {
        uiMenuRoot.SetActive(false);
        uiAP.SetActive(true);
    }

    public void OnPlay() {
        ShowUIAP();
        GameController.instance.PlayGame();
    }

    public void OnExit() {

    }

    public void OnPause() {
        Time.timeScale = 0;
        uiIGM.SetActive(true);
    }

    public void OnResume() {
        Time.timeScale = 1;
        uiIGM.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

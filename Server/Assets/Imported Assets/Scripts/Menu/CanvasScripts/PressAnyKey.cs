using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAnyKey : MonoBehaviour
{
    public GameObject pressAnyKeyPanel, menuPanel;
    // Start is called before the first frame update
    void Start()
    {
        menuPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            menuPanel.SetActive(true);
            pressAnyKeyPanel.SetActive(false);
        }
    }
}

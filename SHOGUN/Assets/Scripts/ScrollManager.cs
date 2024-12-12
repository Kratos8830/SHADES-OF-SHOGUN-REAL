using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollManager : MonoBehaviour
{
    public TMP_Text currentScrolls;
    public GameController gc;

    private void Start()
    {
        currentScrolls.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        currentScrolls.text= " "+ gc.currentScrolls.text;
    }
}

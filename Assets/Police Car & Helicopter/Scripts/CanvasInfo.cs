using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasInfo : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    public static CanvasInfo instance;

    private void Awake()
    {
        instance = this;
        infoText.enabled = false;
    }
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        infoText.enabled = true;
        yield return new WaitForSeconds(7f);
        infoText.enabled = false;
    }
    public void SetInfoText(string s)
    {
        infoText.enabled = true;
        infoText.text = s;
    }
}

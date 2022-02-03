using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager singleton;

    private void Awake()
    {
        singleton = this;
        selectionInfoPanel.gameObject.SetActive(false);
    }

    [SerializeField]
    SelectionInfoPanel selectionInfoPanel;
    public void UpdateSelectionPanel(bool show, Clickable selection)
    {
        if (show)
        {
            selectionInfoPanel.gameObject.SetActive(true);
            selectionInfoPanel.Setup(selection);
        }
        else
        {
            selectionInfoPanel.gameObject.SetActive(false);
            selectionInfoPanel.Setup(null);
        }
    }
}

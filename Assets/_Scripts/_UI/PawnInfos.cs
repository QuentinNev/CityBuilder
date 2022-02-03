using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PawnInfos : MonoBehaviour
{
    [HideInInspector]
    public Pawn watchedPawn;

    [SerializeField]
    Bar healthBar;

    #region Stats
    [SerializeField]
    TextMeshProUGUI strText;
    [SerializeField]
    TextMeshProUGUI conText;
    [SerializeField]
    TextMeshProUGUI agiText;
    [SerializeField]
    TextMeshProUGUI intText;
    [SerializeField]
    TextMeshProUGUI chaText;
    #endregion

    [SerializeField]
    TextMeshProUGUI status;  
    [SerializeField]
    ItemList itemList;

    public void Setup(Pawn pawn)
    {
        watchedPawn = pawn;
        Subscribe(watchedPawn);
        SetInfos(pawn.strength, pawn.constitution, pawn.agility, pawn.intelligence, pawn.charisma);
        pawn.RefreshUI();
    }

    public void SetInfos(int str, int con, int agi, int intel, int cha)
    {
        strText.SetText(str.ToString());
        conText.SetText(con.ToString());
        agiText.SetText(agi.ToString());
        intText.SetText(intel.ToString());
        chaText.SetText(cha.ToString());
    }

    public void Subscribe(Pawn pawn)
    {
        pawn.HealthUpdate += UpdateHealthBar;
        pawn.StatusUpdate += UpdateStatus;
        pawn.InventoryUpdate += UpdateItemList;
    }

    public void Unsubscribe(Pawn pawn)
    {
        pawn.HealthUpdate -= UpdateHealthBar;
        pawn.StatusUpdate -= UpdateStatus;
        pawn.InventoryUpdate -= UpdateItemList;
    }

    public void UpdateStatus(object sender, System.EventArgs e)
    {
        status.SetText((sender as Pawn).Status);
    }

    public void UpdateHealthBar(object sender, System.EventArgs e)
    {
        healthBar.UpdatebBar((sender as Pawn).CurrentHealth, (sender as Pawn).baseHealth);
    }    

    public void UpdateItemList(object sender, System.EventArgs e)
    {
        Pawn pawn = (sender as Pawn);
        itemList.UpdateList(pawn.inventory, pawn.invTotalWeight, pawn.maxWeight);
    }
}
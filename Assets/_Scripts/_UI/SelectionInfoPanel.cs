using UnityEngine;

public class SelectionInfoPanel : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI titleText;
    [SerializeField]
    PawnInfos pawnInfo;
    [SerializeField]
    StockpileInfo stockpileInfo;

    public void Setup(Clickable selected)
    {
        if (selected)
        {
            if (pawnInfo.watchedPawn)
                pawnInfo.Unsubscribe(pawnInfo.watchedPawn);

            if (stockpileInfo.watchedStockpile)
                stockpileInfo.Unsubscribe(stockpileInfo.watchedStockpile);

            titleText.SetText(selected.name);

            pawnInfo.gameObject.SetActive(false);
            stockpileInfo.gameObject.SetActive(false);

            switch (selected)
            {
                case Pawn pawn:
                    pawnInfo.gameObject.SetActive(true);
                    pawnInfo.Setup(pawn);
                    break;

                case Stockpile stockpile:
                    stockpileInfo.gameObject.SetActive(true);
                    stockpileInfo.Setup(stockpile);
                    break;
            }
        }
        else
        {

        }
    }
}

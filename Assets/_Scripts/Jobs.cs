using System.Collections.Generic;

public class Haul : Job
{
    public Item item;
    public Stockpile stockpileB;

    public Haul(Pawn worker, ProductionBuilding from, Item what, Stockpile to)
    {
        assignedPawn = worker;
        sourceBuilding = from;
        item = what;
        stockpileB = to;
    }
}

public class Produce : Job
{
    public Produce(Pawn worker, ProductionBuilding workplace)
    {
        assignedPawn = worker;
        sourceBuilding = workplace;
    }
}

public class Craft : Job
{
    public List<Item> usedIngredients;

    public Craft(Pawn worker, ProductionBuilding workplace)
    {
        assignedPawn = worker;
        sourceBuilding = workplace;
    }
}
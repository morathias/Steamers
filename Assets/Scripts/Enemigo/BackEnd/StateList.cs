
public enum Pattern //Edit
{

    RELOAD,
    AIMING,
    ATTACK,
    FEAR,
    PROTECT,
    GUARD,
    PATROL,
    MOVING,
}

public enum State
{
    NORMAL,
    ALARM,
    AGGRESIVE,
    DEAD,
    STUNNED,
    FORMATION,
    countState
}

public enum Events
{
    findGratmos,
    //suspicious,
    loseGratmos,
    dead,
    //  blunted,
    // recover,
    //capIsHere,
    countEvents
}
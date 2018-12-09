
public enum Pattern //Edit
{

    RELOAD,
    AIMING,
    ATTACK,
    FEAR,
    PROTECT,
    Guard,
    Patrol,
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
    suspicious,
    loseEnemy,
    blunted,
    recover,
    capIsHere,
    countEvents
}
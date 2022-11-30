using System;

public class EventBroker
{
    public static event Action EnemyKilled;

    public static void CallEnemyKilled()
    {
        if (EnemyKilled != null)
            EnemyKilled();
    }
}

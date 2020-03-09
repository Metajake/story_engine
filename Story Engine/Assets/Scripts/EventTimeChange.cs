using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTimeChange: IGameEvent {
    private string eventType = "TIMEEVENT";

    public string getEventType()
    {
        return this.eventType;
    }
}

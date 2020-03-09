using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSceneChange : IGameEvent {
    private string eventType = "LOCATIONEVENT";

    public string getEventType()
    {
        return this.eventType;
    }
}

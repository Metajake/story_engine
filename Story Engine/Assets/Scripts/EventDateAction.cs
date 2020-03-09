using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDateAction : IGameEvent {
    private string eventType = "DATEACTIONEVENT";

    public string getEventType()
    {
        return this.eventType;
    }
}

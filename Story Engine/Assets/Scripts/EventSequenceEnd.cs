using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSequenceEnd : IGameEvent {
    private string eventType = "SEQUENCEENDEVENT";

    public string getEventType()
    {
        return this.eventType;
    }
}

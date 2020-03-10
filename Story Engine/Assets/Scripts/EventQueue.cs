using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventQueue : MonoBehaviour {

    Queue<IGameEvent> gameEvents;
    Queue<IGameEvent> pastEvents;
    List<IEventSubscriber> eventSubscribers;


    // Use this for initialization
    void Awake () {
        gameEvents = new Queue<IGameEvent>();
        pastEvents = new Queue<IGameEvent>();
        eventSubscribers = new List<IEventSubscriber>();
    }
	
	// Update is called once per frame
	void Update () {
		while (gameEvents.Count > 0){
            var thisEvent = gameEvents.Peek();
            foreach(IEventSubscriber subscriber in eventSubscribers) {
                subscriber.eventOccurred(thisEvent);
            }
            moveProcessedEventToPastEvents();
        }
	}

    public void queueEvent(IGameEvent eventToAdd)
    {
        gameEvents.Enqueue(eventToAdd);
    }
   
    private void moveProcessedEventToPastEvents() {
        pastEvents.Enqueue(gameEvents.Dequeue());
    }

    public void subscribe(IEventSubscriber toSubscribe)
    {
        eventSubscribers.Add(toSubscribe);
    }
}

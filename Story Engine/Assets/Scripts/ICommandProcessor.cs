using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface ICommandProcessor {
    void createAndEnqueueChangeDialogueSequence(List<string> dialogues);
    void executeNextCommand();
}

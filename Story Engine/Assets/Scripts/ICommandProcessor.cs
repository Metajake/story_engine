using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface ICommandProcessor {
    void doSequence(List<ICommand> commandSequence);
}

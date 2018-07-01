using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialoguePiece {

	public Character speaker;
    public string dialogueContent;
    List<string> tags;
    List<DialoguePiece> responses;

	public DialoguePiece(string content, Character character){
		this.dialogueContent = content;
		this.speaker = character;
		this.tags = new List<string>();
		this.responses = new List<DialoguePiece>();
	}

	public DialoguePiece addTag(string tag){
		this.tags.Add(tag);
		return this;
	}

	public DialoguePiece addResponse(DialoguePiece response){
		this.responses.Add(response);
		return this;
	}

	public bool matchesExactly(List<string> queryTags){
		bool toReturn = true;
		foreach (string tag in queryTags){
			if(!tags.Any(t => t.ToString() == tag)){ //If Incoming List Item doesn't match our tag list
				toReturn = false;
			}
		}
		if(queryTags.Count != tags.Count){
			toReturn = false;
		}
		return toReturn;
	}

    //0 if you have all of the query tags, or a positive # of how many fewer you have if you don't have all
	public int matchesPartially(List<string> queryTags){
		int toReturn = 0;
		foreach(string tag in queryTags){
			if(!tags.Any(t => t.ToString() == tag)){
				toReturn++;
			}
		}
		return toReturn;
	}
}
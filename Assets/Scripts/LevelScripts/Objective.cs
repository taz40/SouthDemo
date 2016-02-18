using UnityEngine;
using System.Collections;
using System;

public class Objective {
    Action objectiveSuccess;
    Action objectiveFailure;
    public string objectiveName;
	public bool periodBound;

	public Objective(string name, bool periodBound = true){
		this.periodBound = periodBound;
		objectiveName = name;
	}

    public void completed() {
		if (objectiveSuccess != null)
			objectiveSuccess ();
    }

    public void failed() {
		if (objectiveFailure != null)
			objectiveFailure ();
    }

	public void RegisterObjectiveSuccessCallback(Action cb){
		objectiveSuccess += cb;
	}

	public void UnregisterObjectiveSuccessCallback(Action cb){
		objectiveSuccess -= cb;
	}

	public void RegisterObjectiveFailureCallback(Action cb){
		objectiveFailure += cb;
	}
	
	public void UnregisterObjectiveFailureCallback(Action cb){
		objectiveFailure -= cb;
	}
}

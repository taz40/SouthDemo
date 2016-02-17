using UnityEngine;
using System.Collections.Generic;

public class Period {
	public string name;
	public int startTime;
	public List<Objective> objectives;

	public Period(string name, int startTime){
		this.name = name;
		this.startTime = startTime;
		objectives = new List<Objective> ();
	}
}

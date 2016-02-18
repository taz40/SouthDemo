using UnityEngine;
using System.Collections.Generic;

public class Period {
	public string name;
	public int startTime;
	public List<Objective> objectives;

	public Period(int startTime, string name){
		this.name = name;
		this.startTime = startTime;
		objectives = new List<Objective> ();
	}
}

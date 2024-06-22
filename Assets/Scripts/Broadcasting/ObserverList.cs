using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Holds the associated actions associated with the event name
 * Created By: NeilDG
 */
public class ObserverList {
	private List<System.Action<Parameters>> eventListeners;	//by default, event listeners with params
	private List<System.Action> eventListenersNoParams; //event listeners that does not have params;

	public ObserverList() {
		this.eventListeners = new List<System.Action<Parameters>>();
		this.eventListenersNoParams = new List<System.Action>();
	}

	public void AddObserver(System.Action<Parameters> action) {
		this.eventListeners.Add(action);
	}

	public void AddObserver(System.Action action) {
		this.eventListenersNoParams.Add(action);
	}

	public void RemoveObserver(System.Action<Parameters> action) {
		if(this.eventListeners.Contains(action)) {
			this.eventListeners.Remove(action);
		}
	}

	public void RemoveObserver(System.Action action) {
		if(this.eventListenersNoParams.Contains(action)) {
			this.eventListenersNoParams.Remove(action);
		}
	}

	public void RemoveAllObservers() {
		this.eventListeners.Clear();
		this.eventListenersNoParams.Clear();
	}

	public void NotifyObservers(Parameters parameters) {
		for(int i = 0; i < this.eventListeners.Count; i++) {
			System.Action<Parameters> action = this.eventListeners[i];

			action(parameters);
		}
	}

	public void NotifyObservers() {
		for(int i = 0; i < this.eventListenersNoParams.Count; i++) {
			System.Action action = this.eventListenersNoParams[i];

			action();
		}
	}

	public int GetListenerLength() {
		return (this.eventListeners.Count + this.eventListenersNoParams.Count);
	}
}

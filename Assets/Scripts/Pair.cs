using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pair<T1,T2> {
	//templated pair class because unity does not support natively pairs, only keyvaluepairs -_-

	public Pair() {

	}

	public Pair(T1 first, T2 second) {
		this.First = first;
		this.Second = second;
	}

	public T1 First { get; set; }
	public T2 Second { get; set; }
}


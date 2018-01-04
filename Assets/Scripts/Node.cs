using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    private GameObject nodeObj, model;
	
    public Node()
    {
        nodeObj = ((GameObject)Resources.Load("node"));
        model = null;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool isEmpty()
    {
        if (model == null)
            return true;
        else
            return false;
    }

    public void removeModel()
    {
        model = null;
    }

    public GameObject getNodeObj()
    {
        return nodeObj;
    }

    public void setNodeObj(GameObject node)
    {
        this.nodeObj = node;
    }

    public GameObject getModel()
    {
        return model;
    }

    public void setModel(GameObject model)
    {
        this.model = model;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardEventHandler : MonoBehaviour {

    public delegate void minionEventHandler(string minionName);

    public static event minionEventHandler onSummon;
    public static event minionEventHandler onDeath;

    public static void onMinionSummon(string minionName)
    {
        onSummon(minionName);
    }

    public static void onMinionDeath(string minionName)
    {
        onDeath(minionName);
    }

}

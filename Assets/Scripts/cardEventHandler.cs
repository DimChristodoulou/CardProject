using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardEventHandler : MonoBehaviour {

    public delegate void minionEventHandler(string minionName);

    public static event minionEventHandler onSummon;
    public static event minionEventHandler onDeath;

    /*
     * Function used to handle the on Summon effect
     */
    public static void onMinionSummon(string minionName)
    {
        onSummon(minionName);
    }

    /*
     * Function used to handle the on Death effect
     */
    public static void onMinionDeath(string minionName)
    {
        onDeath(minionName);
    }

}

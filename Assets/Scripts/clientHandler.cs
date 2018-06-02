//using ExitGames.Client.Photon.LoadBalancer;

//// It is best practice to extend the LoadBalancingClient to modify callbacks and react to updates coming from the server.
//public class MyClient : LoadBalancingClient
//{
//    // call this to connect to Photon
//    void CallConnect()
//    {
//        this.AppId = "<your appid>";  // set your app id here
//        this.AppVersion = "1.0";  // set your app version here

//        // "eu" is the European region's token
//        if (!this.ConnectToRegionMaster("eu")) // can return false for errors
//        {
//            this.DebugReturn(DebugLevel.ERROR, "Can't connect to: " + this.CurrentServerAddress);
//        }
//    }
//}
//    // [...]
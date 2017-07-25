using Launcher;
using UnityEngine;

namespace SLA
{
    public class DeathSLA : MonoBehaviour
    {
        /*
private PhotonView _photonView;

private void Awake()
{
    _photonView = GetComponent<PhotonView>();
}

//events following Deathtrigger
public void Death()
{
    GameControl.PlayerState.IsImmobile = true;
    GameControl.PlayerState.IsInvulnerable = true;
    _photonView.RPC("AdjustScore", PhotonTargets.MasterClient, PhotonNetwork.player.ID);
}

[PunRPC]
private void AdjustScore(int playerID)
{
    _photonView.RPC("SyncScore", PhotonTargets.Others, playerID,
        GameControl.PlayerState.SyncVars[playerID - 1].CurrentScore,
        GameControl.PlayerState.SyncVars[playerID - 1].TotalScore);
}

[PunRPC]
private void SyncScore(int playerID, int curScore, int totScore)
{
    GameControl.PlayerState.SyncVars[playerID - 1].CurrentScore = curScore;
    GameControl.PlayerState.SyncVars[playerID - 1].TotalScore = totScore;
}
*/
    }
}

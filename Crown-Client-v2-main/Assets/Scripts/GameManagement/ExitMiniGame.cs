using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ExitMiniGame : MonoBehaviour
{
    public int _scoreEarned = 10;
    // Start is called before the first frame update
    public void ExitClicked()
    {
        string _gameType = SceneLoader.returnCurrentSceneName();
        string _miniGameHash = Client.instance.miniGameHash;
        ClientSend.AddQueueCommand(new ExitMiniGameCommand(_gameType,_miniGameHash,_scoreEarned));
    }
}

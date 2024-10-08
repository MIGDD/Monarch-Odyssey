using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestListPanel : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject questButtonPrefab;
    public GameObject scrollRectContainer;

    public void PopulateQuestList()
    {
        List<QuestClass> _questList = GameManager.instance.player.currentQuests;
        //create a button for each quest class and add it to the scrollview content transform
        foreach (QuestClass _quest in _questList)
        {
            GameObject _questButton = Instantiate(questButtonPrefab, scrollRectContainer.transform);
            QuestListingButton _buttonScript = _questButton.GetComponent<QuestListingButton>();
            _buttonScript.Initialize(_quest);
        }
    }

    public void RemoveButtons()
    {
        foreach (Transform _button in scrollRectContainer.transform)
        {
            Destroy(_button.gameObject);
        }
    }
}

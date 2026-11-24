using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField]private Text text;
    [SerializeField]private Button button;
    [SerializeField]private GameObject SelectGO;

    private BaseAction baseAction;

    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;
        text.text = baseAction.GetActionName();
        button.onClick.AddListener(
            () => { UnitActionSystem.Instance.SetSelectedAction(baseAction); }
            );
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        SelectGO.SetActive(!(selectedAction == baseAction));
    }
}

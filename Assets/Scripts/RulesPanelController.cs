using UnityEngine;

public class RulesPanelController : MonoBehaviour
{
    public GameObject rulePanel;

    // OPEN RULES PANEL
    public void OpenRules()
    {
        rulePanel.SetActive(true);
    }

    // CLOSE RULES PANEL
    public void CloseRules()
    {
        rulePanel.SetActive(false);
    }
}

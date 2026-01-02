using UnityEngine;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour
{
    [SerializeField] Image healthFill;
    [SerializeField] Image auraFill;
    [SerializeField] Image softFill;
    [SerializeField] Image hardFill;

    public void SetHealth(float part)
    {
        healthFill.fillAmount = part;
    }

    public void SetAura(float part)
    {
        auraFill.fillAmount = part;
    }

    public void SetSoft(float part)
    {
        softFill.fillAmount = part;
    }

    public void SetHard(float part)
    {
        hardFill.fillAmount = part;
    }
}

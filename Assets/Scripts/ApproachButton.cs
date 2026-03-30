using UnityEngine;

public class ApproachButton : MonoBehaviour
{
    public PlanetApproachController planetController;

    public void OnApproachButtonClick()
    {
        if (planetController != null)
        {
            planetController.StartApproach();
        }
    }
}
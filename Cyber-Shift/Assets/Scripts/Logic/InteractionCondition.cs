using UnityEngine;

public class InteractionCondition : MonoBehaviour
{
    [Tooltip("Override this method for custom conditions")]
    public virtual bool IsMet()
    {
        return true;
    }
}
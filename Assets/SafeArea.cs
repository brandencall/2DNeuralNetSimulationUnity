using UnityEngine;

public class SafeArea : MonoBehaviour
{

    public float westBorder;
    public float eastBorder;
    public float northBorder;
    public float southBorder;
    void Start()
    {
        westBorder = this.transform.localPosition.x - (this.transform.localScale.x / 2);
        eastBorder = this.transform.localPosition.x + (this.transform.localScale.x / 2);
        northBorder = this.transform.localPosition.y + (this.transform.localScale.y / 2);
        southBorder = this.transform.localPosition.y - (this.transform.localScale.y / 2);
    }

    public bool IsEntityInSafeZone(Entity entity)
    {
        if (entity.transform.localPosition.x > westBorder
            && entity.transform.localPosition.x < eastBorder
            && entity.transform.localPosition.y < northBorder
            && entity.transform.localPosition.y > southBorder)
        {
            return true;
        }

        return false;
    }
}

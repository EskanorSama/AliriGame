using UnityEngine;

public class HiddenPlace : MonoBehaviour, IUsable
{
    private bool Hidden = false;
    public void Use()
    {
        Hide();
    }
    private void Hide()
    {
        Player.Instance.GetComponent<SpriteRenderer>().enabled = Hidden;
        Hidden = !Hidden;
        if (Hidden)
        {
            Player.Instance.Freeze();
            Player.Instance.Physick.gravityScale = 0f;
        }
        else
        {
            Player.Instance.UnFreeze();
            Player.Instance.Physick.gravityScale = 1f;
        }
        Player.Instance.GetComponent<Collider2D>().isTrigger = Hidden;
        Player.Instance.Hidden = Hidden;
    }
}

using UnityEngine;

public class ProtectionAmulet : Item
{
    private void Start() => Id = 3;
    public override void OnStartInventoryUse()
    {
        Player.Instance.GetComponent<OldHealth>().Amulet = true;
    }
    public override void OnPullOut()
    {
        Player.Instance.GetComponent<OldHealth>().Amulet = false;
    }
}
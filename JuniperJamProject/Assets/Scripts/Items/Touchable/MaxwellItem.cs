using System.Collections;
using UnityEngine;

public class MaxwellItem : TouchableItem
{
    [SerializeField] GameObject maxwellPrefab;
    [SerializeField] float maxwellDuration;
    GameObject maxwell;

    public override void Use()
    {
        maxwell = Instantiate(maxwellPrefab);
        player.GetMesh().gameObject.SetActive(false);
        player.TogglePlayerInput(false);

        StartCoroutine(MaxwellDeath());
    }

    IEnumerator MaxwellDeath()
    {
        yield return new WaitForSeconds(maxwellDuration);

        player.GetMesh().gameObject.SetActive(true);
        player.TogglePlayerInput(true);
    }
}

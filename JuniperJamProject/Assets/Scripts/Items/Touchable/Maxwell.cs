using System.Collections;
using UnityEngine;

public class Maxwell : TouchableItem
{
    [SerializeField] GameObject maxwell;
    Vector3 currentDir;

    public override void Use()
    {
        Instantiate(maxwell);
        player.GetMesh().gameObject.SetActive(false);
        player.TogglePlayerInput(false);

        StartCoroutine(MaxwellBehavior());
    }

    IEnumerator MaxwellBehavior()
    {
        yield return null;
    }

    private void ChooseNextDirection()
    {

    }
}

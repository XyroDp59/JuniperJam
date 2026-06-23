using System.Runtime.CompilerServices;
using UnityEngine;

public class Maxwell : MonoBehaviour
{
    [SerializeField] float attackPower;
    [SerializeField] float speed;
    [SerializeField] string tag;
    Vector3 currentDir;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(tag))
        {
            currentDir = ChooseNextDirection();
        } 
    }

    private Vector3 ChooseNextDirection()
    {
        Vector3 xAxis = transform.position.normalized;
        Vector3 yAxis = Vector3.Cross(xAxis, Vector3.up);

        float x = -1 * Vector3.Dot(currentDir, xAxis);
        float y = Vector3.Dot(currentDir, yAxis) * Random.Range(0.25f, 1.25f);

        return x*xAxis + y*yAxis;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += currentDir * speed;
    }


}

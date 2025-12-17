using UnityEngine;

public class KelpNode : MonoBehaviour
{

    [SerializeField] GameObject kelpLeafPrefab;

    Transform[] nodes;
    Rigidbody[] rigidBodies;

    Transform leafPivot;

    private void Start()
    {
        nodes = new Transform[3];
        rigidBodies = new Rigidbody[3];
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = Instantiate(kelpLeafPrefab, transform).transform;
            rigidBodies[i] = nodes[i].GetComponent<Rigidbody>();
            nodes[i].localPosition = (i + 1) * 1f * Vector3.right;
        }
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < nodes.Length; i++)
        {
            Vector3 deltaVector1;
            if (i == 0)
            {
                deltaVector1 = nodes[i].position - transform.position;
                nodes[i].LookAt(transform);
            }
            else
            {
                deltaVector1 = nodes[i].position - nodes[i - 1].position;
                nodes[i].LookAt(nodes[i - 1]);
            }
            nodes[i].GetChild(0).localScale = new Vector3(nodes[i].GetChild(0).localScale.x, nodes[i].GetChild(0).localScale.y, deltaVector1.magnitude);
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < nodes.Length; i++)
        {

            Vector3 deltaVector1;
            if (i == 0)
            {
                deltaVector1 = nodes[i].position - transform.position;
            }
            else
            {
                deltaVector1 = nodes[i].position - nodes[i - 1].position;
            }
            deltaVector1 = deltaVector1 - (deltaVector1.normalized / 4);
            deltaVector1 = Vector3.Scale(
                Vector3.Scale(deltaVector1, deltaVector1),
                new Vector3(Mathf.Sign(deltaVector1.x), Mathf.Sign(deltaVector1.y), Mathf.Sign(deltaVector1.z)));

            Vector3 deltaVector2 = Vector3.zero;
            if (i != nodes.Length - 1)
            {
                deltaVector2 = nodes[i + 1].position - nodes[i + 1].position;
                deltaVector2 = deltaVector2 - (deltaVector2.normalized / 4);
            }
            deltaVector2 = Vector3.Scale(
                Vector3.Scale(deltaVector2, deltaVector2),
                new Vector3(Mathf.Sign(deltaVector2.x), Mathf.Sign(deltaVector2.y), Mathf.Sign(deltaVector2.z)));

            rigidBodies[i].AddForce(-(deltaVector1 + deltaVector2) * 50 + Vector3.right * 10);

        }
    }
}


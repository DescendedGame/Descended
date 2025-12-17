using UnityEngine;

public class CarpetAlgae : Algae
{
    [SerializeField] GameObject algaePrefab;

    Transform[] algae;

    int targetLayer;

    public override void Initialize(Quaternion rotation)
    {
        base.Initialize(rotation);
        transform.position -= rotation * Vector3.forward * 2;
        transform.rotation = rotation * Quaternion.AngleAxis(-90, Vector3.right);
        targetLayer = LayerMask.GetMask("Shifting") | LayerMask.GetMask("Solid");
        algae = new Transform[5 * 6 + 1];

        algae[0] = SpawnAlgaeWithRaycast(transform.position, -transform.up);

        float angleCounter = 0;
        int lapCounter = 1;
        float randomRotation = 0;
        for(int i = 1; i < algae.Length; i++)
        {
            algae[i] = SpawnAlgaeWithRaycast(transform.position + Quaternion.AngleAxis(60*angleCounter + (Random.value-0.5f)*30 + randomRotation, transform.up)* transform.forward * lapCounter * 0.2f, -transform.up);

            if (!algae[i])
            {
                angleCounter++;
                if (angleCounter >= 6)
                {
                    angleCounter -= 6;
                    if (angleCounter > 0) angleCounter -= 0.5f;
                    else angleCounter += 0.5f;
                    lapCounter++;
                    randomRotation = (Random.value - 0.5f) * 30;

                }
                continue;
            }
            algae[i].localScale = Vector3.one * (1 - 0.1f*Random.value * lapCounter);
            angleCounter++;
            if(angleCounter >= 6)
            {
                angleCounter -= 6;
                if (angleCounter > 0) angleCounter -= 0.5f;
                else angleCounter += 0.5f;
                lapCounter++;
                randomRotation = (Random.value - 0.5f) * 30;

            }
        }

    }


    Transform SpawnAlgaeWithRaycast(Vector3 position, Vector3 downVector)
    {
        RaycastHit hit;
        if(Physics.Raycast(position, downVector, out hit, 3, targetLayer))
        {
            GameObject alg = Instantiate(algaePrefab, transform);
            alg.transform.position = hit.point + downVector *0.02f;
            alg.transform.rotation = Quaternion.LookRotation(transform.forward, -downVector);
            alg.transform.LookAt(alg.transform.position + hit.normal);
            return alg.transform;
        }
        return null;
    }

}

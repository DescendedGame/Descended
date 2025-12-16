using UnityEngine;

public class KelpCreator : MonoBehaviour
{
    [SerializeField] GameObject kelpPrefab;

    private void Awake()
    {
        for(int i = 0; i < 100; i++)
        {
            Instantiate(kelpPrefab);
            Vector2 randomPos = Random.insideUnitCircle;
            kelpPrefab.transform.position = new Vector3(randomPos.x, 0, randomPos.y)*20;
        }
    }
}

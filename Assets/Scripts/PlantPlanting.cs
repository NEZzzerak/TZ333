using UnityEngine;
using System.Collections;

public class PlantPlanting : MonoBehaviour
{
    [SerializeField] private float colorChangeDuration = 1f;
    [SerializeField] private float scaleChangeDuration = 1f;
    [SerializeField] private float minDistanceBetweenPlants = 2f;
    [SerializeField] private float collectDistance = 2f;
    [SerializeField] public GameObject plantPrefab;
    [SerializeField] public Material greenMaterial;
    [SerializeField] public GameObject player;
    [SerializeField] public LayerMask playerLayer;


    private GameObject plantedPlant;
    private Renderer plantRenderer;
    private bool plantIsGrown = false;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && plantedPlant == null)
        {
            Plant();
        }

        if (Input.GetKeyDown(KeyCode.R) && plantIsGrown && plantedPlant != null)
        {
            if (Physics.CheckSphere(plantedPlant.transform.position, collectDistance, playerLayer))
            {
                CollectPlant();
            }
        }
    }

    private void Plant()
    {
        Vector3 spawnPosition = transform.position + transform.forward * 2f;

        Collider[] colliders = Physics.OverlapSphere(spawnPosition, minDistanceBetweenPlants / 2f);
        bool collisionDetected = false;
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject && collider.gameObject.GetComponent<Plant>() != null) 
            {
                collisionDetected = true;
                break;
            }
        }

        if (!collisionDetected)
        {
            plantedPlant = Instantiate(plantPrefab, spawnPosition, Quaternion.identity);
            plantRenderer = plantedPlant.GetComponent<Renderer>();
            if (plantRenderer == null)
            {
                Destroy(plantedPlant);
                return;
            }
            plantRenderer.material.color = Color.yellow;
            StartCoroutine(GrowPlant(plantedPlant));
        }
        plantIsGrown = false;
    }

    private IEnumerator GrowPlant(GameObject plant)
    {
        yield return new WaitForSeconds(10f);

        StartCoroutine(ChangeScale(plant.transform, plant.transform.localScale, plant.transform.localScale * 2f, scaleChangeDuration));
        StartCoroutine(ChangeColor(plantRenderer, Color.yellow, greenMaterial.color, colorChangeDuration));

        plantIsGrown = true;
    }

    private IEnumerator ChangeScale(Transform transform, Vector3 startScale, Vector3 endScale, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = endScale;
    }

    private IEnumerator ChangeColor(Renderer renderer, Color startColor, Color endColor, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            renderer.material.color = Color.Lerp(startColor, endColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        renderer.material.color = endColor;
    }

    private void CollectPlant()
    {
        Destroy(plantedPlant);
        plantIsGrown = false;
    }
}
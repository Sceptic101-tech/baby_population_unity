using UnityEngine;

public class Vision : MonoBehaviour
{
    private bool is_detected = false;
    public void GetVisionData(float[][] layers, int count_of_rays, float field_of_view, float vision_range, bool is_predator)
    {
        if (layers == null || layers[0] == null)
        {
            Debug.LogError("Vision layers are not initialized!");
            return;
        }

        float angle_of_rotation = field_of_view / count_of_rays;
        for (int i = 0; i < count_of_rays; i++)
        {
            Vector3 dir = Quaternion.Euler(0, (-field_of_view / 2) + (i * angle_of_rotation), 0) * transform.forward;

            if (Physics.Raycast(transform.position, dir * vision_range, out RaycastHit hit, vision_range))
            {
                float distance = hit.distance;
                if (float.IsNaN(distance)) distance = 0f;

                //3 groups of input neurons for different object classes
                if (((hit.collider.tag == "prey") && is_predator) || ((hit.collider.tag == "predator") && !is_predator))
                {
                    is_detected = true;
                    Debug.DrawRay(transform.position, dir * vision_range, Color.red);
                    layers[0][i] = Mathf.Exp(-(distance * distance) / 30) + 1.0f;
                    layers[0][count_of_rays + i] = 0;
                    layers[0][count_of_rays + count_of_rays + i] = 0;
                }
                else if (((hit.collider.tag == "prey") && !is_predator) || ((hit.collider.tag == "predator") && is_predator))
                {
                    is_detected = true;
                    Debug.DrawRay(transform.position, dir * vision_range, Color.green);
                    layers[0][i] = 0;
                    layers[0][count_of_rays + i] = Mathf.Exp(-(distance * distance) / 30) + 1.0f;
                    layers[0][count_of_rays + count_of_rays + i] = 0;
                }
                else if (hit.collider.tag == "wall")
                {
                    is_detected = true;
                    Debug.DrawRay(transform.position, dir * vision_range, Color.blue);
                    layers[0][i] = 0;
                    layers[0][count_of_rays + i] = 0;
                    layers[0][count_of_rays + count_of_rays + i] = Mathf.Exp(-(distance * distance) / 30) + 1.0f;
                }
                else
                {
                    layers[0][i] = 0;
                    layers[0][count_of_rays + i] = 0;
                    layers[0][count_of_rays + count_of_rays + i] = 0;
                }
            }
        }
        //creating random idle input for agents
        if (!is_detected)
        {
            for (int i = 0; i < count_of_rays; i++)
            {
                layers[0][i] = Random.Range(0.0f, 1f);
                layers[0][count_of_rays + i] = Random.Range(0.0f, 1f);
                layers[0][count_of_rays + count_of_rays + i] = Random.Range(0.0f, 1f);           
            }
        }
    }
}

using System;
using UnityEngine;

[System.Serializable]
public struct VeinSettings
{
    public Vector3 start_position;
    public Vector3 end_position;
    public Quaternion start_rotation;
    public Quaternion end_rotation;
    public int corner_count;
    public float start_size;
    public float end_size;
    public float size_step;
    public float vein_length;
    public bool is_tunnel;
}

//Clean this up!!!
//Less cluttered, separate functions, choose if hollow or not upon creation, clear parameters for corners and segments and radius and stuff...
[ExecuteInEditMode]
public class Vein : MonoBehaviour
{
    [SerializeField] VeinEnd m_vein_ends;

    public VeinSettings settings;

    Vector3 m_end_position;
    Quaternion m_end_rotation;
    int m_corner_count;
    float m_start_size;
    float m_end_size;
    float m_size_step;
    float m_vein_length;
    bool m_is_tunnel;

    Vector3 m_intersection_point;

    float m_degree_interval;
    Vector3 m_rotation_plane;
    [Range(-1, 1)]
    public float m_rotation;
    int m_joint_count;
    int m_vertex_count;

    Vector3[] m_joint_middles;

    Quaternion[] m_joint_rotations;

    Transform m_vein_start = null;
    Transform m_vein_end = null;

    public void Generate(Vector3 target_position, float start_radius, float end_radius, float rotation, bool is_tunnel, EndType start_type, EndType end_type)
    {
        if (GetComponent<MeshFilter>().sharedMesh == null) GetComponent<MeshFilter>().mesh = new Mesh();
        UpdateVeinMesh(target_position, 12, start_radius, end_radius, rotation, is_tunnel);
        if (m_vein_end == null)
        {

        }
        else
        {
            m_vein_end.localPosition = m_end_position;
            m_vein_end.localRotation = m_end_rotation;
            m_vein_end.localScale = m_end_size * Vector3.one;
            if (m_joint_count % 2 == 0) m_vein_end.rotation *= Quaternion.AngleAxis(360 / 24, Vector3.forward);
        }
        if (m_vein_start == null)
        {

        }
        else
        {
            m_vein_start.localPosition = Vector3.zero;
            m_vein_start.localRotation = Quaternion.LookRotation(-Vector3.forward, Vector3.up);
            m_vein_start.localScale = m_end_size * Vector3.one;
        }
    }

    public void UpdateVeinMesh(Vector3 end_position, int corner_count, float start_size, float end_size, float rotation, bool is_tunnel)
    {
        m_corner_count = corner_count;
        m_start_size = start_size;
        m_end_size = end_size;
        m_is_tunnel = is_tunnel;
        m_rotation = rotation;

        if (end_position.magnitude == 0) return;

        PrepareMathematics(transform.InverseTransformPoint(end_position), 12);

        Vector3[] vertices;
        Vector3[] normals;

        CreateOuterVerticesAndNormals(out vertices, out normals);
        int[] triangles = CreateOuterTriangles(vertices.Length);

        if (m_is_tunnel)
        {
            AddInnerVerticesAndNormals(vertices, normals);
            AddInnerTriangles(triangles);
        }

        CorrectNormalsForScale(normals, vertices);

        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = triangles;

        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    void CorrectNormalsForScale(Vector3[] normals, Vector3[] vertices)
    {
        for (int i = 0; i < m_joint_count; i++)
        {
            //Calculate inner normals, if there are such...
            for (int j = 0; j < 12; j++)
            {
                int index = i * 12 + j + normals.Length / 2;
                Vector3 prev_point;
                if (i == 0)
                {
                    prev_point = vertices[index];
                }
                else
                {
                    prev_point = m_joint_middles[i - 1];
                    prev_point += Quaternion.AngleAxis(-m_degree_interval, m_rotation_plane) * (normals[i * 12 + j] - normals[i * 12 + j].normalized * m_size_step) * 0.8f;
                }

                Vector3 next_point;
                if (i == m_joint_count - 1)
                {
                    next_point = vertices[index];
                }
                else
                {
                    next_point = m_joint_middles[i + 1];
                    next_point += Quaternion.AngleAxis(m_degree_interval, m_rotation_plane) * (normals[i * 12 + j] + normals[i * 12 + j].normalized * m_size_step) * 0.8f;
                }

                normals[index] = (Quaternion.AngleAxis(90, Vector3.Cross(next_point - prev_point, normals[index])) * (next_point - prev_point)).normalized;

                Debug.DrawLine(vertices[index], vertices[index] + normals[index]);
            }

            for (int j = 0; j < 12; j++)
            {
                Vector3 prev_point;
                if (i == 0)
                {
                    prev_point = vertices[i * 12 + j];
                }
                else
                {
                    prev_point = m_joint_middles[i - 1];
                    prev_point += Quaternion.AngleAxis(-m_degree_interval, m_rotation_plane) * (normals[i * 12 + j] - normals[i * 12 + j].normalized * m_size_step);
                }
                Vector3 next_point;
                if (i == m_joint_count - 1)
                {
                    next_point = vertices[i * 12 + j];
                }
                else
                {
                    next_point = m_joint_middles[i + 1];
                    next_point += Quaternion.AngleAxis(m_degree_interval, m_rotation_plane) * (normals[i * 12 + j] + normals[i * 12 + j].normalized * m_size_step);
                }

                normals[i * 12 + j] = (Quaternion.AngleAxis(90, Vector3.Cross(next_point - prev_point, normals[i * 12 + j])) * (next_point - prev_point)).normalized;

                Debug.DrawLine(vertices[i * 12 + j], vertices[i * 12 + j] + normals[i * 12 + j]);
            }


        }
    }

    /// <summary>
    /// Sets up a 2D target (in the forward-normal plane), finds the intersection point on that plance, 
    /// (where the perpendicular thing from the middle point goes... Viktor smarts)
    /// calculates segment count and how far apart they should be.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="max_segments"></param>
    void PrepareMathematics(Vector3 target, int max_segments)
    {
        if (Vector3.Angle(transform.forward, target) == 0 || Vector3.Angle(transform.forward, target) == 180)
        {
            m_joint_count = 2;
            m_joint_rotations = new Quaternion[m_joint_count];
            m_joint_middles = new Vector3[m_joint_count];
            m_joint_middles[0] = Vector3.zero;
            m_joint_middles[1] = target;
        }
        else
        {
            Vector2 m_target_to_plane = target;
            //calculate line equation
            float x = m_target_to_plane.magnitude;
            float k = target.z / x;
            k = -(1 / k); //perpendicular line

            Vector3 mid_point = target / 2;

            //mid_point.z + k * x2 = 0;
            float distance_to_intersection = -mid_point.z / k;
            m_intersection_point = (m_target_to_plane / 2) + m_target_to_plane.normalized * distance_to_intersection;




            // 0, intersection point, and target spans a isosceles triangle. This substract the two equal angles to calculate the full angle.
            float degrees = (Mathf.PI - Mathf.Asin(target.z / target.magnitude) * 2) * Mathf.Rad2Deg;
            // same as above:
            //float degrees = Vector3.Angle(-m_intersection_point, target - m_intersection_point);

            max_segments -= 1; //Say you want 6... but 180 degrees results in 7 if 6 is used!!!
            int joint_interval = 360 / max_segments;

            //add two because that is the least amount of joints (first and last). Segments are joints-1.
            m_joint_count = (int)((degrees - (degrees % joint_interval)) / joint_interval) + 2;

            //Validate stuff - ONLY ACCURATE TO 180deg
            float circle_radius = m_intersection_point.magnitude;
            float bigger_size = Mathf.Max(m_start_size /*- (m_end_size - m_start_size) / m_joint_count*/, m_end_size /*- (m_end_size - m_start_size) / m_joint_count*/);
            if (circle_radius < bigger_size)
            {
                m_intersection_point = m_intersection_point.normalized * bigger_size;
            }

            //the joints are placed at the beginning of segments. I want the last one to be placed at the end of the segment.
            //therefore, I remove 1 from the segment_count divider to that the radian_interval is increased enough to add
            //up to that.
            m_degree_interval = degrees / (m_joint_count - 1);

            m_joint_rotations = new Quaternion[m_joint_count];
            m_joint_middles = new Vector3[m_joint_count];

            m_rotation_plane = Vector3.Cross(Vector3.forward, m_intersection_point);

            float incrementing_angle = 0;
            {
                Vector3 offset_3D = Quaternion.AngleAxis(-m_degree_interval, m_rotation_plane) * (-m_intersection_point);
            }
            for (int i = 0; i < m_joint_count; i++)
            {

                Vector3 offset_3D = Quaternion.AngleAxis(incrementing_angle, m_rotation_plane) * (-m_intersection_point);

                m_joint_rotations[i] = Quaternion.AngleAxis(incrementing_angle, m_rotation_plane);
                m_joint_middles[i] = m_intersection_point + offset_3D;

                incrementing_angle += m_degree_interval;
                if (i == m_joint_count - 1)
                {
                    offset_3D = Quaternion.AngleAxis(incrementing_angle, m_rotation_plane) * (-m_intersection_point);
                }
            }
        }

        

        if (m_is_tunnel)
        {
            m_vertex_count = m_corner_count * m_joint_count * 2;
        }
        else
        {
            m_vertex_count = m_corner_count * m_joint_count;
        }

        m_vein_length = m_joint_middles[1].magnitude * (m_joint_count - 1);
        float delta_size = Mathf.Abs(m_end_size - m_start_size);

        if (delta_size / m_vein_length > 0.2f)
        {
            m_end_size = m_start_size + m_vein_length * Mathf.Sign(m_end_size-m_start_size) * 0.2f;
        }
        else
        {

        }

        m_size_step = (m_end_size - m_start_size) / m_joint_count;

        m_end_position = m_joint_middles[m_joint_count - 1];


    }


    /// <summary>
    /// Does not calculate finished normals. They are later adjusted for scale difference between start and end,
    /// but good to keep in their current version if a tunnel is wanted.
    /// This function also calculates end rotation. Maybe unnecessary.
    /// </summary>
    /// <param name="vertices"></param>
    /// <param name="normals"></param>
    /// <param name="corners"> 6 gives a hexagon vein, for example. </param>
    void CreateOuterVerticesAndNormals(out Vector3[] vertices, out Vector3[] normals)
    {
        vertices = new Vector3[m_vertex_count];
        normals = new Vector3[m_vertex_count];

        float polygon_angle = 360 / m_corner_count;

        float size_interval = (m_end_size - m_start_size) / (m_joint_count - 1);

        for (int i = 0; i < m_joint_count; i++)
        {
            Vector3 rotated_up_vector = m_joint_rotations[i] * Vector3.up;
            Vector3 m_joint_direction = m_joint_rotations[i] * Vector3.forward;

            float rotation = i * polygon_angle / 2;
            rotation = rotation + rotation * m_rotation;
            for (int j = 0; j < m_corner_count; j++)
            {
                int index = i * m_corner_count + j;

                normals[index] = Quaternion.AngleAxis(j * polygon_angle + rotation, m_joint_direction) * rotated_up_vector * (m_start_size + i * size_interval);
                vertices[index] = m_joint_middles[i] + normals[index];
            }

            if (i == m_joint_count - 1)
            {
                m_end_rotation = m_joint_rotations[i] * Quaternion.AngleAxis(rotation, Vector3.forward);
                return;
            }
        }
    }

    /// <summary>
    /// Returns the triangles for the outer shell.
    /// </summary>
    /// <param name="vertex_count"></param>
    /// <param name="corners"></param>
    /// <returns></returns>
    int[] CreateOuterTriangles(int vertex_count)
    {

        // every corner will be responsible of 2 triangles, with 3 corners each...
        // verts count * 6 - corners * 6 =

        int triangle_count;

        if (m_is_tunnel)
        {
            triangle_count = (vertex_count - m_corner_count) * 12;
        }
        else
        {
            triangle_count = (vertex_count - m_corner_count) * 6;
        }

        int[] triangles = new int[triangle_count];


        for (int i = 0; i < m_joint_count - 1; i++)
        {

            //This stuff is awesome. Requires some additional, complicated work for the edges though. Why is it awesome though, Viktor!?
            #region
            //if (i == m_joint_count - 2)
            //{
            //    for (int j = 0; j < m_corner_count; j++)
            //    {
            //        int vertex_index = i * m_corner_count + j;
            //        int triangle_index = vertex_index * 6;

            //        if (j == m_corner_count - 1)
            //        {
            //            triangles[triangle_index] = j;
            //            triangles[triangle_index + 1] = j - (m_corner_count - 1);
            //            triangles[triangle_index + 2] = j + m_corner_count;

            //            triangles[triangle_index + 3] = vertex_index - (m_corner_count - 1);
            //            triangles[triangle_index + 4] = vertex_index + 1;
            //            triangles[triangle_index + 5] = vertex_index + m_corner_count;
            //        }
            //        else
            //        {
            //            triangles[triangle_index] = j;
            //            triangles[triangle_index + 1] = j + 1;
            //            triangles[triangle_index + 2] = j + m_corner_count;

            //            triangles[triangle_index + 3] = vertex_index + 1;
            //            triangles[triangle_index + 4] = vertex_index + m_corner_count + 1;
            //            triangles[triangle_index + 5] = vertex_index + m_corner_count;
            //        }
            //    }
            //}
            //else
            //{


            //    for (int j = 0; j < m_corner_count; j++)
            //    {
            //        int vertex_index = i * m_corner_count + j;
            //        int triangle_index = vertex_index * 6;



            //        if (j == 0)
            //        {

            //            triangles[triangle_index] = vertex_index;
            //            triangles[triangle_index + 1] = vertex_index + m_corner_count;
            //            triangles[triangle_index + 2] = vertex_index + m_corner_count + m_corner_count + m_corner_count - 1;

            //            triangles[triangle_index + 3] = vertex_index;
            //            triangles[triangle_index + 4] = vertex_index + m_corner_count + m_corner_count + m_corner_count - 1;
            //            triangles[triangle_index + 5] = vertex_index + m_corner_count + m_corner_count - 1;

            //            //triangles[triangle_index] = vertex_index;
            //            //triangles[triangle_index + 1] = vertex_index - (m_corner_count - 1);
            //            //triangles[triangle_index + 2] = vertex_index + m_corner_count;

            //            //triangles[triangle_index + 3] = vertex_index - (m_corner_count - 1);
            //            //triangles[triangle_index + 4] = vertex_index + 1;
            //            //triangles[triangle_index + 5] = vertex_index + m_corner_count;
            //        }
            //        else
            //        {
            //            triangles[triangle_index] = vertex_index;
            //            triangles[triangle_index + 1] = vertex_index + m_corner_count;
            //            triangles[triangle_index + 2] = vertex_index + m_corner_count + m_corner_count - 1;

            //            triangles[triangle_index + 3] = vertex_index;
            //            triangles[triangle_index + 4] = vertex_index + m_corner_count + m_corner_count - 1;
            //            triangles[triangle_index + 5] = vertex_index + m_corner_count - 1;
            //        }
            //    }
            //}
            #endregion

            for (int j = 0; j < m_corner_count; j++)
            {
                int vertex_index = i * m_corner_count + j;
                int triangle_index = vertex_index * 6;

                if (j == m_corner_count - 1)
                {
                    triangles[triangle_index] = vertex_index;
                    triangles[triangle_index + 1] = vertex_index - (m_corner_count - 1);
                    triangles[triangle_index + 2] = vertex_index + m_corner_count;

                    triangles[triangle_index + 3] = vertex_index - (m_corner_count - 1);
                    triangles[triangle_index + 4] = vertex_index + 1;
                    triangles[triangle_index + 5] = vertex_index + m_corner_count;
                }
                else
                {
                    triangles[triangle_index] = vertex_index;
                    triangles[triangle_index + 1] = vertex_index + 1;
                    triangles[triangle_index + 2] = vertex_index + m_corner_count;

                    triangles[triangle_index + 3] = vertex_index + 1;
                    triangles[triangle_index + 4] = vertex_index + m_corner_count + 1;
                    triangles[triangle_index + 5] = vertex_index + m_corner_count;
                }
            }
        }

        return triangles;
    }

    void AddInnerVerticesAndNormals(Vector3[] vertices, Vector3[] normals)
    {
        for (int i = vertices.Length / 2; i < vertices.Length; i++)
        {
            normals[i] = normals[i - vertices.Length / 2] * (-1);
            vertices[i] = vertices[i - vertices.Length / 2] + normals[i] * 0.2f;
        }
    }

    void AddInnerTriangles(int[] triangles)
    {
        int vertex_count = m_vertex_count / 2;
        for (int i = 0; i < m_joint_count - 1; i++)
        {
            for (int j = 0; j < m_corner_count; j++)
            {
                int vertex_index = i * m_corner_count + j + vertex_count;
                int triangle_index = vertex_index * 6;

                if (j == m_corner_count - 1)
                {
                    triangles[triangle_index] = vertex_index;
                    triangles[triangle_index + 1] = vertex_index + m_corner_count;
                    triangles[triangle_index + 2] = vertex_index - m_corner_count + 1;

                    triangles[triangle_index + 3] = vertex_index - m_corner_count + 1;
                    triangles[triangle_index + 4] = vertex_index + m_corner_count;
                    triangles[triangle_index + 5] = vertex_index + 1;
                }
                else
                {
                    triangles[triangle_index] = vertex_index;
                    triangles[triangle_index + 1] = vertex_index + m_corner_count;
                    triangles[triangle_index + 2] = vertex_index + 1;

                    triangles[triangle_index + 3] = vertex_index + 1;
                    triangles[triangle_index + 4] = vertex_index + m_corner_count;
                    triangles[triangle_index + 5] = vertex_index + m_corner_count + 1;
                }
            }
        }
    }

    public enum EndType
    {
        Sphere,
        None,
    }

    public Transform CreateEnd(Vector3 position, Quaternion rotation, float radius, EndType type, bool is_tunnel)
    {
        Transform t_end = GameObject.Instantiate(m_vein_ends.GetEndPrefab(type, is_tunnel), transform).transform;
        t_end.localPosition = position;
        t_end.localRotation = rotation;
        t_end.localScale = Vector3.one * radius;

        switch (type)
        {
            case EndType.Sphere:
                Mesh t_mesh = t_end.GetComponent<MeshFilter>().sharedMesh;
                Vector3[] t_normals = new Vector3[t_mesh.vertices.Length];
                for (int i = 0; i < t_normals.Length; i++)
                {
                    t_normals[i] = t_mesh.vertices[i].normalized;
                }
                t_mesh.SetNormals(t_normals);
                break;
            case EndType.None:
                break;
            default:
                break;
        }

        return t_end;
    }
}

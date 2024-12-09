using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using static UnityEngine.UI.GridLayoutGroup;

//Clean this up!!!
//Less cluttered, separate functions, choose if hollow or not upon creation, clear parameters for corners and segments and radius and stuff...

[ExecuteInEditMode]
public class Vein : MonoBehaviour
{
    Vector3 m_end_position;
    Quaternion m_end_rotation;
    int m_corner_count;
    float m_start_size;
    float m_end_size;
    float m_vein_length;
    public float m_rotation;
    bool m_is_tunnel;

    Vector3 m_intersection_point;
    Vector2 m_direction_2D;

    public float m_radian_interval;
    public int m_joint_count;
    int m_vertex_count;

    Vector3[] m_joint_middles;

    public Transform m_target;

    private void Update()
    {
        UpdateVeinMesh(m_target.position, 12, 5, 5f, m_rotation, true);
    }

    public void UpdateVeinMesh(Vector3 end_position, int corner_count, float start_size, float end_size, float rotation, bool is_tunnel)
    {
        m_end_position = transform.InverseTransformPoint(end_position);
        m_corner_count = corner_count;
        m_start_size = start_size;
        m_end_size = end_size;
        m_is_tunnel = is_tunnel;
        m_rotation = rotation;

        if (m_end_position.magnitude == 0) return;

        PrepareMathematics(m_end_position, 12);

        Vector3[] vertices;
        Vector3[] normals;

        CreateOuterVerticesAndNormals(out vertices, out normals);
        int[] triangles = CreateOuterTriangles(vertices.Length);

        if (m_is_tunnel)
        {
            AddInnerVerticesAndNormals(vertices, normals);
            AddInnerTriangles(triangles);
        }

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = triangles;

        if(m_start_size != m_end_size)
        {
            mesh.RecalculateNormals();
        }

    }


    /// <summary>
    /// Sets up the 2D direction (in the forward-normal plane), finds the intersection point on that plance, 
    /// (where the perpendicular thing from the middle point goes... Viktor smarts)
    /// calculates segment count and how far apart they should be.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="max_segments"></param>
    void PrepareMathematics(Vector3 target, int max_segments)
    {
        m_direction_2D = target;
        //calculate line equation
        float x = m_direction_2D.magnitude;
        float k = target.z / x;
        k = -(1 / k); //perpendicular line

        Vector3 mid_point = target / 2;

        //mid_point.z + k * x2 = 0;
        float distance_to_intersection = -mid_point.z / k;
        m_intersection_point = (m_direction_2D / 2) + m_direction_2D.normalized * distance_to_intersection;

        //Validate stuff
        float circle_radius = m_intersection_point.magnitude;
        float bigger_size = Mathf.Max(m_start_size, m_end_size);
        if(circle_radius < bigger_size)
        {
            m_intersection_point = m_intersection_point.normalized * bigger_size;
            circle_radius = bigger_size;
        }


        float radians = Mathf.PI - Mathf.Asin(target.z / target.magnitude) * 2;

        float delta_size = Mathf.Abs(m_end_size - m_start_size);

        if(delta_size / (circle_radius * radians) > 0.2f)
        {
            //1 = (m_end_size -m_start_size) / (circle_radius * radians)
            m_end_size = circle_radius * radians * 0.2f + m_start_size;
        }


        float degrees = radians * Mathf.Rad2Deg;

        max_segments -= 1; //Say you want 6... but 180 degrees results in 7 if 6 is used!!!
        int segment_intervar = 360 / max_segments;

        //add two because that is the least amount of segments (first and last).
        m_joint_count = (int)((degrees - (degrees % segment_intervar)) / segment_intervar) + 2;

        //the nodes are placed at the beginning of segments. I want the last one to be placed at the end of the segment.
        //therefore, I remove 1 from the segment_count divider to that the radian_interval is increased enough to add
        //up to that.
        m_radian_interval = radians / (m_joint_count - 1);

        if (m_is_tunnel)
        {
            m_vertex_count = m_corner_count * m_joint_count * 2;
        }
        else
        {
            m_vertex_count = m_corner_count * m_joint_count;
        }

        m_joint_middles = new Vector3[m_joint_count];

        float incrementing_radians = -Mathf.PI / 2;
        for (int i = 0; i < m_joint_count; i++)
        {
            Vector2 offset_2D = m_intersection_point.magnitude * new Vector2(Mathf.Sin(incrementing_radians), Mathf.Cos(incrementing_radians));
            Vector3 offset_3D = (Vector3)(offset_2D.x * m_direction_2D.normalized) + Vector3.forward * offset_2D.y;
            m_joint_middles[i] = m_intersection_point + offset_3D;
            incrementing_radians += m_radian_interval;
        }

        m_vein_length = m_joint_middles[1].magnitude * m_joint_count - 2;



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

        Vector3 rotate_vector = Vector3.Cross(m_direction_2D, Vector3.forward);

        float degree_interval = -m_radian_interval * Mathf.Rad2Deg;

        float polygon_angle = 360 / m_corner_count;

        float size_interval = (m_end_size-m_start_size) /(m_joint_count-1);

        for (int i = 0; i < m_joint_count; i++)
        {
            Quaternion total_rotation = Quaternion.AngleAxis(i * degree_interval, rotate_vector);
            Vector3 rotated_up_vector = total_rotation * Vector3.up;
            Vector3 rotated_forward_vector = total_rotation * Vector3.forward;

            for (int j = 0; j < m_corner_count; j++)
            {
                int index = i * m_corner_count + j;

                float rotation = i * polygon_angle / 2;
                rotation = rotation + rotation * m_rotation;

                normals[index] = Quaternion.AngleAxis(j * polygon_angle + rotation, rotated_forward_vector) * rotated_up_vector * (m_start_size + i * size_interval);
                vertices[index] = m_joint_middles[i] + normals[index] ;
            }

            if (i == m_joint_count - 1)
            {
                m_end_rotation = Quaternion.LookRotation(rotated_forward_vector, rotated_up_vector);
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

            //This stuff is awesome. Requires some additional, complicated work for the edges though
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
}

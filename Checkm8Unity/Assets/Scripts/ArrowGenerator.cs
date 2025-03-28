using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ArrowGenerator : MonoBehaviour
{
    private float _stemLength;
    private float _stemWidth;
    private float _tipLength;
    private float _tipWidth;

    //[System.NonSerialized]
    private List<Vector3> _verticesList;
    //[System.NonSerialized]
    private List<int> _trianglesList;

    private Mesh _mesh;

    void Start()
    {
        //make sure Mesh Renderer has a material
        _mesh = new Mesh();
        this.GetComponent<MeshFilter>().mesh = _mesh;
    }

    void Update()
    {
        GenerateArrow();
    }

    //arrow is generated starting at Vector3.zero
    //arrow is generated facing right, towards radian 0.
    void GenerateArrow()
    {
        //setup
        _verticesList = new List<Vector3>();
        _trianglesList = new List<int>();

        //stem setup
        Vector3 stemOrigin = Vector3.zero;
        float stemHalfWidth = _stemWidth / 2f;
        //Stem points
        _verticesList.Add(stemOrigin + (stemHalfWidth * Vector3.down));
        _verticesList.Add(stemOrigin + (stemHalfWidth * Vector3.up));
        _verticesList.Add(_verticesList[0] + (_stemLength * Vector3.right));
        _verticesList.Add(_verticesList[1] + (_stemLength * Vector3.right));

        //Stem triangles
        _trianglesList.Add(0);
        _trianglesList.Add(1);
        _trianglesList.Add(3);

        _trianglesList.Add(0);
        _trianglesList.Add(3);
        _trianglesList.Add(2);

        //tip setup
        Vector3 tipOrigin = _stemLength * Vector3.right;
        float tipHalfWidth = _tipWidth / 2;

        //tip points
        _verticesList.Add(tipOrigin + (tipHalfWidth * Vector3.up));
        _verticesList.Add(tipOrigin + (tipHalfWidth * Vector3.down));
        _verticesList.Add(tipOrigin + (_tipLength * Vector3.right));

        //tip triangle
        _trianglesList.Add(4);
        _trianglesList.Add(6);
        _trianglesList.Add(5);

        //assign lists to mesh.
        _mesh.vertices = _verticesList.ToArray();
        _mesh.triangles = _trianglesList.ToArray();
    }
}

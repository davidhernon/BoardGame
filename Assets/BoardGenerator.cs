using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardGenerator : MonoBehaviour {

	public List<Vector3> newVertices = new List<Vector3> ();
	public List<int> newTriangles = new List<int> ();
	public List<Vector2> newUV = new List<Vector2>();
	private Mesh mesh;
	private int num_rows, num_cols, ult_col;
	private int scale;
	private int triangle_count;
	private int x_shift, y_shift, this_x_shift, this_y_shift, row_index;
	private float tUnit;
	public double[,] map;

	// Use this for initialization
	void Start () {

		tUnit = 0.25f;
		Vector2 texture = new Vector2 (0,0);
	
		num_rows = 10;
		ult_col = num_rows;
		scale = 1;
		map = new double[(4*num_rows) + 10,(3*ult_col)+10];

		mesh = GetComponent<MeshFilter> ().mesh;
		triangle_count = 0;
		float z = transform.position.z;
		x_shift = 4;
		y_shift = 0;
		row_index = 0;
		num_cols = (num_rows / 2);

		GenerateTerrain (num_rows+10, ult_col+10, scale);
		CreateMesh (z, texture);
		BuildMesh ();
		}
	
	// Update is called once per frame
	void Update () {
	
	}

	void GenerateTerrain(int rows, int cols, int scale){

		for(int i=0; i<rows; i++){
			for(int j=0; j<cols; j++){
				map[i,j] = (Random.value * (1.0*scale));
			}
		}

	}

	void BuildHexEven(int row, int col, float z, Vector2 texture){

		//original flat working 
		/*newVertices.Add (new Vector3 (((2 * row) + 2 + this_x_shift) * scale, (-(3 * 2 * col) + this_y_shift) * scale, z)); //0
		newVertices.Add (new Vector3 (((2 * row) + 4 + this_x_shift) * scale, ((-(3 * 2 * col)) - 1 + this_y_shift) * scale, z)); //1
		newVertices.Add (new Vector3 (((2 * row) + 4 + this_x_shift) * scale, ((-(3 * 2 * col)) - 3 + this_y_shift) * scale, z)); //2
		newVertices.Add (new Vector3 (((2 * row) + 2 + this_x_shift) * scale, ((-(3 * 2 * col)) - 4 + this_y_shift) * scale, z)); //3
		newVertices.Add (new Vector3 (((2 * row) + this_x_shift) * scale, ((-(3 * 2 * col)) - 3 + this_y_shift) * scale, z)); //4
		newVertices.Add (new Vector3 (((2 * row) + this_x_shift) * scale, ((-(3 * 2 * col)) - 1 + this_y_shift) * scale, z)); //5
		*/

		/*//added random noise for height/*
		newVertices.Add (new Vector3 (((2 * row) + 2 + this_x_shift) * scale, (-(3 * 2 * col) + this_y_shift) * scale, (float)map[((2 * row) + 2 + this_x_shift) * scale,(3 * col) - this_y_shift * scale] )); //0
		newVertices.Add (new Vector3 (((2 * row) + 4 + this_x_shift) * scale, ((-(3 * 2 * col)) - 1 + this_y_shift) * scale, (float)map[((2 * row) + 4 + this_x_shift) * scale, (((3 * col)) + 1 - this_y_shift) * scale])); //1
		newVertices.Add (new Vector3 (((2 * row) + 4 + this_x_shift) * scale, ((-(3 * 2 * col)) - 3 + this_y_shift) * scale, (float)map[((2 * row) + 4 + this_x_shift) * scale, (((3 * col)) + 3 - this_y_shift) * scale])); //2
		newVertices.Add (new Vector3 (((2 * row) + 2 + this_x_shift) * scale, ((-(3 * 2 * col)) - 4 + this_y_shift) * scale, (float)map[((2 * row) + 2 + this_x_shift) * scale, (((3 * col)) + 4 - this_y_shift) * scale])); //3
		newVertices.Add (new Vector3 (((2 * row) + this_x_shift) * scale, ((-(3 * 2 * col)) - 3 + this_y_shift) * scale, (float)map[((2 * row) + this_x_shift) * scale, (((3 * col)) + 3 - this_y_shift) * scale])); //4
		newVertices.Add (new Vector3 (((2 * row) + this_x_shift) * scale, ((-(3 * 2 * col)) - 1 + this_y_shift) * scale, (float)map[((2 * row) + this_x_shift) * scale, (((3 * col)) + 1 - this_y_shift) * scale])); //5*/

		newVertices.Add (new Vector3 (((2 * row) + 2 + this_x_shift) * scale, (-(3 * 2 * col) + this_y_shift) * scale, (float)map[((2 * row) + 2 + this_x_shift) * scale,(3 * col) - this_y_shift * scale] )); //0
		newVertices.Add (new Vector3 (((2 * row) + 4 + this_x_shift) * scale, ((-(3 * 2 * col)) - 1 + this_y_shift) * scale, (float)map[((2 * row) + 4 + this_x_shift) * scale, (((3 *2 * col)) + 1 - this_y_shift) * scale])); //1
		newVertices.Add (new Vector3 (((2 * row) + 4 + this_x_shift) * scale, ((-(3 * 2 * col)) - 3 + this_y_shift) * scale, (float)map[((2 * row) + 4 + this_x_shift) * scale, (((3 *2* col)) + 3 - this_y_shift) * scale])); //2
		newVertices.Add (new Vector3 (((2 * row) + 2 + this_x_shift) * scale, ((-(3 * 2 * col)) - 4 + this_y_shift) * scale, (float)map[((2 * row) + 2 + this_x_shift) * scale, (((3 *2* col)) + 4 - this_y_shift) * scale])); //3
		newVertices.Add (new Vector3 (((2 * row) + this_x_shift) * scale, ((-(3 * 2 * col)) - 3 + this_y_shift) * scale, (float)map[((2 * row) + this_x_shift) * scale, (((3 * 2*col)) + 3 - this_y_shift) * scale])); //4
		newVertices.Add (new Vector3 (((2 * row) + this_x_shift) * scale, ((-(3 * 2 * col)) - 1 + this_y_shift) * scale, (float)map[((2 * row) + this_x_shift) * scale, (((3 * 2*col)) + 1 - this_y_shift) * scale])); //5
		
		newTriangles.Add ((triangle_count * 6) + 0);
		newTriangles.Add ((triangle_count * 6) + 1);
		newTriangles.Add ((triangle_count * 6) + 5);
		
		newTriangles.Add ((triangle_count * 6) + 5);
		newTriangles.Add ((triangle_count * 6) + 1);
		newTriangles.Add ((triangle_count * 6) + 2);
		
		newTriangles.Add ((triangle_count * 6) + 5);
		newTriangles.Add ((triangle_count * 6) + 2);
		newTriangles.Add ((triangle_count * 6) + 4);
		
		newTriangles.Add ((triangle_count * 6) + 2);
		newTriangles.Add ((triangle_count * 6) + 3);
		newTriangles.Add ((triangle_count * 6) + 4);

		newUV.Add(new Vector2 (0.5f, 0f));
		newUV.Add(new Vector2 (1f, 0.3f));
		newUV.Add(new Vector2 (1f, 0.7f));
		newUV.Add(new Vector2 (0.5f, 1f));
		newUV.Add(new Vector2 (0f, 0.7f));
		newUV.Add (new Vector2 (0f, 0.3f));

		triangle_count++;
		}

	void BuildHexOdd(int row, int col, float z, Vector2 texture){

		//original working
		/*newVertices.Add (new Vector3 (((2 * row) + 2 + this_x_shift) * scale, ((-(3 * 2 * col)) - 3 + this_y_shift) * scale, z)); //0
		newVertices.Add (new Vector3 (((2 * row) + 4 + this_x_shift) * scale, ((-(3 * 2 * col)) - 4 + this_y_shift) * scale, z)); //1
		newVertices.Add (new Vector3 (((2 * row) + 4 + this_x_shift) * scale, ((-(3 * 2 * col)) - 6 + this_y_shift) * scale, z)); //2
		newVertices.Add (new Vector3 (((2 * row) + 2 + this_x_shift) * scale, ((-(3 * 2 * col)) - 7 + this_y_shift) * scale, z)); //3
		newVertices.Add (new Vector3 (((2 * row) + this_x_shift) * scale, ((-(3 * 2 * col)) - 6 + this_y_shift) * scale, z)); //4
		newVertices.Add (new Vector3 (((2 * row) + this_x_shift) * scale, ((-(3 * 2 * col)) - 4 + this_y_shift) * scale, z)); //5*/


		//kind of working with heights
		/*newVertices.Add (new Vector3 (((2 * row) + 2 + this_x_shift) * scale, ((-(3 * 2 * col)) - 3 + this_y_shift) * scale, (float)map[((2 * row) + 2 + this_x_shift) * scale, (((3 * col)) + 3 - this_y_shift) * scale])); //0 //bottom right?
		newVertices.Add (new Vector3 (((2 * row) + 4 + this_x_shift) * scale, ((-(3 * 2 * col)) - 4 + this_y_shift) * scale, (float)map[((2 * row) + 4 + this_x_shift) * scale, (((3 * col)) + 4 - this_y_shift) * scale])); //1
		newVertices.Add (new Vector3 (((2 * row) + 4 + this_x_shift) * scale, ((-(3 * 2 * col)) - 6 + this_y_shift) * scale, (float)map[((2 * row) + 4 + this_x_shift) * scale, (((3 * col)) + 6 - this_y_shift) * scale])); //2
		newVertices.Add (new Vector3 (((2 * row) + 2 + this_x_shift) * scale, ((-(3 * 2 * col)) - 7 + this_y_shift) * scale, (float)map[((2 * row) + 2 + this_x_shift) * scale, (((3 * col)) + 7 - this_y_shift) * scale])); //3
		newVertices.Add (new Vector3 (((2 * row) + this_x_shift) * scale, ((-(3 * 2 * col)) - 6 + this_y_shift) * scale, (float)map[((2 * row) + this_x_shift) * scale, (((3 * col)) + 6 - this_y_shift) * scale])); //4
		newVertices.Add (new Vector3 (((2 * row) + this_x_shift) * scale, ((-(3 * 2 * col)) - 4 + this_y_shift) * scale, (float)map[((2 * row) + this_x_shift) * scale, (((3 * col)) + 4 - this_y_shift) * scale])); //5*/

		newVertices.Add (new Vector3 (((2 * row) + 2 + this_x_shift) * scale, ((-(3 * 2 * col)) - 3 + this_y_shift) * scale, (float)map[((2 * row) + 2 + this_x_shift) * scale, (((3 * 2 * col)) - this_y_shift) * scale])); // old numbers 3
		newVertices.Add (new Vector3 (((2 * row) + 4 + this_x_shift) * scale, ((-(3 * 2 * col)) - 4 + this_y_shift) * scale, (float)map[((2 * row) + 4 + this_x_shift) * scale, (((3 * 2 * col)) + 1 - this_y_shift) * scale])); // 4
		newVertices.Add (new Vector3 (((2 * row) + 4 + this_x_shift) * scale, ((-(3 * 2 * col)) - 6 + this_y_shift) * scale, (float)map[((2 * row) + 4 + this_x_shift) * scale, (((3 * 2 * col)) + 3 - this_y_shift) * scale])); // 6
		newVertices.Add (new Vector3 (((2 * row) + 2 + this_x_shift) * scale, ((-(3 * 2 * col)) - 7 + this_y_shift) * scale, (float)map[((2 * row) + 2 + this_x_shift) * scale, (((3 * 2 * col)) + 4 - this_y_shift) * scale])); // 7
		newVertices.Add (new Vector3 (((2 * row) + this_x_shift) * scale, ((-(3 * 2 * col)) - 6 + this_y_shift) * scale, (float)map[((2 * row) + this_x_shift) * scale, (((3 * 2 * col)) + 3 - this_y_shift) * scale])); // 6
		newVertices.Add (new Vector3 (((2 * row) + this_x_shift) * scale, ((-(3 * 2 * col)) - 4 + this_y_shift) * scale, (float)map[((2 * row) + this_x_shift) * scale, (((3 * 2 * col)) + 1 - this_y_shift) * scale])); //4
		
		newTriangles.Add ((triangle_count * 6) + 0);
		newTriangles.Add ((triangle_count * 6) + 1);
		newTriangles.Add ((triangle_count * 6) + 5);
		
		newTriangles.Add ((triangle_count * 6) + 5);
		newTriangles.Add ((triangle_count * 6) + 1);
		newTriangles.Add ((triangle_count * 6) + 2);
		
		newTriangles.Add ((triangle_count * 6) + 5);
		newTriangles.Add ((triangle_count * 6) + 2);
		newTriangles.Add ((triangle_count * 6) + 4);
		
		newTriangles.Add ((triangle_count * 6) + 2);
		newTriangles.Add ((triangle_count * 6) + 3);
		newTriangles.Add ((triangle_count * 6) + 4);

		newUV.Add(new Vector2 (0.5f, 0f));
		newUV.Add(new Vector2 (1f, 0.3f));
		newUV.Add(new Vector2 (1f, 0.7f));
		newUV.Add(new Vector2 (0.5f, 1f));
		newUV.Add(new Vector2 (0f, 0.7f));
		newUV.Add (new Vector2 (0f, 0.3f));
		
		triangle_count++;
		}

	void CreateMesh(float z, Vector2 texture){
		for (int offset = 0; offset < ult_col; offset++) {
			this_x_shift = x_shift*offset;
			this_y_shift = y_shift*offset;
			row_index = 0;
			
			for (int row = 0; row < 2; row++) { //fill in on odd or even rows
				for (int col = 0; col < num_cols; col++) {
					
					if (row % 2 == 0) {
						BuildHexEven(row,col,z,texture);
					} else {
						BuildHexOdd(row,col,z, texture);
					}
					row_index++;
				}
			}
		}
		}

	void BuildMesh(){

		mesh.Clear ();
		mesh.vertices = newVertices.ToArray ();
		mesh.triangles = newTriangles.ToArray ();
		mesh.uv = newUV.ToArray();
		mesh.Optimize ();
		mesh.RecalculateNormals ();

		newVertices.Clear();
		newTriangles.Clear();
		newUV.Clear();

		}


}

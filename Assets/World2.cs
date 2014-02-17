using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.IO;

public class World2 : MonoBehaviour {

	public List<Vector3> newVertices = new List<Vector3> ();
	public List<int> newTriangles = new List<int> ();
	public List<Vector2> newUV = new List<Vector2>();
	public List<Color> newColor = new List<Color> ();
	private Mesh mesh;
	private int num_row, num_col;
	private int scale;
	private int hex_count;
	private int vertex_count;
	private int x_shift, y_shift, this_x_shift, this_y_shift, row_index;
	//private float tUnit;
	public double[,] height_map;
	public Hex[,] terrain;
	public static int border = 2;

	private static int x_off = 1;
	private static int y_off = 1;

	Vector2 shift = new Vector2(0,0);
	private static float zoom = 0.1f;

	private static float max_height = 50.0f;

	// Use this for initialization
	void Start () {
	
		//tUnit = 0.25f;
		Vector2 texture = new Vector2 (0,0);
		float z = transform.position.z;
		mesh = GetComponent<MeshFilter> ().mesh;
		hex_count = 0;

		//rows go sideways across
		num_row = 20; //83
		//cols go down
		num_col = (int)(20*1.33); //110
		scale = 200;
		max_height = max_height * scale;

		terrain = new Hex[num_row, num_col];
		//print ("terrain num: " + num_row + " x " + num_col);

		GenerateHeightMap ();
		CreatBoardMesh(z, texture);
		BuildMesh ();

	}
	
	// Update is called once per frame
	void Update () {

	}

	void GenerateHeightMap()
	{

		//double gain = 1.0;

		Vector2 pos;
		float noise = 0.0f;
		int r = num_row*4;
		int c = num_col*3 + 1;
		height_map = new double[r+3,c+4];

		print ("CCCCC: " + c);

		for (int i=0; i<r+3; i++) {
						for (int j=0; j<c+4; j++) {
								if(isBorder (i,j,r,c)){
					height_map[i,j] = 0;
				}else{
								pos = zoom * (new Vector2(i,j)) + shift;
								noise = Mathf.PerlinNoise(pos.x, pos.y);
								height_map[i,j] = (double)(noise*max_height);
				}
						}
				}
	}

	bool isBorder(int i, int j, int r, int c){

				if (i <= (0 + border+1) || i >= (r + 2 - border) || j <= (0 + border) || j >=(c-border)) {
						return true;
				} else {
			print ("for j: " + j + " and c: " + c);
						return false;
				}

		}

	void BuildMesh()
	{
		//fixZ ();
		Vector3[] vert = newVertices.ToArray ();
		int[] tri = newTriangles.ToArray ();
		print ("v length: " + vert.Length);
		for (int i=0; i< vert.Length; i++) {
			print ("vertex: " + vert[i]);
				}
		for (int i=0; i< tri.Length; i++) {
		//	print ("triangle num: " + tri[i]);
		}

		//print ("vertex c: " + vertex_count);

		mesh.Clear ();
		mesh.vertices = newVertices.ToArray ();
		mesh.triangles = newTriangles.ToArray ();
		//mesh.uv = newUV.ToArray();
		mesh.colors = newColor.ToArray ();
		mesh.Optimize ();
		mesh.RecalculateNormals ();
		
		newVertices.Clear();
		newTriangles.Clear();
		//newUV.Clear();
		newColor.Clear ();

	}

	void fixZ(){

		for(int i = 0; i < num_row; i++){
			terrain[i,num_col-3].bottom.y = 0;
			terrain[i,num_col-3].leftBot.y = 0;
			terrain[i,num_col-3].rightBot.y = 0;
		}
	}

	void UpdateMesh()
	{



	}

	void CreatBoardMesh(float z, Vector2 texture)
	{

		for (int i=0; i<num_col; i++) {
						for (int j=0; j<num_row; j++) {
							if(i % 2 == 0){
								BuildHex2(i,j,z,texture, false);
							}else{
								BuildHex2(i,j,z,texture, true);
							}
						}
				}

	}

	void BuildHex2(int y, int x, float depth, Vector2 texture, bool odd){

				x_off = 0;
				y_off = 0;

				int hy = y;
				int hx = x;
		
				if (odd) {
						y--;
						x_off = 2;
						y_off = 3;
				}

				if (hy == 0 && hx == 0) {

						Vector3[] vertices = new Vector3[] {
								new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, "top"), ((((y / 2) * 6) + y_off) * scale)),
								new Vector3 ((((4 * x) + x_off) * scale), getElevation (x, y, x_off, y_off, "leftTop"), (((y / 2) * 6) + y_off + 1) * scale),
								new Vector3 ((((4 * (x + 1) + x_off) * scale)), getElevation (x, y, x_off, y_off, "rightTop"), ((((y / 2) * 6) + y_off + 1) * scale)),
								new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, "center"), (((y / 2) * 6) + y_off + 2) * scale),
								new Vector3 ((((4 * x) + x_off) * scale), getElevation (x, y, x_off, y_off, "leftBot"), (((y / 2) * 6) + y_off + 3) * scale),
								new Vector3 (((4 * (x + 1) + x_off) * scale), getElevation (x, y, x_off, y_off, "rightBot"), (((y / 2) * 6) + y_off + 3) * scale),
								new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, "bottom"), (((y / 2) * 6) + y_off + 4) * scale)
						};
			
						terrain [hx, hy] = new Hex (vertices, hx, hy);

						newVertices.Add (vertices [0]);
						terrain [hx, hy].id_list [0] = vertex_count;
						vertex_count++;

						newVertices.Add (vertices [1]);
						terrain [hx, hy].id_list [1] = vertex_count;
						vertex_count++;

						newVertices.Add (vertices [2]);
						terrain [hx, hy].id_list [2] = vertex_count;
						vertex_count++;

						newVertices.Add (vertices [3]);
						terrain [hx, hy].id_list [3] = vertex_count;
						vertex_count++;

						newVertices.Add (vertices [4]);
						terrain [hx, hy].id_list [4] = vertex_count;
						vertex_count++;

						newVertices.Add (vertices [5]);
						terrain [hx, hy].id_list [5] = vertex_count;
						vertex_count++;

						newVertices.Add (vertices [6]);
						terrain [hx, hy].id_list [6] = vertex_count;
						vertex_count++;

						newTriangles.Add (terrain [hx, hy].id_list [1]);
						newTriangles.Add (terrain [hx, hy].id_list [0]);
						newTriangles.Add (terrain [hx, hy].id_list [3]);
			
						newTriangles.Add (terrain [hx, hy].id_list [0]);
						newTriangles.Add (terrain [hx, hy].id_list [2]);
						newTriangles.Add (terrain [hx, hy].id_list [3]);
			
						newTriangles.Add (terrain [hx, hy].id_list [2]);
						newTriangles.Add (terrain [hx, hy].id_list [5]);
						newTriangles.Add (terrain [hx, hy].id_list [3]);
			
						newTriangles.Add (terrain [hx, hy].id_list [3]);
						newTriangles.Add (terrain [hx, hy].id_list [5]);
						newTriangles.Add (terrain [hx, hy].id_list [6]);
			
						newTriangles.Add (terrain [hx, hy].id_list [4]);
						newTriangles.Add (terrain [hx, hy].id_list [3]);
						newTriangles.Add (terrain [hx, hy].id_list [6]);
			
						newTriangles.Add (terrain [hx, hy].id_list [1]);
						newTriangles.Add (terrain [hx, hy].id_list [3]);
						newTriangles.Add (terrain [hx, hy].id_list [4]);

			/*newUV.Add(new Vector2 (0.5f, 0f));
			newUV.Add(new Vector2 (0.2f, 0.5f));
			newUV.Add(new Vector2 (1f, 0.3f));
			newUV.Add(new Vector2 (0.5f, 0.5f));
			newUV.Add(new Vector2 (0.2f, 0.7f));
			newUV.Add(new Vector2 (0.7f, 0.2f));
			newUV.Add (new Vector2 (0.5f, 1f));*/

			newColor.Add (Color.white);
			newColor.Add (Color.white);
			newColor.Add (Color.white);
			newColor.Add (Color.white);
			newColor.Add (Color.white);
			newColor.Add (Color.white);
			newColor.Add (Color.white);
			
			hex_count++;

				} else if (hy == 0 && hx != 0) {
						Vector3[] vertices = new Vector3[] {
							new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, "top"), ((((y / 2) * 6) + y_off) * scale)),
							terrain [hx - 1, hy].rightTop,
							new Vector3 ((((4 * (x + 1) + x_off) * scale)), getElevation (x, y, x_off, y_off, "rightTop"), ((((y / 2) * 6) + y_off + 1) * scale)),
							new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, "center"), (((y / 2) * 6) + y_off + 2) * scale),
							terrain [hx - 1, hy].rightBot,
							new Vector3 (((4 * (x + 1) + x_off) * scale), getElevation (x, y, x_off, y_off, "rightBot"), (((y / 2) * 6) + y_off + 3) * scale),
							new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, "bottom"), (((y / 2) * 6) + y_off + 4) * scale)
						};

						terrain [hx, hy] = new Hex (vertices, hx, hy);				

						newVertices.Add (vertices [0]);
						terrain [hx, hy].id_list [0] = vertex_count;
						vertex_count++;

						terrain [hx, hy].id_list [1] = terrain [hx - 1, hy].id_list [2];
			
						newVertices.Add (vertices [2]);
						terrain [hx, hy].id_list [2] = vertex_count;
						vertex_count++;
			
						newVertices.Add (vertices [3]);
						terrain [hx, hy].id_list [3] = vertex_count;
						vertex_count++;

						terrain [hx, hy].id_list [4] = terrain [hx - 1, hy].id_list [5];
			
						newVertices.Add (vertices [5]);
						terrain [hx, hy].id_list [5] = vertex_count;
						vertex_count++;
			
						newVertices.Add (vertices [6]);
						terrain [hx, hy].id_list [6] = vertex_count;
						vertex_count++;

						newTriangles.Add (terrain [hx, hy].id_list [1]);
						newTriangles.Add (terrain [hx, hy].id_list [0]);
						newTriangles.Add (terrain [hx, hy].id_list [3]);
					
						newTriangles.Add (terrain [hx, hy].id_list [0]);
						newTriangles.Add (terrain [hx, hy].id_list [2]);
						newTriangles.Add (terrain [hx, hy].id_list [3]);
			
						newTriangles.Add (terrain [hx, hy].id_list [2]);
						newTriangles.Add (terrain [hx, hy].id_list [5]);
						newTriangles.Add (terrain [hx, hy].id_list [3]);
			
						newTriangles.Add (terrain [hx, hy].id_list [3]);
						newTriangles.Add (terrain [hx, hy].id_list [5]);
						newTriangles.Add (terrain [hx, hy].id_list [6]);
			
						newTriangles.Add (terrain [hx, hy].id_list [4]);
						newTriangles.Add (terrain [hx, hy].id_list [3]);
						newTriangles.Add (terrain [hx, hy].id_list [6]);
			
						newTriangles.Add (terrain [hx, hy].id_list [1]);
						newTriangles.Add (terrain [hx, hy].id_list [3]);
						newTriangles.Add (terrain [hx, hy].id_list [4]);

		/*	newUV.Add(new Vector2 (0.5f, 0f));

			newUV.Add(new Vector2 (1f, 0.3f));
			newUV.Add(new Vector2 (0.5f, 0.5f));

			newUV.Add(new Vector2 (0.7f, 0.2f));
			newUV.Add (new Vector2 (0.5f, 1f));*/

			newColor.Add (Color.white);
			newColor.Add (Color.white);
			newColor.Add (Color.white);
			newColor.Add (Color.white);
			newColor.Add (Color.white);

			
			hex_count++;

				} else if (!odd) {
						//even hex in y direc

						if (hy != 0 && hx == 0) {

								Vector3[] vertices = new Vector3[] {
											terrain [hx, hy - 1].leftBot,
											new Vector3 ((((4 * x) + x_off) * scale), getElevation (x, y, x_off, y_off, "leftTop"), (((y / 2) * 6) + y_off + 1) * scale),
											terrain [hx, hy - 1].bottom,
											new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, "center"), (((y / 2) * 6) + y_off + 2) * scale),
											new Vector3 ((((4 * x) + x_off) * scale), getElevation (x, y, x_off, y_off, "leftBot"), (((y / 2) * 6) + y_off + 3) * scale),
											new Vector3 (((4 * (x + 1) + x_off) * scale), getElevation (x, y, x_off, y_off, "rightBot"), (((y / 2) * 6) + y_off + 3) * scale),
											new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, "bottom"), (((y / 2) * 6) + y_off + 4) * scale)
										};

								terrain [hx, hy] = new Hex (vertices, hx, hy);				

								terrain [hx, hy].id_list [0] = terrain [hx, hy - 1].id_list [4];

								newVertices.Add (vertices [1]);
								terrain [hx, hy].id_list [1] = vertex_count;
								vertex_count++;
			
								terrain [hx, hy].id_list [2] = terrain [hx, hy - 1].id_list [6];
			
								newVertices.Add (vertices [3]);
								terrain [hx, hy].id_list [3] = vertex_count;
								vertex_count++;

								newVertices.Add (vertices [4]);
								terrain [hx, hy].id_list [4] = vertex_count;
								vertex_count++;
			
								newVertices.Add (vertices [5]);
								terrain [hx, hy].id_list [5] = vertex_count;
								vertex_count++;
			
								newVertices.Add (vertices [6]);
								terrain [hx, hy].id_list [6] = vertex_count;
								vertex_count++;


								newTriangles.Add (terrain [hx, hy].id_list [1]);
								newTriangles.Add (terrain [hx, hy].id_list [0]);
								newTriangles.Add (terrain [hx, hy].id_list [3]);
					
								newTriangles.Add (terrain [hx, hy].id_list [0]);
								newTriangles.Add (terrain [hx, hy].id_list [2]);
								newTriangles.Add (terrain [hx, hy].id_list [3]);
			
								newTriangles.Add (terrain [hx, hy].id_list [2]);
								newTriangles.Add (terrain [hx, hy].id_list [5]);
								newTriangles.Add (terrain [hx, hy].id_list [3]);
			
								newTriangles.Add (terrain [hx, hy].id_list [3]);
								newTriangles.Add (terrain [hx, hy].id_list [5]);
								newTriangles.Add (terrain [hx, hy].id_list [6]);
			
								newTriangles.Add (terrain [hx, hy].id_list [4]);
								newTriangles.Add (terrain [hx, hy].id_list [3]);
								newTriangles.Add (terrain [hx, hy].id_list [6]);
			
								newTriangles.Add (terrain [hx, hy].id_list [1]);
								newTriangles.Add (terrain [hx, hy].id_list [3]);
								newTriangles.Add (terrain [hx, hy].id_list [4]);


				/*newUV.Add(new Vector2 (0.2f, 0.5f));

				newUV.Add(new Vector2 (0.5f, 0.5f));
				newUV.Add(new Vector2 (0.2f, 0.7f));
				newUV.Add(new Vector2 (0.7f, 0.2f));
				newUV.Add (new Vector2 (0.5f, 1f));*/

				newColor.Add (Color.white);
				newColor.Add (Color.white);
				newColor.Add (Color.white);
				newColor.Add (Color.white);
				newColor.Add (Color.white);
				
				hex_count++;

						} else if (hy != 0 && hx != 0) {

								Vector3[] vertices = new Vector3[] {
					terrain [hx-1, hy - 1].rightBot,
					terrain [hx - 1, hy].rightTop,
					terrain [hx, hy - 1].bottom,
					new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, "center"), (((y / 2) * 6) + y_off + 2) * scale),
					terrain [hx - 1, hy].rightBot,
					new Vector3 (((4 * (x + 1) + x_off) * scale), getElevation (x, y, x_off, y_off, "rightBot"), (((y / 2) * 6) + y_off + 3) * scale),
					new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, "bottom"), (((y / 2) * 6) + y_off + 4) * scale)
				};
				
								terrain [hx, hy] = new Hex (vertices, hx, hy);
				
								terrain [hx, hy].id_list [0] = terrain [hx-1, hy - 1].id_list [5];

								terrain [hx, hy].id_list [1] = terrain [hx - 1, hy].id_list [2];

								terrain [hx, hy].id_list [2] = terrain [hx, hy - 1].id_list [6];

								newVertices.Add (vertices [3]);
								terrain [hx, hy].id_list [3] = vertex_count;
								vertex_count++;

								terrain [hx, hy].id_list [4] = terrain [hx - 1, hy].id_list [5];
				
								newVertices.Add (vertices [5]);
								terrain [hx, hy].id_list [5] = vertex_count;
								vertex_count++;
				
								newVertices.Add (vertices [6]);
								terrain [hx, hy].id_list [6] = vertex_count;
								vertex_count++;


				
								newTriangles.Add (terrain [hx, hy].id_list [1]);
								newTriangles.Add (terrain [hx, hy].id_list [0]);
								newTriangles.Add (terrain [hx, hy].id_list [3]);
				
								newTriangles.Add (terrain [hx, hy].id_list [0]);
								newTriangles.Add (terrain [hx, hy].id_list [2]);
								newTriangles.Add (terrain [hx, hy].id_list [3]);
				
								newTriangles.Add (terrain [hx, hy].id_list [2]);
								newTriangles.Add (terrain [hx, hy].id_list [5]);
								newTriangles.Add (terrain [hx, hy].id_list [3]);
				
								newTriangles.Add (terrain [hx, hy].id_list [3]);
								newTriangles.Add (terrain [hx, hy].id_list [5]);
								newTriangles.Add (terrain [hx, hy].id_list [6]);
				
								newTriangles.Add (terrain [hx, hy].id_list [4]);
								newTriangles.Add (terrain [hx, hy].id_list [3]);
								newTriangles.Add (terrain [hx, hy].id_list [6]);
				
								newTriangles.Add (terrain [hx, hy].id_list [1]);
								newTriangles.Add (terrain [hx, hy].id_list [3]);
								newTriangles.Add (terrain [hx, hy].id_list [4]);


				/*newUV.Add(new Vector2 (0.5f, 0.5f));

				newUV.Add(new Vector2 (0.7f, 0.2f));
				newUV.Add (new Vector2 (0.5f, 1f));*/
				newColor.Add (Color.white);
				newColor.Add (Color.white);
				newColor.Add (Color.white);
				
				hex_count++;

						} else if (hx != 0 && hy != num_col - 1) {
								print ("uncaight scenario in hex make1");
								return;

						} else {
								print ("uncaight scenario in hex make2");
								return;
						}

				} else {
						//odd hex in y direc

						if (hy != 0 && hx == 0 && hx != num_row - 1) {

				//print(hx+" 1 "+hy);


								Vector3[] vertices = new Vector3[]{
								terrain [hx, hy - 1].rightBot,
					            terrain [hx, hy - 1].bottom,
								terrain [hx + 1, hy - 1].bottom,
								new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, "center"), (((y / 2) * 6) + y_off + 2) * scale),
								new Vector3 ((((4 * x) + x_off) * scale), getElevation (x, y, x_off, y_off, "leftBot"), (((y / 2) * 6) + y_off + 3) * scale),
								new Vector3 (((4 * (x + 1) + x_off) * scale), getElevation (x, y, x_off, y_off, "rightBot"), (((y / 2) * 6) + y_off + 3) * scale),
								new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, "bottom"), (((y / 2) * 6) + y_off + 4) * scale)
							};

								terrain [hx, hy] = new Hex (vertices, hx, hy);

								terrain [hx, hy].id_list [0] = terrain [hx, hy - 1].id_list [5];
				
								terrain [hx, hy].id_list [1] = terrain [hx, hy - 1].id_list [6];
				
								terrain [hx, hy].id_list [2] = terrain [hx + 1, hy - 1].id_list [6];
				
								newVertices.Add (vertices [3]);
								terrain [hx, hy].id_list [3] = vertex_count;
								vertex_count++;

								newVertices.Add (vertices [4]);
								terrain [hx, hy].id_list [4] = vertex_count;
								vertex_count++;
				
								newVertices.Add (vertices [5]);
								terrain [hx, hy].id_list [5] = vertex_count;
								vertex_count++;
				
								newVertices.Add (vertices [6]);
								terrain [hx, hy].id_list [6] = vertex_count;
								vertex_count++;
				
								newTriangles.Add (terrain [hx, hy].id_list [1]);
								newTriangles.Add (terrain [hx, hy].id_list [0]);
								newTriangles.Add (terrain [hx, hy].id_list [3]);
				
								newTriangles.Add (terrain [hx, hy].id_list [0]);
								newTriangles.Add (terrain [hx, hy].id_list [2]);
								newTriangles.Add (terrain [hx, hy].id_list [3]);
				
								newTriangles.Add (terrain [hx, hy].id_list [2]);
								newTriangles.Add (terrain [hx, hy].id_list [5]);
								newTriangles.Add (terrain [hx, hy].id_list [3]);
				
								newTriangles.Add (terrain [hx, hy].id_list [3]);
								newTriangles.Add (terrain [hx, hy].id_list [5]);
								newTriangles.Add (terrain [hx, hy].id_list [6]);
				
								newTriangles.Add (terrain [hx, hy].id_list [4]);
								newTriangles.Add (terrain [hx, hy].id_list [3]);
								newTriangles.Add (terrain [hx, hy].id_list [6]);
				
								newTriangles.Add (terrain [hx, hy].id_list [1]);
								newTriangles.Add (terrain [hx, hy].id_list [3]);
								newTriangles.Add (terrain [hx, hy].id_list [4]);


				/*newUV.Add(new Vector2 (0.5f, 0.5f));
				newUV.Add(new Vector2 (0.2f, 0.7f));
				newUV.Add(new Vector2 (0.7f, 0.2f));
				newUV.Add (new Vector2 (0.5f, 1f));*/

				newColor.Add (Color.white);
				newColor.Add (Color.white);
				newColor.Add (Color.white);
				newColor.Add (Color.white);

				
				hex_count++;

				
						} else if (hy!=0 && hx==num_row-1 && hx==0){
				//print(hx+" 2 "+hy);

				Vector3[] vertices = new Vector3[]{
					terrain[hx,hy-1].rightBot,
					terrain[hx,hy-1].bottom,
					new Vector3 ((((4 * (x + 1) + x_off) * scale)), getElevation (x, y, x_off, y_off, "rightTop"), ((((y / 2) * 6) + y_off + 1) * scale)),
					new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, "center"), (((y / 2) * 6) + y_off + 2) * scale),
					new Vector3 ((((4 * x) + x_off) * scale), getElevation (x, y, x_off, y_off, "leftBot"), (((y / 2) * 6) + y_off + 3) * scale),
					new Vector3 (((4 * (x + 1) + x_off) * scale), getElevation (x, y, x_off, y_off, "rightBot"), (((y / 2) * 6) + y_off + 3) * scale),
					new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, "bottom"), (((y / 2) * 6) + y_off + 4) * scale)
				};
				
				terrain[hx,hy] = new Hex(vertices, hx, hy);
				
				terrain[hx,hy].id_list[0] = terrain[hx,hy-1].id_list[5];
				
				terrain[hx,hy].id_list[1] = terrain[hx,hy-1].id_list[6];
				
				newVertices.Add(vertices[2]);
				terrain [hx, hy].id_list [2] = vertex_count;
				vertex_count++;
				
				newVertices.Add(vertices[3]);
				terrain [hx, hy].id_list [3] =vertex_count;
				vertex_count++;
				
				newVertices.Add (vertices[4]);
				terrain[hx,hy].id_list[4] = vertex_count;
				vertex_count++;
				
				newVertices.Add (vertices [5]);
				terrain [hx, hy].id_list [5] = vertex_count;
				vertex_count++;
				
				newVertices.Add (vertices [6]);
				terrain [hx, hy].id_list [6] = vertex_count;
				vertex_count++;
				
				newTriangles.Add (terrain [hx, hy].id_list [1]);
				newTriangles.Add (terrain [hx, hy].id_list [0]);
				newTriangles.Add (terrain [hx, hy].id_list [3]);
				
				newTriangles.Add (terrain [hx, hy].id_list [0]);
				newTriangles.Add (terrain [hx, hy].id_list [2]);
				newTriangles.Add (terrain [hx, hy].id_list [3]);
				
				newTriangles.Add (terrain [hx, hy].id_list [2]);
				newTriangles.Add (terrain [hx, hy].id_list [5]);
				newTriangles.Add (terrain [hx, hy].id_list [3]);
				
				newTriangles.Add (terrain [hx, hy].id_list [3]);
				newTriangles.Add (terrain [hx, hy].id_list [5]);
				newTriangles.Add (terrain [hx, hy].id_list [6]);
				
				newTriangles.Add (terrain [hx, hy].id_list [4]);
				newTriangles.Add (terrain [hx, hy].id_list [3]);
				newTriangles.Add (terrain [hx, hy].id_list [6]);
				
				newTriangles.Add (terrain [hx, hy].id_list [1]);
				newTriangles.Add (terrain [hx, hy].id_list [3]);
				newTriangles.Add (terrain [hx, hy].id_list [4]);


				/*newUV.Add(new Vector2 (1f, 0.3f));
				newUV.Add(new Vector2 (0.5f, 0.5f));
				newUV.Add(new Vector2 (0.2f, 0.7f));
				newUV.Add(new Vector2 (0.7f, 0.2f));
				newUV.Add (new Vector2 (0.5f, 1f));*/

				newColor.Add (Color.white);
				newColor.Add (Color.white);
				newColor.Add (Color.white);
				newColor.Add (Color.white);
				newColor.Add (Color.white);

				hex_count++;

				} else if(hy!=0 && hx == num_row-1){

				//print(hx+" 3 "+hy);


				Vector3[] vertices = new Vector3[]{
					terrain[hx,hy-1].rightBot,
					terrain[hx,hy-1].bottom,
					new Vector3 ((((4 * (x + 1) + x_off) * scale)), getElevation (x, y, x_off, y_off, "rightTop"), ((((y / 2) * 6) + y_off + 1) * scale)),
					new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, "center"), (((y / 2) * 6) + y_off + 2) * scale),
					terrain[hx-1,hy].rightBot,
					new Vector3 (((4 * (x + 1) + x_off) * scale), getElevation (x, y, x_off, y_off, "rightBot"), (((y / 2) * 6) + y_off + 3) * scale),
					new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, "bottom"), (((y / 2) * 6) + y_off + 4) * scale)
				};

				terrain[hx,hy] = new Hex(vertices, hx, hy);

				terrain[hx,hy].id_list[0] = terrain[hx,hy-1].id_list[5];
				
				terrain[hx,hy].id_list[1] = terrain[hx,hy-1].id_list[6];

				newVertices.Add(vertices[2]);
				terrain [hx, hy].id_list [2] = vertex_count;
				vertex_count++;
				
				newVertices.Add(vertices[3]);
				terrain [hx, hy].id_list [3] =vertex_count;
				vertex_count++;

				terrain[hx,hy].id_list[4] = terrain[hx-1,hy].id_list[5];
				
				newVertices.Add (vertices [5]);
				terrain [hx, hy].id_list [5] = vertex_count;
				vertex_count++;
				
				newVertices.Add (vertices [6]);
				terrain [hx, hy].id_list [6] = vertex_count;
				vertex_count++;

				newTriangles.Add (terrain [hx, hy].id_list [1]);
				newTriangles.Add (terrain [hx, hy].id_list [0]);
				newTriangles.Add (terrain [hx, hy].id_list [3]);
				
				newTriangles.Add (terrain [hx, hy].id_list [0]);
				newTriangles.Add (terrain [hx, hy].id_list [2]);
				newTriangles.Add (terrain [hx, hy].id_list [3]);
				
				newTriangles.Add (terrain [hx, hy].id_list [2]);
				newTriangles.Add (terrain [hx, hy].id_list [5]);
				newTriangles.Add (terrain [hx, hy].id_list [3]);
				
				newTriangles.Add (terrain [hx, hy].id_list [3]);
				newTriangles.Add (terrain [hx, hy].id_list [5]);
				newTriangles.Add (terrain [hx, hy].id_list [6]);
				
				newTriangles.Add (terrain [hx, hy].id_list [4]);
				newTriangles.Add (terrain [hx, hy].id_list [3]);
				newTriangles.Add (terrain [hx, hy].id_list [6]);
				
				newTriangles.Add (terrain [hx, hy].id_list [1]);
				newTriangles.Add (terrain [hx, hy].id_list [3]);
				newTriangles.Add (terrain [hx, hy].id_list [4]);


				//newUV.Add(new Vector2 (1f, 0.3f));
				//newUV.Add(new Vector2 (0.5f, 0.5f));

				//newUV.Add(new Vector2 (0.7f, 0.2f));
				//newUV.Add (new Vector2 (0.5f, 1f));

				newColor.Add (Color.white);
				newColor.Add (Color.white);
				newColor.Add (Color.white);
				newColor.Add (Color.white);
				
				hex_count++;

				
						} else if(hy!=0 && hx!=num_row-1){

				//print(hx+" 4 "+hy);

				//print ("vert: " + vertex_count);

				Vector3[] vertices = new Vector3[]{
					terrain[hx,hy-1].rightBot,
					terrain[hx,hy-1].bottom,
					terrain[hx+1,hy-1].bottom,
					new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, "center"), (((y / 2) * 6) + y_off + 2) * scale),
					terrain[hx-1,hy].rightBot,
					new Vector3 (((4 * (x + 1) + x_off) * scale), getElevation (x, y, x_off, y_off, "rightBot"), (((y / 2) * 6) + y_off + 3) * scale),
					new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, "bottom"), (((y / 2) * 6) + y_off + 4) * scale)
				};

				terrain[hx,hy] = new Hex(vertices, hx, hy);
				
				terrain[hx,hy].id_list[0] = terrain[hx,hy-1].id_list[5];
				
				terrain[hx,hy].id_list[1] = terrain[hx,hy-1].id_list[6];

				terrain [hx, hy].id_list [2] = terrain[hx+1,hy-1].id_list[6];
				
				newVertices.Add(vertices[3]);
				terrain [hx, hy].id_list [3] = vertex_count;
				vertex_count++;
				
				terrain[hx,hy].id_list[4] = terrain[hx-1,hy].id_list[5];
				
				newVertices.Add (vertices [5]);
				terrain [hx, hy].id_list [5] = vertex_count;
				vertex_count++;
				
				newVertices.Add (vertices [6]);
				terrain [hx, hy].id_list [6] = vertex_count;
				vertex_count++;

				newTriangles.Add (terrain [hx, hy].id_list [1]);
				newTriangles.Add (terrain [hx, hy].id_list [0]);
				newTriangles.Add (terrain [hx, hy].id_list [3]);
				
				newTriangles.Add (terrain [hx, hy].id_list [0]);
				newTriangles.Add (terrain [hx, hy].id_list [2]);
				newTriangles.Add (terrain [hx, hy].id_list [3]);
				
				newTriangles.Add (terrain [hx, hy].id_list [2]);
				newTriangles.Add (terrain [hx, hy].id_list [5]);
				newTriangles.Add (terrain [hx, hy].id_list [3]);
				
				newTriangles.Add (terrain [hx, hy].id_list [3]);
				newTriangles.Add (terrain [hx, hy].id_list [5]);
				newTriangles.Add (terrain [hx, hy].id_list [6]);
				
				newTriangles.Add (terrain [hx, hy].id_list [4]);
				newTriangles.Add (terrain [hx, hy].id_list [3]);
				newTriangles.Add (terrain [hx, hy].id_list [6]);
				
				newTriangles.Add (terrain [hx, hy].id_list [1]);
				newTriangles.Add (terrain [hx, hy].id_list [3]);
				newTriangles.Add (terrain [hx, hy].id_list [4]);


				/*newUV.Add(new Vector2 (0.5f, 0.5f));

				newUV.Add(new Vector2 (0.7f, 0.2f));
				newUV.Add (new Vector2 (0.5f, 1f));*/

				newColor.Add (Color.white);
				newColor.Add (Color.white);
				newColor.Add (Color.white);
				
				hex_count++;
				
						} else{
							print ("uncaught scenario in hex make");
							return;
						}

				}

		}

	void BuildHex(int y, int x, float depth, Vector2 texture, bool odd){

		x_off = 0;
		y_off = 0;

		if (odd) 
		{
			y--;
			x_off = 2;
			y_off = 3;
		}

		int i = x;
		int j = y;

		//top
		newVertices.Add (( new Vector3( (((((2*x)+1)*2)+x_off)*scale), getElevation(x,y,x_off,y_off,"top"),  ((((y/2)*6)+y_off)*scale) )));
		//leftTop
		newVertices.Add (( new Vector3( (((4*x)+x_off)*scale), getElevation(x,y,x_off,y_off,"leftTop"),  (((y/2)*6)+y_off+1)*scale) ));
		//rightTop
		newVertices.Add (( (new Vector3( (((4*(x+1)+x_off)* scale)), getElevation(x,y,x_off,y_off,"rightTop"), ((((y/2)*6)+y_off+1)*scale))) ));
		//center
		newVertices.Add (( new Vector3( (((((2*x)+1)*2)+x_off)*scale), getElevation(x,y,x_off,y_off,"center"), (((y/2)*6)+y_off+2)*scale) ));
		//leftBottom
		newVertices.Add (( new Vector3( (((4*x)+x_off)*scale), getElevation(x,y,x_off,y_off,"leftBot"), (((y/2)*6)+y_off+3)*scale) ));
		//rightBottom
		newVertices.Add (( new Vector3( ((4*(x+1)+x_off)* scale), getElevation(x,y,x_off,y_off,"rightBot"), (((y/2)*6)+y_off+3)*scale) ));
		//bottom
		newVertices.Add (( new Vector3( (((((2*x)+1)*2)+x_off)*scale), getElevation(x,y,x_off,y_off,"bottom"), (((y/2)*6)+y_off+4)*scale) ));

		newTriangles.Add ((hex_count*7)+1);
		newTriangles.Add ((hex_count*7)+0);
		newTriangles.Add ((hex_count*7)+3);
		
		newTriangles.Add ((hex_count*7)+0);
		newTriangles.Add ((hex_count*7)+2);
		newTriangles.Add ((hex_count*7)+3);

		newTriangles.Add ((hex_count*7)+2);
		newTriangles.Add ((hex_count*7)+5);
		newTriangles.Add ((hex_count*7)+3);

		newTriangles.Add ((hex_count*7)+3);
		newTriangles.Add ((hex_count*7)+5);
		newTriangles.Add ((hex_count*7)+6);

		newTriangles.Add ((hex_count*7)+4);
		newTriangles.Add ((hex_count*7)+3);
		newTriangles.Add ((hex_count*7)+6);

		newTriangles.Add ((hex_count*7)+1);
		newTriangles.Add ((hex_count*7)+3);
		newTriangles.Add ((hex_count*7)+4);

		newUV.Add(new Vector2 (0.5f, 0f));
		newUV.Add(new Vector2 (0.2f, 0.5f));
		newUV.Add(new Vector2 (1f, 0.3f));
		newUV.Add(new Vector2 (0.5f, 0.5f));
		newUV.Add(new Vector2 (0.2f, 0.7f));
		newUV.Add(new Vector2 (0.7f, 0.2f));
		newUV.Add (new Vector2 (0.5f, 1f));

		hex_count++;

	}

	int getElevation(int x, int y, int x_o, int y_o, string vertex){

				//print ("x: " + x + " " + y + " center at: " + ((2 * x + 1) * 2 + x_o) + " and " + (6 * (y / 2) + 2 + y_o));

				int x_hex = ((2 * x + 1) * 2 + x_o);
				int y_hex = (6 * (y / 2) + 2 + y_o);

				if (vertex == "top") {
						return -((int)height_map[x_hex,y_hex-2]);
				} else if (vertex == "leftTop") {
						return -((int)height_map[x_hex-2,y_hex-1]);
				} else if (vertex == "rightTop") {
						//print ("x_hex+2 " + (x_hex+2) + " y_hex-1 " + (y_hex-1));
						return -((int)height_map[x_hex+2,y_hex-1]);
				} else if (vertex == "center") {
						return -((int)height_map[x_hex,y_hex]);
				} else if (vertex == "leftBot") {
						return -((int)height_map[x_hex-2,y_hex+1]);
				} else if (vertex == "rightBot") {
						return -((int)height_map[x_hex+2,y_hex+1]);
				} else if (vertex == "bottom") {
						return -((int)height_map[x_hex,y_hex+2]);
				} else {
						return 1;
				}
		}



	void addVertex(Vector3 vert){
		newVertices.Add (vert);
		vertex_count++;
		}

	void addVertices(ref Hex hex, int num_new_vert){

		addVertex (hex.top);

		addVertex (hex.leftTop);
		addVertex (hex.rightTop);
		addVertex (hex.center);
		addVertex (hex.leftBot);
		addVertex (hex.rightBot);
		addVertex (hex.bottom);
	}


}

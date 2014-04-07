using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.IO;

public class Hex {
	
	public Vector3 top, leftTop, rightTop, center, leftBot, rightBot, bottom;
	public Vector2 pos;
	public int[] id_list;
	public int scale;
	public string type;
	public Vector3[] vertices;
	public bool river = false;
	public bool coast = false;
	public string[] lowestNeighbor = new string[7];
	public bool city = false;
	public float defense = 0f;
	public float industry = 0f;
	public float food = 0f;

		public Hex(){
			if(UnityEngine.Random.Range(0,100) > 50)
				type = "water";
			else
				type = "land";

			this.top = new Vector3();
			this.leftTop = new Vector3();
			this.rightTop = new Vector3();
			this.center = new Vector3();
			this.leftBot = new Vector3();
			this.rightBot = new Vector3();
			this.bottom = new Vector3();

			this.vertices = new Vector3[7] {this.top,this.leftTop,this.rightTop,this.center,this.leftBot,this.rightBot,this.bottom};
		}

		public Hex(Vector3[] vertex_list, int x, int y)
		{
			this.top = vertex_list[0];
			this.leftTop = vertex_list [1];
			this.rightTop = vertex_list [2];
			this.center = vertex_list [3];
			this.leftBot = vertex_list [4];
			this.rightBot = vertex_list [5];
			this.bottom = vertex_list [6];
			this.pos = new Vector2 (x, y);
			//this.id_list = ids;
			this.id_list = new int[7];
			this.scale = 1;
			this.vertices = vertex_list;
		}

		public Hex(Vector3[] vertex_list, int x, int y, int scl)
		{
			this.top = vertex_list[0];
			this.leftTop = vertex_list [1];
			this.rightTop = vertex_list [2];
			this.center = vertex_list [3];
			this.leftBot = vertex_list [4];
			this.rightBot = vertex_list [5];
			this.bottom = vertex_list [6];
			this.pos = new Vector2 (x, y);
			//this.id_list = ids;
			this.id_list = new int[7];
			this.scale = scl;
			this.vertices = vertex_list;
		}

		public bool inBoundingBox(Vector3 point){

			//float v = (2*this.scale)/(2*Mathf.Cos(30));
			float v = 2f;

			//Debug.Log("Check if " + point + " is in " + this.center);

			if(point.x < (this.center.x + v) && point.x > this.center.x-v && point.y < this.center.z+v && point.y>this.center.z-v){
				//Debug.Log("Inside Bounding Box");
				//inside bounding box so far...
				//Check four outer cases
				Vector3 v1 = new Vector3(this.center.x-v, this.center.z+v, 0);
				Vector3 v2 = new Vector3(this.center.x+v, this.center.z-v,0);
				Vector3 v3 = new Vector3(this.center.x-v, this.center.z-v, 0);
				Vector3 v4 = new Vector3(this.center.x+v, this.center.z+v, 0);

				//Debug.Log("vector3 : " + v3 + " " + this.top + " " + this.leftTop);

				bool b1, b2, b3, b4;
				b1 = pointInTriangle(new Vector3(point.x, point.y,0), new Vector3(this.bottom.x,this.bottom.z,0), v1, new Vector3(this.leftBot.x+0.1f,this.leftBot.z+0.1f,0));
				b2 = pointInTriangle(new Vector3(point.x, point.y,0), new Vector3(this.top.x,this.top.z,0), v2, new Vector3(this.rightTop.x,this.rightTop.z,0));
				b3 = pointInTriangle(new Vector3(point.x, point.y,0), new Vector3(this.top.x,this.top.z,0), v3, new Vector3(this.leftTop.x,this.leftTop.z,0));
				b4 = pointInTriangle(new Vector3(point.x, point.y,0), new Vector3(this.bottom.x,this.bottom.z,0), v4, new Vector3(this.rightBot.x,this.rightBot.z,0));
				//Debug.Log(b1 + " " + b2 + " " + b3 + " " + b4);
				return (!(b1 || b2 || b3 || b4));
			}else{
				return false;
			}

		}

		public float sign(Vector3 p1, Vector3 p2, Vector3 p3)
		{
  			return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
		}

		public bool pointInTriangle(Vector3 p, Vector3 p1, Vector3 p2, Vector3 p3){

			float alpha = ((p2.y - p3.y)*(p.x - p3.x) + (p3.x - p2.x)*(p.y - p3.y)) / ((p2.y - p3.y)*(p1.x - p3.x) + (p3.x - p2.x)*(p1.y - p3.y));
			float beta = ((p3.y - p1.y)*(p.x - p3.x) + (p1.x - p3.x)*(p.y - p3.y)) / ((p2.y - p3.y)*(p1.x - p3.x) + (p3.x - p2.x)*(p1.y - p3.y));
			float gamma = 1.0f - alpha - beta;

			return (alpha > 0f && beta > 0f && gamma > 0f);

		}

		public void addParams(Vector3[] vertex_list, int x, int y)
		{
			this.top = vertex_list[0];
			this.leftTop = vertex_list [1];
			this.rightTop = vertex_list [2];
			this.center = vertex_list [3];
			this.leftBot = vertex_list [4];
			this.rightBot = vertex_list [5];
			this.bottom = vertex_list [6];
			this.pos = new Vector2 (x, y);
			//this.id_list = ids;
			this.id_list = new int[7];
			this.scale = 1;
			this.vertices = vertex_list;
		}

		public void printHex(){
			Debug.Log(this.top + " " + this.leftTop + " " + this.rightTop + " " + this.center + " " + this.leftBot + " " + this.rightBot + " " + this.bottom);
		}

		public void calcValues(){

			if(type=="rock"){
				this.defense = 1f;
				this.industry = 2f;
				this.food = 1f;
			}else if(type=="plains"){
				this.defense = 0.1f;
				this.industry = 1f;
				this.food = 3f;
			}else if(type=="forest"){
				this.defense = 1f;
				this.industry = 2f;
				this.food = 2f;
			}else if(type=="shallow_water"){
				this.defense = 2f;
				this.industry = 3f;
				this.food = 4f;
			}else if(type=="deep_water"){
				this.defense = 0.1f;
				this.industry = 1f;
				this.food = 1f;
			}else if(type=="jungle"){
				this.defense = 5f;
				this.industry = 2f;
				this.food = 2f;
			}else if(type=="desert"){
				this.defense = 0.1f;
				this.industry = 0.1f;
				this.food = 1f;
			}else if(type=="sand"){
				this.defense = -1f;
				this.industry = 1f;
				this.food = 2f;
			}else if(type=="grass"){
				this.defense = 0.1f;
				this.industry = 1f;
				this.food = 2f;
			}

			this.addNoiseToValues();

		}
			public void addNoiseToValues(){
				this.defense += (((float)UnityEngine.Random.Range(-5,5) % this.defense) / this.defense);
				this.industry += (((float)UnityEngine.Random.Range(-5,5) % this.industry) / this.industry);
				this.food += (((float)UnityEngine.Random.Range(-5,5) % this.food) / this.food);
			}
		

	}



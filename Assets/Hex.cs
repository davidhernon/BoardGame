using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.IO;

public class Hex {
	
	public Vector3 top, leftTop, rightTop, center, leftBot, rightBot, bottom;
	public Vector2 pos;
	public int[] id_list;

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
		}
	}



using System.Collections.Generic;
using UnityEngine;


public class TerrainFunctions : MonoBehaviour {

	void Start(){

	}

	void Update(){

	}

	public static List<Vector3> river(List<Vector3> l, Hex h, Vector3 last, Hex[,] terrain, int max_x, int max_y, int c){

		int x = (int)h.pos.x;
		int y = (int)h.pos.y;

		if( c == 0 ){
			return l;
		}
		c--;

		Hex minH = h; 
		Vector3 minV = last; //minimum vector so far
		Vector3 tLast = last;
		bool t = (last == h.rightTop);

		if(h.type == "water"){
			return l;
		}

		if(y % 2 ==0 ){
			//even row
			
			if(last == h.top){	
				minV = h.top;

				if(h.leftTop.y > minV.y){
					minV = h.leftTop;
				}
				if(h.rightTop.y > minV.y){
					minV = h.rightTop;
				}
				if(h.center.y > minV.y){
					minV = h.center;
				}

				if(y!=max_y-1){
					if(terrain[x,y+1].rightTop.y > minV.y){

						minH = terrain[x,y+1];
						minV = minH.rightTop;

						
					}
					if(terrain[x,y+1].center.y > minV.y){

						minH = terrain[x,y+1];
						minV = minH.center;

						
					}

				}
				if(x!=0){
					if(terrain[x-1,y].center.y > minV.y){
						minH = terrain[x,y+1];
						minV = minH.center;
					}
				}

				last = minV;
			}else if(last == h.leftTop){
				
				minV = h.leftTop;

				if(h.top.y > minV.y){
					last = minV;
					minV = h.top;
				}
				if(h.leftBot.y > minV.y){
					last = minV;
					minV = h.leftBot;
				}
				if(h.center.y > minV.y){
					minV = h.center;
				}
				if(y!=max_y-1){
					if(terrain[x,y+1].leftBot.y > minV.y){
						last = minV;
						minV = terrain[x,y+1].leftBot;
						minH = terrain[x,y+1];
					}
					if(terrain[x,y+1].center.y > minV.y){
						minH = terrain[x,y+1];
						minV = minH.center;
					}
				}
				if(x!=max_x-1){
					if(terrain[x+1,y].center.y > minV.y){
						minH = terrain[x+1,y];
						minV = minH.center;
					}
				}
				last = minV;
			}else if(last == h.rightTop){
				minV = h.rightTop;
				if(h.top.y > minV.y){
					last = minV;
					minV = h.top;
				}
				if(h.rightBot.y > minV.y){
					last = minV;
					minV = h.rightBot;
				}
				if(h.center.y > minV.y){
					minV = h.center;
				}
				if(x != 0){
					if(terrain[x-1,y].top.y > minV.y){
						last = minV;
						minV = terrain[x-1,y].top;
						minH = terrain[x-1,y];
					}
					if(terrain[x-1,y].center.y > minV.y){
						minH = terrain[x-1,y];
						minV = minH.center;
					}
				}
				if(y!=max_y-1){
					if(terrain[x,y+1].center.y > minV.y){
						minH = terrain[x,y+1];
						minV = minH.center;
					}
				}
				last = minV;
			}else if(last == h.center){
				minV = h.center;
				//Vector3 cen = new Vector3();
//				return l;
				/*for(int z=0; z < 7; z++){
					if(h.vertices[z].y > minV.y ){
						last = minV
						cen = h.vertices[z];
					}
				}*/
				//minV = cen;
				minV = findLowestNieghborForCenter(h, h.center);
				last = minV;
			}else if(last == h.leftBot){
				minV = h.leftBot;


				if(h.rightTop.y > minV.y){
					last = minV;
					minV = h.rightTop;
				}
				if(h.bottom.y > minV.y){
					last = minV;
					minV = h.bottom;
				}
				if(h.center.y > minV.y){
					minV = h.center;
				}
				if(x!=max_x-1){
					if(terrain[x+1,y].bottom.y > minV.y){
						last = terrain[x+1,y].bottom;
						minV = terrain[x+1,y].bottom;
						minH = terrain[x+1,y];
					}
					if(terrain[x+1,y].center.y > minV.y){
						minH = terrain[x+1,y];
						minV = minH.center;
					}
				}
				if(y!=0){
					if(terrain[x,y-1].center.y > minV.y){
						minH = terrain[x,y-1];
						minV = minH.center;
					}
				}
				last = minV;
			}else if(last == h.bottom){
				minV = h.bottom;

				if(h.leftBot.y > minV.y){
					last = minV;
					minV = h.leftBot;
				}
				if(h.rightBot.y > minV.y){
					last = minV;
					minV = h.rightBot;
				}
				if(h.center.y > minV.y){
					minV = h.center;
				}
				if(y!=0){
					if(terrain[x,y-1].leftBot.y > minV.y){
						last = minV;
						minV = terrain[x,y-1].leftBot;
						minH = terrain[x,y-1];
					}
					if(terrain[x,y-1].center.y > minV.y){
						minV = terrain[x,y-1].center;
						minH = terrain[x,y-1];
					}
					if(x!=max_y-1){
						if(terrain[x+1,y-1].center.y > minV.y){
							minH = terrain[x+1,y-1];
							minV = minH.center;
						}
					}
				}

				last = minV;
			}else if(last == h.rightBot){
				minV = h.rightBot;

				if(h.rightTop.y > minV.y){
					last = minV;
					minV = h.rightTop;
				}
				if(h.bottom.y > minV.y){
					last = minV;
					minV = h.bottom;
				}
				if(h.center.y > minV.y ){
					minV = h.center;
				}
				if(x!=0){
					if(terrain[x-1,y].bottom.y > minV.y){
						last = minV;
						minV = terrain[x-1,y].bottom;
						minH = terrain[x-1,y];
					}
					if(terrain[x-1,y].center.y > minV.y){
						minH = terrain[x-1,y];
						minV = minH.center;
					}
					if(y!=0){
						if(terrain[x-1,y-1].center.y > minV.y){
							minH = terrain[x-1,y-1];
							minV = minH.center;
						}
					}
				}
				last = minV;
			}

		}else{
			//odd row
			if(last == h.top){
				minV = h.top;

				if(h.rightTop.y > minV.y){
					minV = h.rightTop;
				}
				if(h.leftTop.y > minV.y){
					minV = h.leftTop;
				}
				if(h.center.y > minV.y){
					minV = h.center;
				}
				if(y!=max_y-1){
					if(terrain[x,y+1].rightTop.y > minV.y){
						minH = terrain[x,y+1];
						minV = minH.rightTop;
					}
					if(terrain[x,y+1].center.y > minV.y){
						minH = terrain[x,y+1];
						minV = minH.rightTop;
					}
					if(x!=0){
						if(terrain[x-1,y+1].center.y > minV.y){
							minH = terrain[x-1,y+1];
							minV = minH.center;
						}
					}
				}
				last = minV;
			}else if(last == h.leftTop){
				minV = h.leftTop;

				if(h.top.y > minV.y){
					minV = h.top;
				}
				if(h.leftBot.y > minV.y){
					minV = h.leftBot;
				}
				if(h.center.y > minV.y){
					minV = h.center;
				}
				if(x!=max_x-1){
					if(terrain[x+1,y].top.y > minV.y){
						minH = terrain[x+1,y];
						minV = minH.top;
					}
					if(terrain[x+1,y].center.y > minV.y){
						minH = terrain[x+1,y];
						minV = minH.center;
					}

				}
				if(y!=max_y-1){
					if(terrain[x,y+1].center.y > minV.y){
						minH = terrain[x,y+1];
						minV = minH.center;
					}
				}
				last = minV;
			}else if(last == h.rightTop){
				minV = h.rightTop;
				if(h.rightBot.y > minV.y){
					minV = h.rightBot;
				}
				if(h.top.y > minV.y){
					minV = h.top;
				}
				if(h.center.y > minV.y){
					minV = h.center;
				}
				if(x!=0){
					if(terrain[x-1,y].top.y > minV.y){
						minH = terrain[x-1,y];
						minV = minH.top;
					}
					if(terrain[x-1,y].center.y > minV.y){
						minH = terrain[x-1,y];
						minV = minH.top;
					}
				}
				if(y!=max_y-1){
					if(terrain[x,y+1].center.y > minV.y){
						minH = terrain[x,y+1];
						minV = minH.center;
					}
				}
				last = minV;
			}else if(last == h.center){
				//return l;
				/*
				minV = h.center;
				for(int z=0; z > 7; z++){
					if(h.vertices[z].y > minV.y ){
						last = minV
						minV = h.vertices[z].y;
					}
				}*/
				minV = findLowestNieghborForCenter(h,h.center);
				last = minV;
			}else if(last == h.leftBot){
				minV = h.leftBot;

				if(h.leftTop.y > minV.y){
					minV = h.leftTop;
				}
				if(h.bottom.y > minV.y){
					minV = h.bottom;
				}
				if(h.center.y > minV.y){
					minV = h.center;
				}
				if(x!=max_x-1){
					if(terrain[x+1,y].bottom.y > minV.y){
						minH = terrain[x+1,y];
						minV = minH.bottom;
					}
					if(terrain[x+1,y].center.y > minV.y){
						minH = terrain[x+1,y];
						minV = minH.center;
					}
					if(y!=0){
						if(terrain[x+1,y+1].center.y > minV.y){
							minH = terrain[x+1,y];
							minV = minH.center;
						}
					}
				}
				last = minV;
			}else if(last == h.rightBot){
				minV = h.rightBot;
				
				if(h.rightTop.y>minV.y){
					minV = h.rightTop;
				}
				if(h.bottom.y > minV.y){
					minV = h.bottom;
				}
				if(h.center.y > minV.y){
					minV = h.center;
				}
				if(y!=0){
					if(terrain[x,y-1].rightTop.y > minV.y){
						minH = terrain[x,y-1];
						minV = minH.rightTop;
					}
					if(terrain[x,y-1].center.y > minV.y){
						minH = terrain[x,y-1];
						minV = minH.center;
					}
				}
				if(x!=0){
					if(terrain[x-1,y].center.y > minV.y){
						minH = terrain[x-1,y];
						minV = minH.center;
					}
				}
				last = minV;
			}else if(last == h.bottom){
				minV = h.bottom;

				if(h.leftBot.y > minV.y ){
					minV = h.leftBot;
				}
				if(h.rightBot.y > minV.y){
					minV = h.rightBot;
				}
				if(h.center.y > minV.y){
					minV = h.center;
				}
				if(y != 0){
					if(terrain[x,y-1].leftBot.y  > minV.y){
						minH = terrain[x,y-1];
						minV = minH.leftBot;
					}
					if(terrain[x,y-1].center.y  > minV.y){
						minH = terrain[x,y-1];
						minV = minH.center;
					}
					if(x!=max_x-1){
						if(terrain[x+1,y-1].center.y > minV.y){
							minH = terrain[x,y-1];
							minV = minH.center;
						}
					}
				}

				last = minV;
			}

		}

		if(tLast == minV ){
			return l;
		}else{
			minH.river = true;
			l.Add(minV);
			h.river = true;
			return river(l,minH,last,terrain,max_x,max_y,c);
		}

		

	}

	public static List<Vector3> randomRiver(Hex[,] terrain, int max_x, int max_y){

		List<Vector3> rivers = new List<Vector3>();
		int x = (Random.Range(0,100) % max_x);
		int y = (Random.Range(0,100) % max_y);
		while(terrain[x,y].type == "water"){
			x = Random.Range(0,100) % max_x;
			y = Random.Range(0,100) % max_y;
		}
		rivers.Add(terrain[x,y].center);

		return rivers;

	}

	public static List<Vector3> addRivers(Hex[,] terrain, int max_x, int max_y){
		List<Vector3> rivers = new List<Vector3>();
		int x = (Random.Range(0,100) % max_x);
		int y = (Random.Range(0,100) % max_y);
		while(terrain[x,y].type == "water"){
			x = Random.Range(0,100) % max_x;
			y = Random.Range(0,100) % max_y;
		}
		rivers.Add(terrain[x,y].top);
		return river(rivers, terrain[x,y], terrain[x,y].top, terrain, max_x, max_y, max_y);
	}

	public static void printRiver(List<Vector3> l){
		foreach(Vector3 item in l){
		}
	}

	public static void addRiversToBoard(List<Vector3> rivers){

		for(int i=1; i < rivers.Count; i++){
			//createCylinderBetweenPoints(rivers[i], rivers[i-1], 0.1f);
		}

	}

	public static void createCylinderBetweenPoints(Vector3 start, Vector3 end, float width){
		Vector3 offset = end - start;
		Vector3 scale = new Vector3(width, (offset.magnitude / 2.0f) - 0.1f, width);
		Vector3 position = start + (offset / 2.0f);

		GameObject cyl = (GameObject)Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cylinder), position, Quaternion.identity);
		cyl.transform.up = offset;
		cyl.transform.localScale = scale;
		cyl.transform.parent = GameObject.Find("Rivers").transform;
		cyl.renderer.material.color = Color.cyan;
	}

	public static Vector3 findLowestNieghborForCenter(Hex h, Vector3 min){
		for(int i=0; i < 7; i++){
			if(h.vertices[i].y > min.y){
				min = h.vertices[i];
			}
		}
		return min;
	}

	public static void setCoast(Hex[,] terrain, int max_x, int max_y){
		for(int i=0; i<max_x; i++){
			for(int j=0; j<max_y; j++){
				if(terrain[i,j].type != "water"){
					if(countNeighborsOfType(terrain, i, j, "water") > 0){
						terrain[i,j].coast = true;
					}
				}
			}
		}
	}

	public static double[,] createMoistureMap(int x, int y){
		double[,] water = new double[x,y];
		for(int i=0; i< x; i++){
			for(int j=0; j < y; j++){
				Vector2 pos = World3.zoom * (new Vector2(i,j)) + World3.shift;
				float noise = Mathf.PerlinNoise(pos.x, pos.y);
				water[i,j] = (double)noise;
			}
		}
		return water;
	}

	public static Hex[,] initializeBiomes(Hex[,] terrain, double[,] moisture, int x, int y){

		int area = (int)x/5;

			for(int i=0; i < x; i++){
				for(int j=0; j < y; j++){
					if(terrain[i,j].type == "shallow_water" || terrain[i,j].type == "deep_water"){
						continue;
					}
					if(i < area || (i >= x-area)){
						//Ice/Tundra

						if(moisture[i,j] > 0.5 || moisture[i,j] + (moisture[i,j]/2.0) > 0.5){
							if(countNeighborsOfType(terrain, i, j, "shallow_water") > 0 && Random.Range(0,100) > 50)
								terrain[i,j].type = "sand";
							else
								terrain[i,j].type = "grass";

						}else
							terrain[i,j].type = "rock";
					}else if((i >= area && i < (area*2)) || (i>=(x-(area*2)) && i < x-area )){
						//Forest / Grassland
						if(moisture[i,j] < 0.5){
							terrain[i,j].type = "plains";
						}else{
							if(Random.Range(0,100) < 30 || (terrain[i,j].coast && Random.Range(0,100) < 50)){
								terrain[i,j].type = "grass";
							}else
								terrain[i,j].type = "forest";
						}
					}else{
						//Jungle/Desert
						if(moisture[i,j] > 0.5 || moisture[i,j] + (moisture[i,j]/6.0) > 0.5 ){
							terrain[i,j].type = "jungle";
						}else{
							terrain[i,j].type = "desert";
						}

					}

				}
			}

		return iterateOverLand(terrain,moisture,x,y);
	}

	public static Hex[,] iterateOverLand(Hex[,] terrain, double[,] moisture, int x, int y){

		for(int i=1; i < x-1; i++){
			for(int j=1; j < y-1; j++){
				if(Random.Range(0,100) > 50){
					string[] types = getNeighborHexTypes(terrain, i, j);
					string sample = types[Random.Range(0,5)];
					int count = 0;
					/*if(!(countNeighborsOfType(terrain,i,j,"shallow_water") + countNeighborsOfType(terrain,i,j,"deep_water") > 6 || countNeighborsOfType(terrain, i, j, "water") > 6)){
						while(sample == "shallow_water" || sample=="deep_water" || sample=="water"){
							sample = types[Random.Range(0,5)];
							count++;
							if(count > 100)
								break;
						}
						terrain[i,j].type = types[Random.Range(0,5)];
					}*/
				}

			}
		}

		return terrain;
	}

	public static string[] getNeighborHexTypes(Hex[,] terrain, int i, int j){
		string[] ret = new string[6];
		ret[0] = terrain[i+1,j].type;
		ret[1] = terrain[i,j+1].type;
		ret[2] = terrain[i+1,j+1].type;
		ret[3] = terrain[i-1,j].type;
		ret[4] = terrain[i,j-1].type;
		ret[5] = terrain[i+1,j-1].type;
		return ret;
	}


	public static Hex[,] iterateOverWater(Hex[,] terrain, int x, int y){
		for(int i=0; i < x; i++){
			for(int j=0; j < y; j++){
				if(terrain[i,j].type != "water"){
					continue;
				}
				if(countNeighborsOfType(terrain, i, j, "land") > 0){
					terrain[i,j].type = "shallow_water";
				}else{
					terrain[i,j].type = "deep_water";
				}

			}
		}
		return terrain;
	}

	public static int countNeighborsOfType(Hex[,] terrain, int i, int j, string t){

		int count = 0;
		try{
		if(terrain[i+1,j].type == t)
			count++;
		if(terrain[i,j+1].type == t)
			count++;
		if(terrain[i+1,j+1].type == t)
			count++;
		if(terrain[i-1,j].type == t)
			count++;
		if(terrain[i,j-1].type == t)
			count++;
		if(terrain[i+1,j-1].type == t)
			count++;
		} catch{

		}
		return count;
	}

	public static Hex[,] fixCoastHeight(ref Hex[,] terrain, int max_x, int max_y){
		for(int i=0; i < max_x; i++){
			for(int j=0; j < max_y; j++){
					if(terrain[i,j].type=="shallow_water" || terrain[i,j].type == "water"){

						setHexToZero(ref terrain, i, j, max_x, max_y);

					}
				
			}
		}
		return terrain;
	}

	public static void setHexToZero( ref Hex[,] terrain, int i, int j, int max_x, int max_y){

		if(i%2==0){
			terrain[i,j].top.y = 0f;
			terrain[i,j].leftTop.y = 0f;
			terrain[i,j].rightTop.y = 0f;
			terrain[i,j].center.y = 0f;
			terrain[i,j].leftBot.y = 0f;	
			terrain[i,j].rightBot.y = 0f;
			terrain[i,j].bottom.y = 0f;

			if(i!=max_x-1){
				terrain[i+1,j].rightTop.y = 0f;
				terrain[i+1,j].rightBot.y = 0f;
			}
			if(i!=0){
				terrain[i-1,j].leftBot.y = 0f;
				terrain[i-1,j].leftTop.y = 0f;
				if(j!=0){
					terrain[i-1,j-1].leftTop.y = 0f;
					terrain[i-1,j-1].top.y = 0f;
				}
				if(j!=max_y-1){
					terrain[i-1,j+1].leftBot.y = 0f;
					terrain[i-1,j+1].bottom.y = 0f;
				}
			}
			if(j!=0){
				terrain[i,j-1].rightTop.y = 0f;
				terrain[i,j-1].top.y = 0f;
			}
			if(j!=max_y-1){
				terrain[i,j+1].bottom.y = 0f;
				terrain[i,j+1].rightBot.y = 0f;
			}


		}else{

			terrain[i,j].top.y = 0f;
			terrain[i,j].leftTop.y = 0f;
			terrain[i,j].rightTop.y = 0f;
			terrain[i,j].center.y = 0f;
			terrain[i,j].leftBot.y = 0f;	
			terrain[i,j].rightBot.y = 0f;
			terrain[i,j].bottom.y = 0f;
			if(i!=max_x-1){
				terrain[i+1,j].rightTop.y = 0f;
				terrain[i+1,j].rightBot.y = 0f;
				if(j!=max_y-1){
					terrain[i+1, j+1].bottom.y = 0f;
					terrain[i+1,j+1].rightBot.y = 0f;
				}
				if(j!=0){
					terrain[i+1,j-1].top.y = 0f;
					terrain[i+1,j-1].rightTop.y = 0f;
				}
			}
			if(i!=0){
				terrain[i-1,j].leftBot.y = 0f;
				terrain[i-1,j].leftTop.y = 0f;
			}
			if(j!=0){
				terrain[i,j-1].leftTop.y = 0f;
				terrain[i,j-1].top.y = 0f;
			}
			if(j!=max_y-1){
				terrain[i,j+1].bottom.y = 0f;
				terrain[i,j+1].leftBot.y = 0f;
			}

		}
			
			

			//return terrain;
	}

	public static Hex[,] initTerrainValues(Hex[,] terrain, int max_x, int max_y){
		for(int i=0; i < max_x; i++){
			for(int j=0; j < max_y; j++){
				terrain[i,j].calcValues();
			}
		}
		return terrain;
	}

	public static double startTerrainValue(Hex[,] terrain, int i, int j, int max_x, int max_y){
		int count = 0;
		float defense = terrain[i,j].defense;
		float food = terrain[i,j].food;
		float industry = terrain[i,j].industry;
		if(i!=max_x-1){
				count++;
				defense += terrain[i+1,j].defense;
				food += terrain[i+1,j].food;
				industry += terrain[i+1,j].industry;
				if(j!=max_y-1){
					count++;
					defense += terrain[i+1, j+1].defense;
					food += terrain[i+1, j+1].food;
					industry += terrain[i+1, j+1].industry;
				}
				if(j!=0){
					count++;
					defense += terrain[i+1,j-1].defense;
					food += terrain[i+1,j-1].food;
					industry += terrain[i+1,j-1].industry;
				}
			}
			if(i!=0){
				count++;
				defense += terrain[i-1,j].defense;
				food += terrain[i-1,j].food;
				industry += terrain[i-1,j].industry;
			}
			if(j!=0){
				count++;
				defense += terrain[i,j-1].defense;
				food += terrain[i,j-1].food;
				industry += terrain[i,j-1].industry;
			}
			if(j!=max_y-1){
				count++;
				defense += terrain[i,j+1].defense;
				food += terrain[i,j+1].food;
				industry += terrain[i,j+1].industry;
			}

			defense = defense / (float)count;
			food = food / (float)count;
			industry = industry / (float)count;

			return (defense + food + industry) / 3f;
	}




}
using UnityEngine;
using System.Collections;

public class RaycastExample : MonoBehaviour {

	public GameObject terrain;
	private PolygonGenerator tScript;
	public GameObject target;
	private LayerMask layerMask = (1 << 0);

	// Use this for initialization
	void Start () {
		tScript=terrain.GetComponent("PolygonGenerator") as PolygonGenerator; 
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		
		float distance=Vector3.Distance(transform.position,target.transform.position);
		
		if( Physics.Raycast(transform.position, (target.transform.position -
		                                         transform.position).normalized, out hit, distance , layerMask)){
			
			Debug.DrawLine(transform.position,hit.point,Color.red);
			
			
		} else {
			Debug.DrawLine(transform.position,target.transform.position,Color.blue);
		} 
	}
}

var speed = 50.0f;
 

var x = 0.0;

function Update()
{
    var movement = Vector3.zero;
    var up = Vector3.zero;
 
    if (Input.GetKey("w"))
        movement.z++;
    if (Input.GetKey("s"))
        movement.z--;
    if (Input.GetKey("a"))
        movement.x++;
    if (Input.GetKey("d"))
        movement.x--;
    if (Input.GetKey("q"))
    	up.z++;
    if (Input.GetKey("e"))
    	up.z--;
    if (Input.GetKey("left"))
    	Camera.main.transform.RotateAround(Camera.main.transform.position, Vector3.up, speed * 4 * Time.deltaTime);
    if (Input.GetKey("up"))
    	Camera.main.transform.RotateAround(Camera.main.transform.position, transform.right, -speed * 4 * Time.deltaTime);
    if (Input.GetKey("right"))
    	Camera.main.transform.RotateAround(Camera.main.transform.position, Vector3.up, -speed * 4 * Time.deltaTime);
    if (Input.GetKey("down"))
    	Camera.main.transform.RotateAround(Camera.main.transform.position, transform.right, speed * 4 * Time.deltaTime);
    	
 
    Camera.main.transform.position = Camera.main.transform.position + (movement * speed *4* Time.deltaTime);
    Camera.main.transform.Translate(up * speed *4* Time.deltaTime);
}
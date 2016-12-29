#pragma strict

private var startpoint:Vector3; // startpoint
private var endpoint:Vector3; // endpoint
private var panning:boolean=false;

private var zoomSpeed:float=15; 

// cam rotate
private var rotating:boolean = false;
private var layerMask = 1 << 8;
private var tmp:Vector3;
private 	var ray:Ray;
private var hit:RaycastHit;
private var camSpeed:int = 20;

// mainloop
function Update ()
{

	// starts here, right mousebutton is pressed down
	if(Input.GetMouseButtonDown(1)) 
	{
		// make ray
		var ray1 : Ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		startpoint = ray1.GetPoint (transform.position.y*2) ; // Returns a point at distance units along the ray
		startpoint.y = 0; // fix z to 0
		panning = true;
	}

	// 
	if(panning) 
	{
		var ray2 : Ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		endpoint = ray2.GetPoint (transform.position.y*2) ; // Returns a point at distance units along the ray
		endpoint.y = 0; // fix z, somehow its not always 0?
		
		// panning
		transform.position+=startpoint-endpoint;
	}
	
	
	if(Input.GetMouseButtonUp(1)) // release button, stop pan
	{
		panning = false;
		
		// reset midpoint
		ray = Camera.main.ViewportPointToRay (Vector3(0.5,0.5,0));
		if (Physics.Raycast (ray, hit, 9999))
		{
			tmp = hit.point;
		}		
	}


	// zoom
	var scrollWheel:float = Input.GetAxis("Mouse ScrollWheel");
	
	if (scrollWheel)
	{
		//transform.position+=Vector3.forward*zoomSpeed*Time.deltaTime;
		transform.Translate((scrollWheel*Vector3.forward) * zoomSpeed, Space.Self);
	}
	
	// rotate cam
	// Rotation A/D
    if (Input.GetKey("a"))
    {
		if (!rotating)
		{
			ray = Camera.main.ViewportPointToRay (Vector3(0.5,0.5,0));
			if (Physics.Raycast (ray, hit, 9999))
			{
				tmp = hit.point;
				rotating = true;
			}
		}
         Camera.main.transform.RotateAround ( tmp, -Vector3.up, 50 * Time.deltaTime);
    }

    if (Input.GetKeyUp("a"))
    {
		rotating = false;
	}
	
    if (Input.GetKeyUp("d"))
    {
		rotating = false;
	}

    if (Input.GetKey("d"))
    {
		if (!rotating)
		{
			ray = Camera.main.ViewportPointToRay (Vector3(0.5,0.5,0));
			if (Physics.Raycast (ray, hit, 9999))
			{
				tmp = hit.point;
				rotating = true;
			}
		}
         Camera.main.transform.RotateAround ( tmp, Vector3.up, 50 * Time.deltaTime);
    }		
	

}
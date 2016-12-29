#pragma strict

// these are read from worldroot
private var size:int = 30;     // number of cubes along a side
private var axisMin:float = 0; // mesh size
private var axisMax:float =  15; //120;
private var multiplier:float = axisMax/size;
public var infoText:GUIText;
private var addStrength:float = 5; //1
private var removeStrength:float = -3; //-1
private var size2:int=size*size;

private var lineRenderer : LineRenderer;
public var worldRoot:Transform;
private var rotateSpeed:int = 50; //50
private var lastHit:Vector3 = new Vector3(0,0,0);
private var border:Vector3 = new Vector3(size-1,size-1,size-1); // where this comes from?


function Start () 
{

	// get world root
	// read global world variables from there
	var worldSetupScript : CreateWorld = worldRoot.gameObject.GetComponent(CreateWorld);
    size = worldSetupScript.size;
	axisMin = worldSetupScript.axisMin;
	axisMax = worldSetupScript.axisMax;
	multiplier = axisMax/size;
//	axisRange = worldSetupScript.size;
	size2 = size*size;
	border = new Vector3(size-1,size-1,size-1);

    lineRenderer = Camera.main.transform.GetComponent(LineRenderer);
}

function Update () 
{

    // add block .. with mouseclick
    if (Input.GetMouseButton(0)) // button is held down
    {

		// TODO: layermask for ray, hit only terrain
		var ray:Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		var hit:RaycastHit;

        if (Physics.Raycast(ray,hit, 9999))
        {
		
			//print (hit.point);
			lastHit = hit.point;
		
			// get object reference
			
			var chunk:Transform = hit.transform;
			
            var localhit:Vector3 = chunk.InverseTransformPoint(lastHit);
            //var localhit:Vector3 = hit.point;
       
            var hx:int = localhit.x/multiplier;
            var hy:int = localhit.y/multiplier; // -0.2 is temporary fix for clicking top plane and getting too big values..
            var hz:int = localhit.z/multiplier;            
			
			
			//print ("Localhit:"+localhit+"   | Hit:"+hx+","+hy+","+hz + "   | Multiplier:"+multiplier);
			infoText.text = "Name:"+chunk.name+"  Hit:"+hx+","+hy+","+hz;
			
			// show laser
            lineRenderer.SetPosition(0, Camera.main.transform.position+Vector3(0,-1,0));
            lineRenderer.SetPosition(1, lastHit);
			
			
		    var editChunk : MarchingCubesJ = chunk.gameObject.GetComponent(MarchingCubesJ);
			var targetChunk:MarchingCubesJ;
			
			var modifyPower:float=0;
			
			if (Input.GetKey(KeyCode.LeftShift)) // shift pressed, remove
			{
				modifyPower = removeStrength;
			}else{ // add
				modifyPower = addStrength;
			}
			
			editChunk.ModifyChunkAdd(hx + size * hy + size2 * hz, modifyPower);
			editChunk.UpdateChunk();

			
			// check diagonals and other neighbours (if we need to affect them)
			if (hx == border.x && hz==border.z) // modify to forward right
			{
				if (editChunk.neighbours[1]!=null)
				{
					targetChunk = editChunk.neighbours[1].gameObject.GetComponent(MarchingCubesJ);
					targetChunk.ModifyChunkAdd(0 + size * hy + size2 * 0, modifyPower);
					targetChunk.UpdateChunk();
				}
			}
			
			if (hx == border.x && hz==0) // modify to back right
			{
				if (editChunk.neighbours[3]!=null)
				{
					targetChunk = editChunk.neighbours[3].gameObject.GetComponent(MarchingCubesJ);
					targetChunk.ModifyChunkAdd(0 + size * hy + size2 * border.z, modifyPower);
					targetChunk.UpdateChunk();
				}
			}
			
			if (hx == 0 && hz==0) // modify to back left 
			{
				if (editChunk.neighbours[5]!=null)
				{
					targetChunk = editChunk.neighbours[5].gameObject.GetComponent(MarchingCubesJ);
					targetChunk.ModifyChunkAdd(border.x + size * hy + size2 * border.z, modifyPower);
					targetChunk.UpdateChunk();
				}
			}
			
			if (hx == 0 && hz==border.z) // modify to forward left 
			{
				if (editChunk.neighbours[7]!=null)
				{
					targetChunk = editChunk.neighbours[7].gameObject.GetComponent(MarchingCubesJ);
					targetChunk.ModifyChunkAdd(border.x + size * hy + size2 * 0, modifyPower);
					
					targetChunk.UpdateChunk();
				}
			}
			
			
			
			
			
			if (hx == border.x) // modify to RIGHT
			{
				if (editChunk.neighbours[2]!=null)
				{
					targetChunk = editChunk.neighbours[2].gameObject.GetComponent(MarchingCubesJ);
					targetChunk.ModifyChunkAdd(0 + size * hy + size2 * hz, modifyPower);
					targetChunk.UpdateChunk();
				}
			}

			
			if (hx==0) // modify to LEFT
			{
				if (editChunk.neighbours[6]!=null)
				{
					targetChunk = editChunk.neighbours[6].gameObject.GetComponent(MarchingCubesJ);
					targetChunk.ModifyChunkAdd(border.x + size * hy + size2 * hz, modifyPower);
					targetChunk.UpdateChunk();
				}
			}
			
			if (hz==border.z) // modify to forward
			{
				if (editChunk.neighbours[0]!=null)
				{
					targetChunk = editChunk.neighbours[0].gameObject.GetComponent(MarchingCubesJ);
					targetChunk.ModifyChunkAdd(hx + size * hy + size2 * 0, modifyPower);
					targetChunk.UpdateChunk();
				}
				
			}
					
					
			if (hz==0) // modify to backward
			{
				if (editChunk.neighbours[4]!=null)
				{
					targetChunk = editChunk.neighbours[4].gameObject.GetComponent(MarchingCubesJ);
					targetChunk.ModifyChunkAdd(hx + size * hy + size2 * border.z, modifyPower);
					targetChunk.UpdateChunk();
				}
			}
			
        }
    }


    if (Input.GetMouseButtonUp(0)) // button released
    {
        lineRenderer.SetPosition(0, Camera.main.transform.position);
        lineRenderer.SetPosition(1, Camera.main.transform.position);
    }
	
    if (Input.GetKey("r")) // reset scene
    {
		Application.LoadLevel(0);
    }

}
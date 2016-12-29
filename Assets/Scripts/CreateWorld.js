#pragma strict

public var chunk:Transform;

// BUG: Most values wont work too well.. also you might have to adjust shader values to match water height
public var size:int = 16;     // number of cubes along a side = resolution (cannot put too high, 65k vertex limit per mesh)
public var axisMin:float = 0; // chunk pivot 0,0,0 (dont touch)
public var axisMax:float =  8;  // chunk width in world units
private var multiplier:float = axisMax/size; // internal

// How Many Chunks (dont try too many.. they are all created at start)
public var sizeX:int=8;
private var sizeY:int=8; // not used yet
public var sizeZ:int=8;

// instantiate all chunks
function Awake () 
{
	var y:int=0;
	var counter:int=0;
	for (var z:int=0;z<sizeZ;z++)
	{
		for (var x:int=0;x<sizeX;x++)
		{
			var clone : Transform;
			clone = Instantiate(chunk, transform.position+Vector3(x,0,z)*axisMax, transform.rotation);
			clone.parent = transform;
			clone.name = "zChunk_"+clone.position.x+"_"+clone.position.y+"_"+clone.position.z;
			
			// set offset
			 var other : MarchingCubesJ = clone.gameObject.GetComponent(MarchingCubesJ);
			 other.offset = Vector3(x*(size-1),y*(size-1),z*(size-1));
			counter++;
		}
	}

	// get list of our neighbours
	for (var child:Transform in transform) // Downcast notice..uh..
	{
		var childChunk : MarchingCubesJ = child.gameObject.GetComponent(MarchingCubesJ);
		
		
		var currentPos:Vector3 = child.position; ///multiplier;
		// scan forward
		var searchName:String = "zChunk_"+currentPos.x+"_"+currentPos.y+"_"+(currentPos.z+axisMax);
		var neighbour:GameObject = GameObject.Find(searchName);
		if (neighbour != null)
		{
			childChunk.neighbours[0] = neighbour.transform;
		}

		// scan forward-right
		searchName = "zChunk_"+(currentPos.x+axisMax)+"_"+currentPos.y+"_"+(currentPos.z+axisMax);
		neighbour = GameObject.Find(searchName);
		if (neighbour != null)
		{
			childChunk.neighbours[1] = neighbour.transform;
		}

		// scan right
		searchName = "zChunk_"+(currentPos.x+axisMax)+"_"+currentPos.y+"_"+currentPos.z;
		neighbour = GameObject.Find(searchName);
		if (neighbour != null)
		{
			childChunk.neighbours[2] = neighbour.transform;
		}
		
		// scan backward-right
		searchName = "zChunk_"+(currentPos.x+axisMax)+"_"+currentPos.y+"_"+(currentPos.z-axisMax);
		neighbour = GameObject.Find(searchName);
		if (neighbour != null)
		{
			childChunk.neighbours[3] = neighbour.transform;
		}
		
		// scan backward
		searchName = "zChunk_"+currentPos.x+"_"+currentPos.y+"_"+(currentPos.z-axisMax);
		neighbour = GameObject.Find(searchName);
		if (neighbour != null)
		{
			childChunk.neighbours[4] = neighbour.transform;
		}
		
		// scan backward-left
		searchName = "zChunk_"+(currentPos.x-axisMax)+"_"+currentPos.y+"_"+(currentPos.z-axisMax);
		neighbour = GameObject.Find(searchName);
		if (neighbour != null)
		{
			childChunk.neighbours[5] = neighbour.transform;
		}
		
		// scan left
		searchName = "zChunk_"+(currentPos.x-axisMax)+"_"+currentPos.y+"_"+currentPos.z;
		neighbour = GameObject.Find(searchName);
		if (neighbour != null)
		{
			childChunk.neighbours[6] = neighbour.transform;
		}
		
		// scan forward-left
		searchName = "zChunk_"+(currentPos.x-axisMax)+"_"+currentPos.y+"_"+(currentPos.z+axisMax);
		neighbour = GameObject.Find(searchName);
		if (neighbour != null)
		{
			childChunk.neighbours[7] = neighbour.transform;
		}

	}
}

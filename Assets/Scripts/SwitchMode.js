#pragma strict

public var fpsPlayer:GameObject;
public var fpsPlayerCam:Camera;
public var mainCam:Camera;

private var fpsmode:boolean=false;

function Update () 
{

	// swap cam
	if (Input.GetKeyDown(KeyCode.Tab))
	{
		fpsmode = !fpsmode;
	
		if (fpsmode)
		{
			fpsPlayer.active = true;
			fpsPlayerCam.enabled = true;
			fpsPlayerCam.gameObject.active = true;
			fpsPlayer.GetComponent(CharacterController).enabled  = true;
			mainCam.gameObject.GetComponent(TerrainEditor3).enabled  = false;
			mainCam.enabled = false;
			
		}else{
		
			fpsPlayer.active = false;
			fpsPlayerCam.enabled = false;
			fpsPlayerCam.gameObject.active = false;
			fpsPlayer.gameObject.GetComponent(CharacterController).enabled  = false;
			mainCam.enabled = true;
			mainCam.gameObject.GetComponent(TerrainEditor3).enabled  = true;
		}
		
	}

}
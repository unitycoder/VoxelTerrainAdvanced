using UnityEngine;

public class SwitchModeC : MonoBehaviour
{
    public GameObject fpsPlayer;
    public Camera fpsPlayerCam;
    public Camera mainCam;

    private bool fpsmode = false;

    void Update()
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
                fpsPlayer.GetComponent<CharacterController>().enabled = true;
                mainCam.gameObject.GetComponent<TerrainEditor3C>().enabled = false;
                mainCam.enabled = false;

            }
            else
            {

                fpsPlayer.active = false;
                fpsPlayerCam.enabled = false;
                fpsPlayerCam.gameObject.active = false;
                fpsPlayer.gameObject.GetComponent<CharacterController>().enabled = false;
                mainCam.enabled = true;
                mainCam.gameObject.GetComponent<TerrainEditor3C>().enabled = true;
            }

        }

    }
}
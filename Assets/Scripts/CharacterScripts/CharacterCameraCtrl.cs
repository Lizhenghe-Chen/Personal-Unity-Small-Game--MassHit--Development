using UnityEngine;
public partial class CharacterCtrl
{ 
      private void ChangeCamera()//cam1 is CinemachineFreeLook, cam2 is CinemachineTransposer
    {
        if (Input.GetKeyDown(GlobalRules.instance.SwitchCamera))
        {
            Debug.Log("Switch Camera");
            if (Player_Camera1.activeSelf == true)//cam1 to cam2
            {
                Player_Camera2.SetActive(true);
                //  Camera = Player_Camera2.transform.parent.Find("Main Camera").GetComponent<Transform>();
                //GlobalRules.instance.FitCameraDirection(true);
                Player_Camera1.SetActive(false);

            }
            else//cam2 to cam1
            {
                Player_Camera1.SetActive(true);
                //Camera = Player_Camera2.transform.parent.Find("Main Camera").GetComponent<Transform>();
                //GlobalRules.instance.FitCameraDirection(false);
                Player_Camera2.SetActive(false);
            }
        }
    } 
     public void SetPlayerCam1(int isActive)//0 is false,1 is true
    {
        if (isActive == 0)
        {
            Player_Camera1.SetActive(false);
        }
        else
        {
            Player_Camera1.SetActive(true);
        }
    }
}
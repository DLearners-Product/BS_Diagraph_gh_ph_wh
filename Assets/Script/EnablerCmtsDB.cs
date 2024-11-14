using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class EnablerCmtsDB
{
    public string welcome;
    public string recall;
    public string brain_gym_hook_ups;
    public string anchor_chart;
    public string flower_activity;
    public string sorting_activity;
    public string brain_gym_ocean_breathing;
    public string passsage;
    public string listen_and_click;
    public string goodbye;

    public EnablerCmtsDB()
    {
        welcome = Main_Blended.OBJ_main_blended.enablerComments[0];
        recall = Main_Blended.OBJ_main_blended.enablerComments[1];
        brain_gym_hook_ups = Main_Blended.OBJ_main_blended.enablerComments[2];
        anchor_chart = Main_Blended.OBJ_main_blended.enablerComments[3];
        flower_activity = Main_Blended.OBJ_main_blended.enablerComments[4];
        sorting_activity = Main_Blended.OBJ_main_blended.enablerComments[5];
        brain_gym_ocean_breathing = Main_Blended.OBJ_main_blended.enablerComments[6];
        passsage = Main_Blended.OBJ_main_blended.enablerComments[7];
        listen_and_click = Main_Blended.OBJ_main_blended.enablerComments[8];
        goodbye = Main_Blended.OBJ_main_blended.enablerComments[9];
    }
}
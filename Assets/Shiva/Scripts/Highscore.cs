using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ2019.Akihabara.Team5
{
    public class Highscore
    {
        public static int highscore;
        public static string name;

        public static void LoadHighScore()
        {
            name = PlayerPrefs.GetString("HighScore_Name");
            highscore = PlayerPrefs.GetInt("HighScore_Value");
        }

        public static void SaveHighScore(string n, int a)
        {
            LoadHighScore();

            if (a > highscore)
            {
                PlayerPrefs.SetString("HighScore_Name", n);
                PlayerPrefs.SetInt("HighScore_Value", a);
            }
        }

    }
}
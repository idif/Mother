using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour 
{
        public bool Fullscreen;
        
        /*public int ResX = 640;
        public int ResY = 480;*/

        int valueAA = 0;

        public Slider AABar;
        public Slider hertz;
        //public Slider resolutionV;
        public Toggle fullS;
        public Toggle TriB;
        public Toggle Vsync;

        public Text qualityText;
        public Text aAText;
        public Text fps;
        public Text reso;

        public GameObject GraphicMenu;
        public GameObject OptMenu;


        void Start()
        {
                ChangeQualityUp();
                aAText.text = QualitySettings.antiAliasing.ToString();
                fps.text = "30 FPS";
                /*reso.text = Screen.width + " x " + Screen.height;
                ResX = Screen.width;
                ResY = Screen.height;*/
                valueAA = (int)AABar.value;
        }

        public void ChangeQualityUp()
        {
                QualitySettings.IncreaseLevel();
                
                switch(QualitySettings.GetQualityLevel())
                {
                        case 0 :
                        qualityText.text = "low";
                        break;
                        case 1 :
                        qualityText.text = "medium";
                        break;
                        case 2 :
                        qualityText.text = "high";
                        break;
                }
        }

        public void ChangeQualityDown()
        {
                QualitySettings.DecreaseLevel();

                switch(QualitySettings.GetQualityLevel())
                {
                        case 0 :
                        qualityText.text = "low";
                        break;
                        case 1 :
                        qualityText.text = "medium";
                        break;
                        case 2 :
                        qualityText.text = "high";
                        break;
                }
        }

        public void TripleBuffering()
        {
                if(TriB.isOn)
                {
                        QualitySettings.maxQueuedFrames = 3;
                }
                else
                {
                        QualitySettings.maxQueuedFrames = 0;
                }
        }

        public void VSynchro()
        {
                if(Vsync)
                {
                        QualitySettings.vSyncCount = 1;
                }
                else
                {
                        QualitySettings.vSyncCount = 0;
                }  
        }

        public void AAliasing()
        {

                switch((int)AABar.value)
                {
                        case 0 :
                        QualitySettings.antiAliasing = 0;
                        valueAA = 0;
                        aAText.text = valueAA.ToString();
                        break;
                        case 1 :
                        QualitySettings.antiAliasing = 2;
                        valueAA = 2;
                        aAText.text = valueAA.ToString();
                        break;
                        case 2 : 
                        QualitySettings.antiAliasing = 4;
                        valueAA = 4;
                        aAText.text = valueAA.ToString();
                        break;
                        case 3 :
                        QualitySettings.antiAliasing = 8;
                        valueAA = 8;
                        aAText.text = valueAA.ToString();
                        break;
                        default :
                        break;
                }
        }

       /* public void FullS()
        {
                if(fullS.isOn)
                {
                         Screen.fullScreen = true;
                         Fullscreen = true;
                }
                else
                {
                         Screen.fullScreen = false;
                         Fullscreen = false;
                }
        }*/

        public void FPS()
        {
                switch((int)hertz.value)
                {
                        case 0 : 
                                Screen.SetResolution(Screen.width, Screen.height, Fullscreen, 30);
                                fps.text = "30 FPS";
                        break;
                        case 1 : 
                                Screen.SetResolution(Screen.width, Screen.height, Fullscreen, 60);
                                fps.text = "60 FPS";
                        break;
                        case 2 :
                                Screen.SetResolution(Screen.width, Screen.height, Fullscreen, 120);
                                fps.text = "120 FPS";
                        break;
                        default :
                        break;
                }
        }

        /*public void Reso()
        {
                switch((int)resolutionV.value)
                {
                        case 0 :
                               ResX = 1280;
                               ResY = 720;
                        break;
                        case 1 :
                               ResX = 1600;
                               ResY = 900;
                        break;
                        case 2 :
                               ResX = 1920;
                               ResY = 1080;
                        break;

                        default :
                        break;
                }
                Screen.SetResolution(ResX, ResY, Fullscreen);
                reso.text = Screen.width + " x " + Screen.height;
        }*/

        public void ReturnButton()
        {
                GraphicMenu.SetActive(false);
                OptMenu.SetActive(true);
        }
}
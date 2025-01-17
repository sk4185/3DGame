﻿// UI Pack : Toony LITE
// Version: 1.2.0
// Compatilble: Unity 5.5.1 or higher, see more info in Readme.txt file.
//
// Developer:			Gold Experience Team (https://www.assetstore.unity3d.com/en/#!/search/page=1/sortby=popularity/query=publisher:4162)
//
// Unity Asset Store:	https://www.assetstore.unity3d.com/en/#!/content/38836
//
// Please direct any bugs/comments/suggestions to geteamdev@gmail.com

#region Namespaces

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;  // Add since Unity 5.3, see http://docs.unity3d.com/Manual/UpgradeGuide53.html and http://docs.unity3d.com/530/Documentation/ScriptReference/SceneManagement.SceneManager.html

using UnityEngine.UI;

#endregion // Namespaces

// ######################################################################
// UIPT_LITE_Demo09_Credits class
//
// Handles "Demo 09 - Landscape - Credits" and "Demo 09 - Portrait - Credits" scenes.
// ######################################################################

public class UIPT_LITE_Demo09_Credits : MonoBehaviour
{

	// ########################################
	// MonoBehaviour Functions
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.html
	// ########################################

	#region MonoBehaviour

	// Awake is called when the script instance is being loaded.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
	void Awake()
	{
		// Set GUIAnimSystemFREE.Instance.m_AutoAnimation to false, 
		// this will let you control all GUI Animator elements in the scene via scripts.
		if (enabled)
		{
			GUIAnimSystemFREE.Instance.m_GUISpeed = 4.0f;
			GUIAnimSystemFREE.Instance.m_AutoAnimation = false;
		}

		// If this class is not running on Unity Editor, the resolution will be change to 960x600px for Lanscape demo scene or 540x960px for Portrait demo scene
		if (Application.isEditor == false)
		{
			if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer)
			{
				// Load next scene according to orientation of current scene.
				string CurrentLevel = SceneManager.GetActiveScene().name; // No longer use Application.loadedLevelName in Unity 5.3 or higher.
				if (CurrentLevel.Contains("Landscape") == true)
					Screen.SetResolution(960, 600, false);
				else
					Screen.SetResolution(540, 960, false);
			}
		}

	}

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
	void Start()
	{
		// Stop particles in the hierarchy of given transfrom
		GUIAnimSystemFREE.Instance.StopParticle(this.transform);

		// Show Credits Panel
		StartCoroutine(ShowCredits());
	}

	// Update is called once per frame.
	void Update()
	{
	}

	#endregion // MonoBehaviour

	// ########################################
	// UI Responder functions
	// ########################################

	#region UI Responder

	public void Button_Home()
	{
		// Play Back button sound
		UIPT_LITE_SoundController.Instance.Play_SoundBack();

		// Keep particles stay alive until it finished playing.
		GUIAnimSystemFREE.Instance.DontDestroyParticleWhenLoadNewScene(this.transform, true);

		// Play Move Out animation
		GUIAnimSystemFREE.Instance.MoveOut(this.transform, true);

		// Load next scene according to orientation of current scene.
		string CurrentLevel = SceneManager.GetActiveScene().name; // No longer use Application.loadedLevelName in Unity 5.3 or higher.
		string OrientationName = "Portrait";
		if (CurrentLevel.Contains("Landscape") == true)
			OrientationName = "Landscape";
		GUIAnimSystemFREE.Instance.LoadLevel("ToonyLITE Demo 02 - " + OrientationName + " - Home", 1.0f);
	}

	#endregion // UI Responder

	// ########################################
	// Functions
	// ########################################

	#region Functions

	IEnumerator ShowCredits()
	{
		// Creates a yield instruction to wait for a given number of seconds
		// http://docs.unity3d.com/400/Documentation/ScriptReference/WaitForSeconds.WaitForSeconds.html
		yield return new WaitForSeconds(0.5f);

		// Play particles in the hierarchy of given transfrom
		GUIAnimSystemFREE.Instance.PlayParticle(this.transform);

		// Play MoveIn animation
		GUIAnimSystemFREE.Instance.MoveIn(this.transform, true);
	}

	#endregion // Functions
}

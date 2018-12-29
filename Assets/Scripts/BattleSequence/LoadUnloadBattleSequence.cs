using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LoadUnloadBattleSequence
{
	public static event EventHandler<BattleWonArgs> BattleSequenceLoadComplete;
	public static event EventHandler<BattleWonArgs> BattleSequenceUnloadComplete;
	private static BattleWonArgs battleWonArgs = new BattleWonArgs(false);

	public static void LoadBattleSequence()
	{
		SceneManager.LoadSceneAsync("BattleSequence", LoadSceneMode.Additive);
		SceneManager.sceneLoaded += BattleSequenceLoaded;
	}

	public static void UnloadBattleSequence(bool win)
	{
		SceneManager.UnloadSceneAsync("BattleSequence");
		battleWonArgs.Win = win;
		SceneManager.sceneUnloaded += BattleSequenceUnloaded;
	}

	static void BattleSequenceUnloaded(Scene scene)
	{
		if(scene.name == "BattleSequence")
		{
			SceneManager.sceneUnloaded -= BattleSequenceUnloaded;
			PostBattleSequenceUnloadComplete();
		}
	}

	private static void BattleSequenceLoaded(Scene scene, LoadSceneMode loadScene)
	{
		if(scene.name == "BattleSequence")
		{
			SceneManager.sceneLoaded -= BattleSequenceLoaded;
			PostBattleSequenceLoadComplete();
		}
	}

	private static void PostBattleSequenceLoadComplete()
	{
		var handler = BattleSequenceLoadComplete;
		if(handler != null)
		{
			handler(null, null);
		}
	}

	private static void PostBattleSequenceUnloadComplete()
	{
		var handler = BattleSequenceUnloadComplete;
		if(handler != null)
		{
			handler(null, battleWonArgs);
		}
	}
}

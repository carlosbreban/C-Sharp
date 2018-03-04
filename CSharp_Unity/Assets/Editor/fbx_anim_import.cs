// UNITY3D: FBX Anim Importer
// By Carlos Breban
// Generates Animation clips for FBX based on supplied .csv file (filename must match fbx)
// Also allows you to reset the clips to default, single "Take 1" clip
// NOTE: The formatting of the csv file is defined by the cb_batch_render_tools.ms script for 3dsMax included in "Samples" 
// Usage (animation import): 
// 	1. Select fbx asset(s)
//	2. Go to CB_TOOLS>Import/Import FBX Animations
//
// Usage (animation reset): 
// 	1. Select fbx asset(s)
//	2. Go to CB_TOOLS>Import/Reset FBX Animations

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditorInternal;

public class fbx_anim_import : MonoBehaviour{
	public static void SplitFbxClips(Object[] incObjects){
		AssetDatabase.StartAssetEditing();
		foreach (Object current in incObjects){
			string fullAssetPath = AssetDatabase.GetAssetPath(current);
			string justPath = fullAssetPath.Substring(0, fullAssetPath.LastIndexOf('/'));
			string dopeSheetFile = (justPath + '/' + current.name + ".csv");
			string[] allEntries = System.IO.File.ReadAllLines(@dopeSheetFile);				

			ModelImporter modImp = AssetImporter.GetAtPath(fullAssetPath) as ModelImporter;

			ModelImporterClipAnimation[] animClips = new ModelImporterClipAnimation[allEntries.Length];		
	
			string[] curEntry;
			char splitChar = ',';
			int startFrame;
			int endFrame;
			
			for(int i = 0; i < allEntries.Length; i++){	
				curEntry = new string[12];
				curEntry = allEntries[i].Split(splitChar);
				startFrame = int.Parse(curEntry[2].Substring(0, curEntry[2].Length - 1));
				endFrame = int.Parse(curEntry[3].Substring(0, curEntry[3].Length - 1));
				
				ModelImporterClipAnimation curAnimClip = new ModelImporterClipAnimation();
				
				curAnimClip.name = curEntry[7];
				curAnimClip.firstFrame = startFrame;
				curAnimClip.lastFrame = endFrame;
				
				if(curEntry[0] == "true") {
					curAnimClip.loop = true;
					curAnimClip.wrapMode = WrapMode.Loop;
				}		
				else{
					curAnimClip.loop = false;
					curAnimClip.wrapMode = WrapMode.Once;
				}
					
				animClips[i] = curAnimClip;					
			}
			
			modImp.clipAnimations = animClips;
			AssetDatabase.ImportAsset(fullAssetPath,ImportAssetOptions.ForceUpdate);			
		}

		AssetDatabase.StopAssetEditing();
		AssetDatabase.Refresh();
	}
	
	
	public static void ResetFbxClips(Object[] incObjects){
		AssetDatabase.StartAssetEditing();
		foreach (Object current in incObjects){
			string fullAssetPath = AssetDatabase.GetAssetPath(current);

			ModelImporter modImp = AssetImporter.GetAtPath(fullAssetPath) as ModelImporter;

			modImp.clipAnimations = modImp.defaultClipAnimations;
			AssetDatabase.ImportAsset(fullAssetPath,ImportAssetOptions.ForceUpdate);			
		}

		AssetDatabase.StopAssetEditing();
		AssetDatabase.Refresh();
	}
	
	
	
	
	[MenuItem ("CB_TOOLS/Import/Import FBX Animations",false,1)]
	[MenuItem ("Assets/CB_TOOLS/Import/Import FBX Animations",false,1)]
	[MenuItem ("GameObject/CB_TOOLS/Import/Import FBX Animations",false,1)]	
	public static void 	MenuImportFbxAnims(){
		Object[] selectedObjects = Selection.objects;
		if(selectedObjects.Length>0)
			SplitFbxClips(selectedObjects); 
	}	

	
	[MenuItem ("CB_TOOLS/Import/Reset FBX Animations",false,1)]
	[MenuItem ("Assets/CB_TOOLS/Import/Reset FBX Animations",false,1)]
	[MenuItem ("GameObject/CB_TOOLS/Import/Reset FBX Animations",false,1)]	
	public static void 	MenuResetFbxAnims(){
		Object[] selectedObjects = Selection.objects;
		if(selectedObjects.Length>0)
			ResetFbxClips(selectedObjects); 
	}	
	
}
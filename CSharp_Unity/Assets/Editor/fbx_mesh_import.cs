// UNITY3D: FBX Mesh Importer
// By Carlos Breban
// Changes common import settings for FBX legacy animated characters
// Usage: 
// 	1. Select fbx asset(s)
//	2. Go to CB_TOOLS>Import/Import FBX Mesh

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class fbx_mesh_import : MonoBehaviour {
	public static void ImportFbx(Object[] incObjects){
		AssetDatabase.StartAssetEditing();	
		foreach (Object current in incObjects){
			string path = AssetDatabase.GetAssetPath(current); 	
			ModelImporter modImp = AssetImporter.GetAtPath(path) as ModelImporter;
			modImp.materialName = ModelImporterMaterialName.BasedOnMaterialName;		
			modImp.animationType = ModelImporterAnimationType.Legacy;
			modImp.importAnimation = true;
			//modImp.materialLocation = ModelImporterMaterialLocation.External;
			EditorUtility.SetDirty(current);
			AssetDatabase.ImportAsset(path,ImportAssetOptions.ForceUpdate);		
		}
		AssetDatabase.StopAssetEditing();
		AssetDatabase.Refresh();
	}		
				
					
	[MenuItem ("CB_TOOLS/Import/Import FBX Mesh",false,1)]
	[MenuItem ("Assets/CB_TOOLS/Import/Import FBX Mesh",false,1)]
	[MenuItem ("GameObject/CB_TOOLS/Import/Import FBX Mesh",false,1)]
	public static void 	MenuImportFbx(){
		Object[] selectedObjects = Selection.objects;
		if(selectedObjects.Length>0)
			ImportFbx(selectedObjects); 
	}		
}

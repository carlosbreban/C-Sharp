// UNITY3D: FBX Mesh Importer - Post Processor (Automatic on refresh/reimport)
// By Carlos Breban
// Changes common import settings for FBX legacy animated characters
// Usage: 
// 	New assets are affected automatically.
//  RMB>Refresh or RMB>Reimport

using UnityEditor;
    
public class fbx_mesh_import_post : AssetPostprocessor{
	public void OnPreprocessModel(){
		ModelImporter modImp = (ModelImporter) assetImporter;	
		modImp.materialName = ModelImporterMaterialName.BasedOnMaterialName;
		//modImp.materialLocation = ModelImporterMaterialLocation.External;	
		modImp.animationType = ModelImporterAnimationType.Legacy;
		modImp.importAnimation = true;
	}
}

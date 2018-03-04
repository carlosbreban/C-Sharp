// UNITY3D: Asset Renamer Dialog (Inspired by the Rename Tool in 3dsMax)
// By Carlos Breban
// Usage: 
// 	1. Select gameObjects or assets
//	2. Open dialog through Window>CB Asset Renamer
//  3. Modify dialog settings and press "Rename" button


using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class asset_renamer : EditorWindow {
	[MenuItem ("Window/CB Asset Renamer")]
	static void Init() {
		EditorWindow window = GetWindow(typeof(asset_renamer),true, "Asset Renamer");
		window.minSize = new Vector2(280,190);
		window.maxSize = window.minSize;
		window.Show();
	}
			
	bool base_state_a = false;	
	bool base_state_b = false;	
	bool base_state_c = false;	
	bool base_state_d = false;
	bool base_state_e = false;
	bool base_state_f = false;
	private string baseName_tf = ""; 
	private string pref_tf = "";    
	private int remfirst_if = 0;    
	private string suf_tf = "";  
	private int remlast_if = 0; 
	private int start_if = 0; 
	private int step_if = 1; 
		
	void OnGUI() {
		//BASE NAME			
		EditorGUILayout.BeginHorizontal (GUIStyle.none,GUILayout.Width(30));
		base_state_a = EditorGUILayout.Toggle( "", base_state_a, GUILayout.Width(30));
		GUILayout.Label("Base Name",GUILayout.Width(80));
		baseName_tf = GUILayout.TextField(baseName_tf,25,GUILayout.Width(150));
		EditorGUILayout.EndHorizontal ();

		//PREFIX			
		EditorGUILayout.BeginHorizontal (GUIStyle.none,GUILayout.Width(30));
		base_state_b = EditorGUILayout.Toggle( "", base_state_b, GUILayout.Width(30));
		GUILayout.Label("Prefix",GUILayout.Width(80));
		pref_tf = GUILayout.TextField(pref_tf,25,GUILayout.Width(80));
		EditorGUILayout.EndHorizontal ();
		
		//REMOVE FIRST
		EditorGUILayout.BeginHorizontal (GUIStyle.none,GUILayout.Width(30));
		base_state_c = EditorGUILayout.Toggle( "", base_state_c, GUILayout.Width(30));
		GUILayout.Label("Remove First",GUILayout.Width(80));
		remfirst_if = EditorGUILayout.IntField("",remfirst_if, GUILayout.Width(30));
		GUILayout.Label("Digits",GUILayout.Width(80));
		EditorGUILayout.EndHorizontal ();
		
		//SUFFIX			
		EditorGUILayout.BeginHorizontal (GUIStyle.none,GUILayout.Width(30));
		base_state_d = EditorGUILayout.Toggle( "", base_state_d, GUILayout.Width(30));
		GUILayout.Label("Suffix",GUILayout.Width(80));
		suf_tf = GUILayout.TextField(suf_tf,25,GUILayout.Width(80));
		EditorGUILayout.EndHorizontal ();		

		//REMOVE LAST
		EditorGUILayout.BeginHorizontal (GUIStyle.none,GUILayout.Width(30));
		base_state_e = EditorGUILayout.Toggle( "", base_state_e, GUILayout.Width(30));
		GUILayout.Label("Remove Last",GUILayout.Width(80));
		remlast_if = EditorGUILayout.IntField("",remlast_if, GUILayout.Width(30));
		GUILayout.Label("Digits",GUILayout.Width(80));
		EditorGUILayout.EndHorizontal ();

		//NUMBERED		
		EditorGUILayout.BeginHorizontal (GUIStyle.none,GUILayout.Width(30));
		base_state_f = EditorGUILayout.Toggle( "", base_state_f, GUILayout.Width(30));
		GUILayout.Label("Numbered",GUILayout.Width(80));		
		EditorGUILayout.EndHorizontal ();
		
		EditorGUILayout.BeginHorizontal (GUIStyle.none,GUILayout.Width(30));
		GUILayout.Label("         Start",GUILayout.Width(80));	
		start_if = EditorGUILayout.IntField("",start_if, GUILayout.Width(30));
		EditorGUILayout.EndHorizontal ();
		
		EditorGUILayout.BeginHorizontal (GUIStyle.none,GUILayout.Width(30));
		GUILayout.Label("         Step",GUILayout.Width(80));	
		step_if = EditorGUILayout.IntField("",step_if, GUILayout.Width(30));
		EditorGUILayout.EndHorizontal ();		
		
		
		//RENAME BUTTON
		if(GUILayout.Button("Rename")){
			List<string> renameArgs = new List<string>();	
			if(base_state_a) renameArgs.Add(baseName_tf);	
			else renameArgs.Add("");
			
			if(base_state_b) renameArgs.Add(pref_tf);	
			else renameArgs.Add("");
			
			if(base_state_c) renameArgs.Add(remfirst_if.ToString());	
			else renameArgs.Add("");
			
			if(base_state_d) renameArgs.Add(suf_tf);	
			else renameArgs.Add("");
			
			if(base_state_e) renameArgs.Add(remlast_if.ToString());	
			else renameArgs.Add("");
			
			if(base_state_f) {renameArgs.Add(start_if.ToString()); renameArgs.Add(step_if.ToString());}
			else {renameArgs.Add(""); renameArgs.Add("");}
	
			RenamerTool(renameArgs); 
			}	
			
		this.Repaint();
	}
			
			
	static void	RenamerTool(List<string> incArgs){
		string finalName = "";		
		string newName = "";
		if(incArgs[0] != "")
			newName = incArgs[1] + incArgs[0] + incArgs[3];
	
		Object[] selectedObjects = Selection.objects;
		Selection.objects = new Object[0];
		
		if(selectedObjects.Length>0){			
			int startNum = 0;
			int stepNum = 0;				

			if(incArgs[5] != ""){	
				startNum = int.Parse(incArgs[5]);
				stepNum = int.Parse(incArgs[6]);	
			}
		
			for(int i = 0; i < selectedObjects.Length; i++)	{			
				string baseName = "";				
				string fullAssetPath = AssetDatabase.GetAssetPath(selectedObjects[i]);
				
				if(incArgs[0] == ""){					
					baseName = selectedObjects[i].name;
					
					if(incArgs[2]!="" && baseName.Length > int.Parse(incArgs[2]))
						baseName = baseName.Substring(int.Parse(incArgs[2]),baseName.Length - int.Parse(incArgs[2]));
					
					if(incArgs[4]!="" && baseName.Length > int.Parse(incArgs[4]))
						baseName = baseName.Substring(0,baseName.Length - int.Parse(incArgs[4]));
				
					if(incArgs[1]!="") 
						baseName = incArgs[1] + baseName;

					if(incArgs[3]!="") 
						baseName = baseName + incArgs[3];
				
					newName = baseName;
				}	
					
				string numberedName = "";
		
				if(incArgs[5]!= ""){				
					if(i == 0) {
						numberedName = (newName + incArgs[5]);
					}
					else {
						startNum = startNum + stepNum;	
						numberedName = (newName + (startNum.ToString()));					
					}
				}
			
				if(numberedName != "") finalName = numberedName;
				else finalName = newName;
			
				if(fullAssetPath != "") 
					AssetDatabase.RenameAsset(fullAssetPath, finalName); 
				else
					selectedObjects[i].name = finalName;
			}
			
			Selection.objects = selectedObjects;
			AssetDatabase.Refresh();			
		}
	}	
}
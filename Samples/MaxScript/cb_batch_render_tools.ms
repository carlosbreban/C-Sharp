-- 3dsMax: Batch Render Tools Dialog 
-- By Carlos Breban
-- Use to save and load batch render data in scene. Can be saved out as custom .csv or as standalone maxscript  
-- Also useful for clearing the batch render and finding exported files in windows explorer

FN BatchQuery = (
	local curBatchRender = batchRenderMgr 
	local curBREntryCount = curBatchRender.numViews 
	local curEntryGet = #()
	local batchBank = #()

	for i = 1 to curBREntryCount do(	
		curEntry = curBatchRender.GetView i
		curEntryGet[1] = curEntry.enabled
		curEntryGet[2] = curEntry.overridePreset 
		curEntryGet[3] = curEntry.startFrame 
		curEntryGet[4] = curEntry.endFrame
		curEntryGet[5] = curEntry.width 
		curEntryGet[6] = curEntry.height
		curEntryGet[7] = curEntry.pixelAspect
		curEntryGet[8] = curEntry.name
		curEntryGet[9] = curEntry.outputFilename
		if(curEntry.camera != undefined) then 
			curEntryGet[10] = curEntry.camera.name
		else curEntryGet[10] = curEntry.camera
		curEntryGet[11] = curEntry.sceneStateName
		curEntryGet[12] = curEntry.presetFile
		append batchBank curEntryGet
		curEntryGet = #()	
	)	
	return batchBank	
)


global batchFileName

FN BatchFileSave incEntries = (
	global batchFileName
	
	if(batchFileName == undefined) do batchFileName = substring (maxfilepath + maxFileName) 1 ((maxfilepath + maxFileName).count - 3) + "csv"

	batchFileName = getSaveFileName caption:"Save Batch Render" filename:(batchFileName)  types:"Batch Render List(*.csv)|*.csv" 

	if(batchFileName != undefined) do(
		local fileOpen = openFile batchFileName mode:"w+"
		
		for i = 1 to incEntries.count do(
			format "%\n" (incEntries[i][1] as string + "," + incEntries[i][2] as string  + "," + incEntries[i][3] as string  + "," + 
							incEntries[i][4] as string  + "," + 	incEntries[i][5] as string  + "," + incEntries[i][6] as string  + "," +
							incEntries[i][7] as string  + "," + incEntries[i][8] as string  + "," + incEntries[i][9] as string  + "," +
							incEntries[i][10] as string  + "," + incEntries[i][11] as string  + "," + incEntries[i][12] as string 	
							) to:fileOpen
		)
		
		close fileOpen 
	)
	return batchFileName	
	clearListener()
)


FN BatchClear = (
	local batchRenderDialog = windows.getChildHWND 0 "Batch Render"
	
	if(batchRenderDialog != undefined) do	windows.sendMessage batchRenderDialog[1] 0x0112 0xF060 0	

	if(batchRenderMgr.numViews > 0) do for i = 1 to batchRenderMgr.numViews do batchRenderMgr.DeleteView 1	
	clearListener()	
)


FN BatchFileOpen incBatchFile = (
	local entriesRead = #()
	local splitLine = #()
	local fileOpen = openFile batchFileName mode:"r"
	
	while not eof fileOpen do(
		local nextLine = readLine fileOpen
		local splitLine = filterstring nextLine ","	
		append entriesRead splitLine
	)
	
	close fileOpen

	local entriesBank = #()

	for i = 1 to entriesRead.count do(
		local tmpEntry = #()	
		tmpEntry[1] = entriesRead[i][1] as BooleanClass	
		tmpEntry[2] = entriesRead[i][2] as BooleanClass	
		tmpEntry[3] = entriesRead[i][3] as time
		tmpEntry[4] = entriesRead[i][4] as time
		tmpEntry[5] = entriesRead[i][5] as integer
		tmpEntry[6] = entriesRead[i][6] as integer
		tmpEntry[7] = entriesRead[i][7] as float
		tmpEntry[8] = entriesRead[i][8] as string
		tmpEntry[9] = entriesRead[i][9] as string
		if(entriesRead[i][10] != "undefined")then 
			tmpEntry[10] = entriesRead[i][10] as string
		else tmpEntry[10] = undefined	
		if(entriesRead[i][11] != undefined)then 
			tmpEntry[11] = entriesRead[i][11] as string
		else tmpEntry[11] = undefined		
		if(entriesRead[i][12] != undefined)then 
			tmpEntry[12] = entriesRead[i][12] as string
		else tmpEntry[12] = undefined	

		entriesBank[i] = tmpEntry		
	)
	
	return 	entriesBank
)


FN newBatchEntries incProps = (	
	local batchRenderDialog = windows.getChildHWND 0 "Batch Render"
	
	if(batchRenderDialog != undefined) do	windows.sendMessage batchRenderDialog[1] 0x0112 0xF060 0	
	
	BatchClear()	
	
	for i = 1 to incProps.count do(	
		sceneCam = try(getNodeByName incProps[i][10])catch()	
		curBatchRender = batchRenderMgr 	
		curEntry = curBatchRender.CreateView sceneCam
		curEntry.enabled = incProps[i][1]
		curEntry.overridePreset  = incProps[i][2]
		curEntry.startFrame  = incProps[i][3]
		curEntry.endFrame = incProps[i][4]
		curEntry.width  = incProps[i][5]
		curEntry.height = incProps[i][6]
		curEntry.pixelAspect = incProps[i][7]
			
		if(batchRenderMgr.FindView incProps[i][8] == 0) do curEntry.name = (incProps[i][8]) 

		curEntry.outputFilename = incProps[i][9]
			
		if(incProps[i][11] != undefined) then 
			curEntry.sceneStateName = incProps[i][11] 
		else curEntry.sceneStateName = ""		

		if(incProps[i][12] != undefined) then 
			curEntry.presetFile = incProps[i][12] 
		else curEntry.presetFile = ""	

	)
	
	clearListener()	
)


FN BatchScriptCreate incProps = (
	local curBatchRender = batchRenderMgr 
	local curBREntryCount = curBatchRender.numViews
	
	if(curBREntryCount >0)do(	
		local batchFileName = ((getDir #export) + "\batch_render_recreator.ms")	
		local fileOpen = openFile batchFileName mode:"w+"
		
		for i = 1 to incProps.count do(
			format ("sceneCam = try(getNodeByName \"" + incProps[i][10] as string + "\")catch()\n") to:fileOpen 
			format (
						"curBatchRender = batchRenderMgr \n" +
						"curEntry = curBatchRender.CreateView sceneCam \n" +
						"curEntry.enabled = " + incProps[i][1] as string + "\n" + 
						"curEntry.overridePreset = " + incProps[i][2] as string + "\n" +
						"curEntry.startFrame = " + incProps[i][3] as string + "\n" +
						"curEntry.endFrame = " + incProps[i][4] as string + "\n" +
						"curEntry.width = " + incProps[i][5] as string + "\n" +
						"curEntry.height = " + incProps[i][6] as string + "\n" +
						"curEntry.pixelAspect = " + incProps[i][7] as string + "\n"
					) to:fileOpen 
					
			format ("if(batchRenderMgr.FindView \"" + incProps[i][8] + "\" == 0) do (curEntry.name = \"" + incProps[i][8] as string + "\")\n") to:fileOpen

			if(incProps[i][9] != "" ) do (format ("curEntry.outputFilename = \"" + incProps[i][9]  + "\"\n" )to:fileOpen)

			if(incProps[i][11] != "" ) do (format ("curEntry.sceneStateName = \"" + incProps[i][11]  + "\"\n" )to:fileOpen)
			if(incProps[i][12] != "" ) do (
				local presetFile = filenameFromPath  incProps[i][12] 
				local presetPath = getFilenamePath incProps[i][12] + "\\"	
				format ("curEntry.presetFile = \"" +  presetPath + presetFile + "\"\n" )to:fileOpen
			)
			
			format "\n" to:fileOpen	
		)
		
		close fileOpen 	
		edit batchFileName
	)	
)


if(batchRenderToolDialog == undefined)then(
	global batchFileName
	global batchScriptFileName
	
	rollout batchRenderToolDialog "Batch Render Tools"(
		button btn_fileOpenBatch "Toggle Batch Render" width:140 pos:[15,10]
		on btn_fileOpenBatch pressed do(
			local batchRenderDialog=windows.getChildHWND 0 "Batch Render"
			if(batchRenderDialog != undefined) then	(windows.sendMessage batchRenderDialog[1] 0x0112 0xF060 0	)
			else (actionMan.executeAction -43434444 "4096")		
		)
			
		groupbox grp_brFile "Batch Render File (.csv)" width:150 height:100 pos:[10,40]
		
		button btn_saveBrFile "Save Batch File" width:140 pos:[15,60]
		on btn_saveBrFile pressed do(
			currentBatchEntries = BatchQuery()
			if(currentBatchEntries != undefined) do batchFileName = BatchFileSave currentBatchEntries	
			clearListener()		
		)
			
		button btn_loadBrFile "Load Batch File" width:140 pos:[15,85]
		on btn_loadBrFile pressed do(
			if(batchFileName == undefined) do batchFileName = substring (maxfilepath + maxFileName) 1 ((maxfilepath + maxFileName).count - 3) + "csv"

			batchFilePath = getOpenFileName caption:"Open Batch Render" filename:(batchFileName)  types:"Batch Render List(*.csv)|*.csv" 

			if(batchFilePath != undefined) do(
				batchFileName = 	batchFilePath
				fileEntriesRead = BatchFileOpen batchFileName	
				newBatchEntries fileEntriesRead
				clearListener()
				local batchRenderDialog = windows.getChildHWND 0 "Batch Render"

				if(batchRenderDialog != undefined) then	(windows.sendMessage batchRenderDialog[1] 0x0112 0xF060 0	;actionMan.executeAction -43434444 "4096")
				else (actionMan.executeAction -43434444 "4096")	
			)	
		)
		
		button btn_findInExpl1 "Find in Explorer" width:140 pos:[15,110]
		on btn_findInExpl1 pressed do(
			if(batchFileName != undefined and pathConfig.isLegalPath batchFileName)do	
			try(doscommand ("explorer \"" + (getFilenamePath batchFileName)  + "\""))catch()	
		)
		
		groupbox grp_BrScript "Batch Render Script (.ms)" width:150 height:100 pos:[10,140]	

		button mySavebrScript_btn "Save Script" width:140 pos:[15,160]
		on mySavebrScript_btn pressed do(
			currentBatchEntries = BatchQuery()
			BatchScriptCreate currentBatchEntries	
			clearListener()	
		)
		
		button btn_loadBrScript "Edit Script" width:140 pos:[15,185]	
		on btn_loadBrScript pressed do(
			batchScriptFileName = ((getDir #export) + "\batch_render_recreator.ms")	
			if((getFiles batchScriptFileName).count > 0)do edit batchScriptFileName  
		)
		
		button btn_findInExpl "Find in Explorer" width:140 pos:[15,210]	
		on btn_findInExpl pressed do (
			if(batchScriptFileName != undefined and pathConfig.isLegalPath batchScriptFileName)do	
			try(doscommand ("explorer \"" + (getFilenamePath batchScriptFileName)  + "\""))catch()	
		)	
		
		button btn_clearBR "Clear Batch Render" width:140 pos:[15,245]	
		on btn_clearBR pressed do(
			BatchClear()	
		)	
	)
	
	try(destroyDialog batchRenderToolDialog)catch()
	createDialog batchRenderToolDialog width:170 height:280
	clearListener()
)
else(
	try(destroyDialog batchRenderToolDialog)catch()
	createDialog batchRenderToolDialog width:170 height:280
	clearListener()
)
; AutoIt script to automate AnthemScore

; Open AnthemScore
;Run("C:\Users\Administrator\Desktop\anthemScore2\AnthemScore.exe")
;WinWait("AnthemScore", "", 8) 
;MsgBox(0, "Debug", "AnthemScore started")

; Function to process an audio chunk
Func ProcessAudioChunk($chunkFile, $outputFile)

    ; Send Ctrl+O to open the file dialog
    Send("^o")
    WinWait("Select File", "", 8) 
    Send($chunkFile)
    Send("{ENTER}")
    ;Sleep(500)

    ; Check for the confirmation dialog and cancel it if it appears
    If WinWait("Confirm", "", 1) Then
    ;MsgBox(0, "Debug", "Confirm dialog")
        If WinActive("[CLASS:Qt663QWindowIcon]") Then
            Send("{TAB}")
            Send("{ENTER}")
        EndIf
    EndIf
    ;MsgBox(0, "Debug", "after confirm  if")

    WinWait("Select File", "", 3)
    ;MsgBox(0, "Debug", "Select File")
    ; Select OK on the select file final dialog
    Send("{ENTER}")
    ;MsgBox(0, "Debug", "after select file")

    ; Check for the trail edition dialog and select ok if it appears
    ;If WinWait("AnthemScore", "The trial has a maximum song length of 30.0 s. Purchase an activation key to see the full song.", 1) Then
        ;MsgBox(0, "Debug", "AnthemScore trial")
        ;If WinActive("[CLASS:Qt663QWindowIcon]") Then
       ;     Send("{ENTER}")
       ; EndIf
   ; EndIf

    ; After processing is complete, send Ctrl+E to save the file
    While Not WinExists("Export", "")
        ; Check for the trail edition dialog and select ok if it appears
        Send("{ENTER}")
        Send("^e")
        Sleep(100) ; Wait for 100 milliseconds before sending again
    WEnd
    Local $i = 0;
    Local $end = 11
    While $i < $end
            Send("{TAB}")
            $i+=1
    WEnd
    Send($outputFile)
    Send("{ENTER}")
    
    ; Check for the override dialog
    If WinWait("Confirm", "", 1) Then
        If WinActive("[CLASS:Qt663QWindowIcon]") Then
            Send("{ENTER}")
        EndIf
    EndIf



    ; Wait for the output file to be created
    Local $maxWaitTime = 3000 ; Maximum wait time in milliseconds (15 seconds)
    Local $waitInterval = 100 ; Check interval in milliseconds (.5 second)
    Local $elapsedTime = 0

    While $elapsedTime < $maxWaitTime
        If FileExists($outputFile) Then
            ;MsgBox(0, "Debug", "Output file found")
            ExitLoop
        EndIf
        Sleep($waitInterval)
        $elapsedTime += $waitInterval
    WEnd

    ; Check if the file was not created within the maximum wait time
    If Not FileExists($outputFile) Then
        ;MsgBox(0, "Error", "Output file was not created within the expected time.")
    Else
        ;MsgBox(0, "Debug", "Output file created successfully")
    EndIf
EndFunc

; Example usage
Local $chunkFile = "C:\Users\Administrator\Desktop\AIInput\Wind (mp3cut.net) (1).mp3"
Local $outputFile = "C:\Users\Administrator\Desktop\AIOutput\Wind (mp3cut.net) (1).musicxml"
ProcessAudioChunk($chunkFile, $outputFile)
; AutoIt script to automate AnthemScore

; Open AnthemScore
Run("C:\Users\digi max\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\AnthemScore\AnthemScore.exe")
Sleep(5000) ; Wait for AnthemScore to load

; Function to process an audio chunk
Func ProcessAudioChunk($chunkFile, $outputFile)

    ; Send Ctrl+O to open the file dialog
    Send("^o")
    Sleep(1000)
    ;=========================================================================
    
    ; Type the file path and press Enter
    Send($chunkFile)
    Send("{ENTER}")
    ; Adjust the a min sleep time for closing the dialog
    Sleep(1000) 
    ;==============================================================================

    ; Check for the confirmation dialog and cancel it if it appears
    If WinWaitActive("Confirm", "There are unsaved changes.", 5) Then
        Send("{TAB}")
        Send("{ENTER}")
        Sleep(500)
    EndIf

    ; select ok of the select file final dialog
    Send("{ENTER}")
    ; Adjust a min sleep time based on how long AnthemScore takes to process the file
    Sleep(1000) 
    
    ; Wait for the output file to be created
    Local $maxWaitTime = 40000 ; Maximum wait time in milliseconds (15 seconds)
    Local $waitInterval = 500 ; Check interval in milliseconds (.5 second)
    Local $elapsedTime = 0

    While $elapsedTime < $maxWaitTime
        If FileExists($outputFile) Then
            ExitLoop
        EndIf
        Sleep($waitInterval)
        $elapsedTime += $waitInterval
    WEnd

    ; Check if the file was not created within the maximum wait time
    If Not FileExists($outputFile) Then
        MsgBox(0, "Error", "Output file was not created within the expected time.")
    EndIf

EndFunc

; Example usage
Local $chunkFile = "D:\ArmanFanavaranParsRayaneh\Projects\bigProject\MusicAi\AIInPut\Wind (mp3cut.net) (1).mp3"
Local $outputFile = "D:\ArmanFanavaranParsRayaneh\Projects\bigProject\MusicAi\AIOutPut\Wind (mp3cut.net) (1).musicxml"
ProcessAudioChunk($chunkFile, $outputFile)

; Add more chunks as needed
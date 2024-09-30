; AutoIt script to automate AnthemScore in the background

; Function to process an audio chunk
Func ProcessAudioChunk($chunkFile, $outputFile)
    ; Open AnthemScore minimized
    Run("C:\Users\digi max\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\AnthemScore\AnthemScore.exe", "", @SW_MINIMIZE)
    If WinWait("AnthemScore", "", 10) Then
        ConsoleWrite("AnthemScore started" & @CRLF)
    Else
        MsgBox(0, "Error", "AnthemScore did not start")
        Exit
    EndIf

    ; Send Ctrl+O to open the file dialog in the background
    ControlSend("AnthemScore", "", "", "^o")
    If WinWait("Select File", "", 10) Then
        ConsoleWrite("File dialog opened" & @CRLF)
    Else
        MsgBox(0, "Error", "File dialog did not open")
        Exit
    EndIf

    ; Interact with the file dialog
    ControlSetText("Select File", "", "[CLASS:Edit; INSTANCE:1]", $chunkFile)
    ControlSend("Select File", "", "", "{ENTER}")
    Sleep(1000)

    ; Check for the confirmation dialog and cancel it if it appears
    If WinWait("Confirm", "", 5) Then
        ConsoleWrite("Confirm dialog appeared" & @CRLF)
        If WinActive("[CLASS:Qt663QWindowIcon]") Then
            ControlSend("Confirm", "", "", "{TAB}")
            ControlSend("Confirm", "", "", "{ENTER}")
            ConsoleWrite("Handled confirm dialog" & @CRLF)
        EndIf
    EndIf

    ; Wait for the processing to complete by checking for the progress bar
    While ControlGetText("[CLASS:WindowClass]", "", "[CLASS:msctls_progress32]") <> ""
        Sleep(1000) ; Wait for 1 second before checking again
    WEnd
    ConsoleWrite("Processing complete" & @CRLF)

    ; After processing is complete, continuously send Ctrl+E to save the file
    While Not WinExists("Export", "")
        ControlSend("AnthemScore", "", "", "^e")
        Sleep(100) ; Wait for 100 milliseconds before sending again
    WEnd

    ; Wait for the Export window to become active
    If WinWaitActive("Export", "", 10) Then
        ConsoleWrite("Export window appeared" & @CRLF)
        If WinActive("[CLASS:Qt663QWindowIcon]") Then
            ; Save the file with the desired output file path
            ControlSetText("Export", "", "[CLASS:Edit; INSTANCE:1]", $outputFile)
            ControlClick("Export", "", "[CLASS:Button; INSTANCE:1]") ; Click OK button

            ; Check for the override dialog
            If WinWait("Confirm", "", 5) Then
                ConsoleWrite("Override confirm dialog appeared" & @CRLF)
                If WinActive("[CLASS:Qt663QWindowIcon]") Then
                    ControlSend("Confirm", "", "", "{ENTER}")
                EndIf
            EndIf

            ; Wait for the output file to be created
            Local $maxWaitTime = 15000 ; Maximum wait time in milliseconds (15 seconds)
            Local $waitInterval = 500 ; Check interval in milliseconds (.5 second)
            Local $elapsedTime = 0

            While $elapsedTime < $maxWaitTime
                If FileExists($outputFile) Then
                    ConsoleWrite("Output file created successfully" & @CRLF)
                    ExitLoop
                EndIf
                Sleep($waitInterval)
                $elapsedTime += $waitInterval
            WEnd

            ; Check if the file was not created within the maximum wait time
            If Not FileExists($outputFile) Then
                MsgBox(0, "Error", "Output file was not created within the expected time.")
            EndIf
        Else
            MsgBox(0, "Error", "Export window did not become active.")
        EndIf
    Else
        MsgBox(0, "Error", "Export window did not appear.")
    EndIf
EndFunc

; Example usage
Local $chunkFile = "D:\ArmanFanavaranParsRayaneh\Projects\bigProject\MusicAi\AIInPut\Wind (mp3cut.net) (1).mp3"
Local $outputFile = "D:\ArmanFanavaranParsRayaneh\Projects\bigProject\MusicAi\AIOutPut\Wind (mp3cut.net) (1).musicxml"
ProcessAudioChunk($chunkFile, $outputFile)

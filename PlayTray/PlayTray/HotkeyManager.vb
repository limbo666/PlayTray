Imports System.Runtime.InteropServices

Public Class HotkeyManager
    ' Windows API declarations
    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function RegisterHotKey(hWnd As IntPtr, id As Integer, fsModifiers As UInteger, vk As UInteger) As Boolean
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function UnregisterHotKey(hWnd As IntPtr, id As Integer) As Boolean
    End Function

    ' Modifier keys
    Public Const MOD_ALT As UInteger = &H1
    Public Const MOD_CONTROL As UInteger = &H2
    Public Const MOD_SHIFT As UInteger = &H4
    Public Const MOD_WIN As UInteger = &H8

    ' Hotkey IDs
    Public Const HOTKEY_PLAY As Integer = 1
    Public Const HOTKEY_STOP As Integer = 2
    Public Const HOTKEY_SHOWTIPS As Integer = 3
    Public Const HOTKEY_EXIT As Integer = 4
    Public Const HOTKEY_OPENLINK As Integer = 5
    Public Const HOTKEY_VOLUMEUP As Integer = 6
    Public Const HOTKEY_VOLUMEDOWN As Integer = 7
    Public Const HOTKEY_TIMEDMUTE As Integer = 8
    Public Const HOTKEY_FAVORITE_BASE As Integer = 100 ' 100-109 for favorites 1-10
    Public Const HOTKEY_SAVEBELOVED As Integer = 9

    Public Const HOTKEY_SHOWCOVER As Integer = 10

    Public Const HOTKEY_SHOWLYRICS As Integer = 20

    Private _hwnd As IntPtr
    Private _registeredHotkeys As New Dictionary(Of Integer, String)
    Private _currentModifier As Integer = 1  ' track current modifier
    Public ReadOnly Property RegisteredHotkeys As Dictionary(Of Integer, String)
        Get
            Return _registeredHotkeys
        End Get
    End Property

    Public Sub New(windowHandle As IntPtr)
        _hwnd = windowHandle
    End Sub

    Public Function RegisterAllHotkeys(modifier As Integer) As Integer
        UnregisterAllHotkeys()

        Dim modifierKeys As UInteger = GetModifierKeys(modifier)
        Dim successCount As Integer = 0

        ' Store current modifier for key combo display
        _currentModifier = modifier

        ' Register basic hotkeys
        If TryRegisterHotkey(HOTKEY_PLAY, modifierKeys, Keys.P, "Play") Then successCount += 1
        If TryRegisterHotkey(HOTKEY_STOP, modifierKeys, Keys.S, "Stop") Then successCount += 1
        If TryRegisterHotkey(HOTKEY_SHOWTIPS, modifierKeys, Keys.T, "Show Tips") Then successCount += 1
        If TryRegisterHotkey(HOTKEY_EXIT, modifierKeys, Keys.X, "Exit") Then successCount += 1
        If TryRegisterHotkey(HOTKEY_OPENLINK, modifierKeys, Keys.O, "Open Link") Then successCount += 1
        If TryRegisterHotkey(HOTKEY_VOLUMEUP, modifierKeys, Keys.Oemplus, "Volume Up") Then successCount += 1
        If TryRegisterHotkey(HOTKEY_VOLUMEDOWN, modifierKeys, Keys.OemMinus, "Volume Down") Then successCount += 1
        If TryRegisterHotkey(HOTKEY_TIMEDMUTE, modifierKeys, Keys.M, "Timed Mute") Then successCount += 1
        If TryRegisterHotkey(HOTKEY_SAVEBELOVED, modifierKeys, Keys.B, "Save Beloved") Then successCount += 1
        If TryRegisterHotkey(HOTKEY_SHOWCOVER, modifierKeys, Keys.C, "Show Cover") Then successCount += 1
        ' If TryRegisterHotkey(HOTKEY_SHOWLYRICS, modifierKeys, Keys.L, "Show Lyrics") Then successCount += 1
        ' Register favorite hotkeys (1-10, where 0 = favorite 10)
        For i As Integer = 0 To 9
            Dim keyCode As Keys
            If i = 0 Then
                keyCode = Keys.D0
            Else
                keyCode = CType(Keys.D1 + (i - 1), Keys)
            End If

            Dim favoriteNum As Integer = If(i = 0, 10, i)
            If TryRegisterHotkey(HOTKEY_FAVORITE_BASE + i, modifierKeys, keyCode, "Favorite " & favoriteNum.ToString()) Then
                successCount += 1
            End If
        Next

        Return successCount
    End Function

    Public Function RegisterMediaKeys() As Integer
        Dim successCount As Integer = 0

        ' Media keys use no modifier
        If TryRegisterHotkey(200, 0, Keys.MediaPlayPause, "Media Play/Pause") Then successCount += 1
        If TryRegisterHotkey(201, 0, Keys.MediaStop, "Media Stop") Then successCount += 1
        If TryRegisterHotkey(202, 0, Keys.MediaNextTrack, "Media Next") Then successCount += 1
        If TryRegisterHotkey(203, 0, Keys.MediaPreviousTrack, "Media Previous") Then successCount += 1

        Return successCount
    End Function

    Private Function TryRegisterHotkey(id As Integer, modifiers As UInteger, key As Keys, description As String) As Boolean
        Try
            If RegisterHotKey(_hwnd, id, modifiers, CUInt(key)) Then
                _registeredHotkeys.Add(id, description)
                Return True
            End If
        Catch ex As Exception
            ' Hotkey already registered by another app
        End Try
        Return False
    End Function

    Public Sub UnregisterAllHotkeys()
        For Each id In _registeredHotkeys.Keys.ToList()
            UnregisterHotKey(_hwnd, id)
        Next
        _registeredHotkeys.Clear()
    End Sub

    Private Function GetModifierKeys(modifier As Integer) As UInteger
        Select Case modifier
            Case 0 ' NONE - disabled
                Return 0
            Case 1 ' CTRL + SHIFT
                Return MOD_CONTROL Or MOD_SHIFT
            Case 2 ' CTRL + ALT
                Return MOD_CONTROL Or MOD_ALT
            Case 3 ' CTRL + ALT + SHIFT
                Return MOD_CONTROL Or MOD_ALT Or MOD_SHIFT
            Case Else
                Return MOD_CONTROL Or MOD_SHIFT
        End Select
    End Function

    Public Function GetModifierString(modifier As Integer) As String
        Select Case modifier
            Case 0
                Return "None (Disabled)"
            Case 1
                Return "Ctrl+Shift"
            Case 2
                Return "Ctrl+Alt"
            Case 3
                Return "Ctrl+Alt+Shift"
            Case Else
                Return "Ctrl+Shift"
        End Select
    End Function

    Public Sub WriteActiveHotkeysFile(filePath As String)
        Try
            Dim lines As New List(Of String)()
            lines.Add("=== Play Tray - Registered Hotkeys ===")
            lines.Add("Generated: " & DateTime.Now.ToString())
            lines.Add("")

            If _registeredHotkeys.Count = 0 Then
                lines.Add("No hotkeys registered.")
            Else
                ' Group by type
                Dim basicHotkeys As New List(Of String)()
                Dim favoriteHotkeys As New List(Of String)()
                Dim mediaHotkeys As New List(Of String)()

                For Each kvp In _registeredHotkeys.OrderBy(Function(k) k.Key)
                    Dim hotkeyId As Integer = kvp.Key
                    Dim description As String = kvp.Value
                    Dim keyCombo As String = GetKeyComboString(hotkeyId)

                    Dim line As String = keyCombo.PadRight(25) & " → " & description

                    ' Categorize
                    If hotkeyId >= 200 AndAlso hotkeyId < 210 Then
                        ' Media keys
                        mediaHotkeys.Add(line)
                    ElseIf hotkeyId >= HOTKEY_FAVORITE_BASE AndAlso hotkeyId < HOTKEY_FAVORITE_BASE + 10 Then
                        ' Favorites
                        favoriteHotkeys.Add(line)
                    Else
                        ' Basic hotkeys
                        basicHotkeys.Add(line)
                    End If
                Next

                ' Write basic hotkeys
                If basicHotkeys.Count > 0 Then
                    lines.Add("--- Basic Controls ---")
                    lines.AddRange(basicHotkeys)
                    lines.Add("")
                End If

                ' Write favorite hotkeys
                If favoriteHotkeys.Count > 0 Then
                    lines.Add("--- Favorite Stations ---")
                    lines.AddRange(favoriteHotkeys)
                    lines.Add("")
                End If

                ' Write media keys
                If mediaHotkeys.Count > 0 Then
                    lines.Add("--- Multimedia Keys ---")
                    lines.AddRange(mediaHotkeys)
                End If
            End If

            System.IO.File.WriteAllLines(filePath, lines.ToArray())

        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    Private Function GetKeyComboString(hotkeyId As Integer) As String
        ' Get modifier string
        Dim modString As String = ""
        If _currentModifier > 0 Then
            modString = GetModifierString(_currentModifier) & "+"
        End If

        ' Determine key
        Dim keyName As String = ""

        Select Case hotkeyId
            Case HOTKEY_PLAY
                keyName = "P"
            Case HOTKEY_STOP
                keyName = "S"
            Case HOTKEY_SHOWTIPS
                keyName = "T"
            Case HOTKEY_EXIT
                keyName = "X"
            Case HOTKEY_OPENLINK
                keyName = "O"
            Case HOTKEY_VOLUMEUP
                keyName = "+"
            Case HOTKEY_VOLUMEDOWN
                keyName = "-"
            Case HOTKEY_TIMEDMUTE
                keyName = "M"
            Case HOTKEY_SAVEBELOVED
                keyName = "B"
            Case HOTKEY_SHOWCOVER
                keyName = "C"
            Case HOTKEY_SHOWLYRICS
                keyName = "L"
            Case 200
                Return "Media Play/Pause"
            Case 201
                Return "Media Stop"
            Case 202
                Return "Media Next"
            Case 203
                Return "Media Previous"
            Case Else
                ' Favorites (100-109)
                If hotkeyId >= HOTKEY_FAVORITE_BASE AndAlso hotkeyId < HOTKEY_FAVORITE_BASE + 10 Then
                    Dim favoriteNum As Integer = hotkeyId - HOTKEY_FAVORITE_BASE
                    Dim displayNum As Integer = If(favoriteNum = 0, 0, favoriteNum)
                    keyName = displayNum.ToString()
                Else
                    keyName = "?"
                End If
        End Select

        Return modString & keyName
    End Function
End Class

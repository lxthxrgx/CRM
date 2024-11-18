Imports System.Runtime.InteropServices
Imports Word = Microsoft.Office.Interop.Word
Imports System.IO

Public Class WordHelper
    Private Const MaxRetryAttempts As Integer = 5
    Private Const RetryDelayMilliseconds As Integer = 500

    Public Shared Function AddTextToContentControl(docPath As String,
                                              DogovirSuborendu As String,
                                              DateTime As String,
                                              PIB As String,
                                              edruofop As String,
                                              rnokpp As String,
                                              area As String,
                                              address_p As String,
                                              StrokDii As String,
                                              address As String,
                                              IPN As String,
                                              BanckAccount As String,
                                              PIBS As String,
                                              numtext As String,
                                              sum_text As String,
                                              suma As String,
                                              fullDate As String,
                                              Director As String,
                                              PIBSDirector As String,
                                                   subleaseDopNum As String,
                                                   subleaseDopDate As String,
                                                   subleaseDopName As String,
                                                   subleaseDoprnokpp As String,
                                                   subleaseDopstatus As String) As Boolean
        Dim wordApp As Word.Application = Nothing
        Dim doc As Word.Document = Nothing
        Console.WriteLine("Путь к файлу: " & docPath)
        Console.WriteLine("DogovirSuborendu: " & DogovirSuborendu)
        Console.WriteLine("DateTime: " & DateTime)
        Console.WriteLine("PIB: " & PIB)
        Console.WriteLine("edruofop: " & edruofop)
        Console.WriteLine("rnokpp: " & rnokpp)
        Console.WriteLine("area: " & area)
        Console.WriteLine("address_p: " & address_p)
        Console.WriteLine("StrokDii: " & StrokDii)
        Console.WriteLine("address: " & address)
        Console.WriteLine("IPN: " & IPN)
        Console.WriteLine("BanckAccount: " & BanckAccount)
        Console.WriteLine("PIBS: " & PIBS)
        Console.WriteLine("numtext: " & numtext)
        Console.WriteLine("sum_text: " & sum_text)
        Console.WriteLine("suma: " & suma)
        Console.WriteLine("fullDate: " & fullDate)
        Console.WriteLine("Director: " & Director)
        Console.WriteLine("PIBSDirector: " & PIBSDirector)

        Console.WriteLine("subleaseDopNum: " & subleaseDopNum)
        Console.WriteLine("subleaseDopDate: " & subleaseDopDate)
        Console.WriteLine("subleaseDopName: " & subleaseDopName)
        Console.WriteLine("subleaseDoprnokpp: " & subleaseDoprnokpp)
        Console.WriteLine("subleaseDopstatus: " & subleaseDopstatus)

        Try
            If Not File.Exists(docPath) Then
                Console.WriteLine("Ошибка: Файл не существует.")
                Return False
            End If

            wordApp = New Word.Application()
            wordApp.Visible = False ' Делаем Word невидимым

            ' Подключаемся к текущему документу Word
            Try
                doc = OpenDocumentWithRetry(wordApp, docPath)
            Catch ex As Exception
                Console.WriteLine("Ошибка при открытии документа: " & ex.Message)
                Return False
            End Try

            Dim contentData As New Dictionary(Of String, String) From {
                {"DateTime", DateTime},
                {"DogovirSuborendu", DogovirSuborendu},
                {"PIB", PIB},
                {"edruofop_Data", edruofop},
                {"rnokpp", rnokpp},
                {"area", area},
                {"address_p", address_p},
                {"StrokDii", StrokDii},
                {"address", address},
                {"IPN", IPN},
                {"BanckAccount", BanckAccount},
                {"PIBS", PIBS},
                {"area_text", numtext},
                {"sum_text", sum_text},
                {"suma", suma},
                {"fullDate", fullDate},
                {"Director", Director},
                {"PIBSDirector", PIBSDirector},
                {"subleaseDopNum", subleaseDopNum},
                {"subleaseDopDate", subleaseDopDate},
                {"subleaseDopName", subleaseDopName},
                {"subleaseDopRnokpp", subleaseDoprnokpp},
                {"subleaseDopStatus", subleaseDopstatus}
            }

            ' Проходим по элементам управления содержимым в документе
            Try
                For Each cc As Word.ContentControl In doc.ContentControls
                    If contentData.ContainsKey(cc.Title) Then
                        cc.Range.Text = contentData(cc.Title)
                    End If
                Next
            Catch ex As Exception
                Console.WriteLine("Ошибка при заполнении содержимого: " & ex.Message)
                Return False
            End Try

            ' Сохраняем документ
            Try
                doc.Save()
            Catch ex As Exception
                Console.WriteLine("Ошибка при сохранении документа: " & ex.Message)
                Return False
            End Try

        Catch ex As Exception
            Console.WriteLine("Общая ошибка: " & ex.Message)
        Finally
            If doc IsNot Nothing Then
                Try
                    ' Закрываем документ
                    doc.Close(SaveChanges:=False)
                    Marshal.ReleaseComObject(doc)
                Catch ex As Exception
                    Console.WriteLine("Ошибка при закрытии документа: " & ex.Message)
                End Try
            End If

            If wordApp IsNot Nothing Then
                Try
                    ' Закрываем приложение Word
                    wordApp.Quit()
                    Marshal.ReleaseComObject(wordApp)
                Catch ex As Exception
                    Console.WriteLine("Ошибка при закрытии приложения Word: " & ex.Message)
                End Try
            End If
        End Try
        Return True
    End Function

    Private Shared Function OpenDocumentWithRetry(wordApp As Word.Application, docPath As String) As Word.Document
        Dim attempt As Integer = 0
        Dim doc As Word.Document = Nothing

        While attempt < MaxRetryAttempts
            Try
                doc = wordApp.Documents.Open(docPath)
                Return doc
            Catch ex As COMException When ex.ErrorCode = &H8001010A
                attempt += 1
                If attempt >= MaxRetryAttempts Then
                    Throw
                End If
                Threading.Thread.Sleep(RetryDelayMilliseconds)
            End Try
        End While

        Return doc
    End Function
End Class

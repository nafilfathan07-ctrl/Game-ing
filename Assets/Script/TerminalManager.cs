using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;

public class TerminalManager : MonoBehaviour
{
    [Header("UI Terminal")]
    public TMP_Text consoleOutput;
    public TMP_InputField terminalInput;
    public ScrollRect terminalScroll;
    
    public FileSystemManager fileSystemManager;
    private bool isExecuting = false;

    [Header("Audio Terminal")]
    public AudioClip suaraBuka;   // Masukkan efek suara pas terminal pop-up
    public AudioClip suaraTutup;  // Masukkan efek suara pas tombol back diklik
    private AudioSource audioSource;

    private void Awake()
    {
        // Bikin komponen AudioSource otomatis pas game mulai
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // Set ke 0 (2D) biar suaranya selalu jelas di telinga
    }

    private void Start()
    {
        consoleOutput.text = "KopdOS v1.0.4 - Terminal\nType 'help' for available commands.\n\n";
        terminalInput.onSubmit.AddListener(ProsesPerintah);
    }

    private void OnEnable()
    {
        // 1. MAINKAN SUARA BUKA TERMINAL
        if (audioSource != null && suaraBuka != null)
        {
            audioSource.PlayOneShot(suaraBuka);
        }

        if (terminalInput != null && !isExecuting)
        {
            terminalInput.text = "";
            terminalInput.interactable = true;
            terminalInput.ActivateInputField();
            StartCoroutine(ScrollKeBawah());
        }
    }

    // --- FUNGSI BARU BUAT TOMBOL BACK / CLOSE TERMINAL ---
    public void TutupTerminal()
    {
        // 2. MAINKAN SUARA TUTUP (Pakai Trik Objek Sementara)
        if (suaraTutup != null)
        {
            GameObject pemutarSementara = new GameObject("SuaraTutupTemp");
            AudioSource src = pemutarSementara.AddComponent<AudioSource>();
            src.clip = suaraTutup;
            src.spatialBlend = 0f; // Biar tetep 2D
            src.Play();
            
            // Hancurkan pemutar sementara ini otomatis setelah durasi lagunya habis
            Destroy(pemutarSementara, suaraTutup.length); 
        }

        // Matikan UI Terminalnya
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy && !terminalInput.isFocused && !isExecuting)
        {
            terminalInput.ActivateInputField();
        }
    }

    private void ProsesPerintah(string input)
    {
        if (string.IsNullOrWhiteSpace(input) || isExecuting)
        {
            if (!isExecuting) terminalInput.ActivateInputField();
            return;
        }

        isExecuting = true;
        terminalInput.interactable = false;
        
        CetakTeks("rayhan@KopdOS:~$ " + input);
        
        string[] parts = input.Trim().Split(new char[] { ' ' }, 2);
        string command = parts[0].ToLower();
        string args = parts.Length > 1 ? parts[1].Trim() : "";

        StartCoroutine(EksekusiCommand(command, args));
    }

    private void CetakTeks(string teks)
    {
        if (!string.IsNullOrEmpty(teks))
        {
            consoleOutput.text += teks + "\n";
            StartCoroutine(ScrollKeBawah());
        }
    }

    private IEnumerator ScrollKeBawah()
    {
        yield return new WaitForEndOfFrame();
        Canvas.ForceUpdateCanvases();
        
        if (terminalScroll != null)
        {
            terminalScroll.verticalNormalizedPosition = 0f;
        }
    }

    private IEnumerator EksekusiCommand(string cmd, string args)
    {
        switch (cmd)
        {
            case "help":
                CetakTeks("KopdOS Available Commands:\n\n[ SYSTEM & UTILS ]\n  help, clear, date, whoami, uname, uptime, sudo, echo, neofetch, exit\n\n[ FILE & DIRECTORY ]\n  pwd, ls, cd, touch, mkdir, rm, cat, file\n\n[ NETWORK & HACKING ]\n  ifconfig, ping, wget, nmap, hashcat, sqlmap");
                break;

            case "clear":
                consoleOutput.text = "";
                break;

            case "date": CetakTeks(DateTime.Now.ToString("ddd MMM dd HH:mm:ss 'WIB' yyyy")); break;
            case "whoami": CetakTeks("rayhan"); break;
            case "uname": CetakTeks("KopdOS 5.15.0-generic x86_64"); break;
            case "uptime": CetakTeks(" " + DateTime.Now.ToString("HH:mm:ss") + " up 2 days,  1 user,  load average: 0.00, 0.01, 0.05"); break;
            case "sudo": CetakTeks("rayhan is not in the sudoers file. This incident will be reported."); break;
            case "exit": 
                CetakTeks("Process detached. Please close the window via GUI."); 
                // Opsional: Kamu bisa panggil TutupTerminal() di sini kalau mau terminalnya langsung ketutup pas ngetik exit
                break;
            case "pwd": CetakTeks("/home/rayhan/desktop/investigation"); break;

            case "echo":
                if (args.Contains(">"))
                {
                    string[] echoParts = args.Split(new char[] { '>' }, 2);
                    string teks = echoParts[0].Trim();
                    if (teks.StartsWith("\"") && teks.EndsWith("\"")) teks = teks.Substring(1, teks.Length - 2);
                    string namaFile = echoParts[1].Trim();
                    
                    if (fileSystemManager != null) fileSystemManager.TambahFile(namaFile, teks);
                }
                else
                {
                    CetakTeks(args);
                }
                break;

            case "ls":
                string isiDir = "";
                if (fileSystemManager != null)
                {
                    foreach (string f in fileSystemManager.GetDaftarFile()) isiDir += f + "\n";
                }
                CetakTeks(isiDir.TrimEnd());
                break;

            case "cd":
                if (string.IsNullOrEmpty(args) || args == "~" || args == "..") CetakTeks("");
                else CetakTeks("cd: " + args + ": No such file or directory");
                break;

            case "touch":
                if (string.IsNullOrEmpty(args)) CetakTeks("touch: missing file operand");
                else if (fileSystemManager != null && !fileSystemManager.fileData.ContainsKey(args)) fileSystemManager.TambahFile(args, "(Empty file)");
                break;

            case "mkdir":
                if (string.IsNullOrEmpty(args)) CetakTeks("mkdir: missing operand");
                else if (fileSystemManager != null && !fileSystemManager.fileData.ContainsKey(args + "/")) fileSystemManager.TambahFile(args + "/", "");
                else CetakTeks("mkdir: cannot create directory '" + args + "': File exists");
                break;

            case "rm":
                if (string.IsNullOrEmpty(args)) CetakTeks("rm: missing operand");
                else if (fileSystemManager != null && fileSystemManager.fileData.ContainsKey(args)) fileSystemManager.HapusFile(args);
                else if (fileSystemManager != null && fileSystemManager.fileData.ContainsKey(args + "/")) CetakTeks("rm: cannot remove '" + args + "': Is a directory");
                else CetakTeks("rm: cannot remove '" + args + "': No such file or directory");
                break;

            case "file":
                if (args == "surat_wasiat.pdf") CetakTeks("surat_wasiat.pdf: ELF 64-bit LSB executable, x86-64, version 1 (SYSV), dynamically linked, stripped");
                else if (fileSystemManager != null && fileSystemManager.fileData.ContainsKey(args)) CetakTeks(args + ": ASCII text");
                else CetakTeks(args + ": cannot open (No such file or directory)");
                break;

            case "cat":
                if (args == "surat_wasiat.pdf") CetakTeks("@@^%&!@$#*&^!@#^&*(GIBBERISH_DATA_DETECTED_CANNOT_READ_ENCRYPTED_PAYLOAD)@@^%&!");
                else if (fileSystemManager != null && fileSystemManager.fileData.ContainsKey(args)) CetakTeks(fileSystemManager.fileData[args]);
                else CetakTeks("cat: " + args + ": No such file or directory");
                break;

            case "ifconfig": CetakTeks("eth0: flags=4163<UP,BROADCAST,RUNNING>\n        inet 192.168.1.10  netmask 255.255.255.0"); break;

            case "ping":
                if (string.IsNullOrEmpty(args)) { CetakTeks("ping: usage error: Destination address required"); break; }
                CetakTeks("PING " + args + " (192.168.1." + UnityEngine.Random.Range(2, 255) + "): 56 data bytes");
                for (int i = 1; i <= 4; i++)
                {
                    yield return new WaitForSeconds(0.8f);
                    CetakTeks("64 bytes from " + args + ": icmp_seq=" + i + " ttl=64 time=0.0" + UnityEngine.Random.Range(20, 99) + " ms");
                }
                break;

            case "wget":
                if (string.IsNullOrEmpty(args)) { CetakTeks("wget: missing URL"); break; }
                CetakTeks("--" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "--  " + args);
                yield return new WaitForSeconds(0.5f);
                CetakTeks("Resolving target... connected.");
                CetakTeks("HTTP request sent, awaiting response... 200 OK");
                yield return new WaitForSeconds(0.5f);
                CetakTeks("Length: 24512 (24K) [application/zip]");
                for (int i = 20; i <= 100; i += 20)
                {
                    yield return new WaitForSeconds(0.4f);
                    CetakTeks(i + "% [===================>] " + (24512 * i / 100) + "  " + UnityEngine.Random.Range(100, 500) + "KB/s");
                }
                CetakTeks("'" + args + "' saved [24512/24512]");
                if (fileSystemManager != null) fileSystemManager.TambahFile("downloaded_file.zip", "PK\u0003\u0004\u0014\u0000\b\b\b\u0000(ENCRYPTED_ZIP_CONTENT)");
                break;

            case "nmap":
                if (string.IsNullOrEmpty(args)) { CetakTeks("nmap: Please specify a target."); break; }
                CetakTeks("Starting Nmap 7.93 at " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                yield return new WaitForSeconds(1.2f);
                CetakTeks("Scanning " + args + " [1000 ports]");
                yield return new WaitForSeconds(2.5f);
                CetakTeks("Discovered open port 22/tcp on " + args);
                yield return new WaitForSeconds(0.5f);
                CetakTeks("Discovered open port 80/tcp on " + args);
                CetakTeks("Nmap done: 1 IP address (1 host up) scanned in 4.21 seconds");
                break;

            case "hashcat":
            case "sqlmap":
                CetakTeks("TOOL NOT READY YET.");
                break;

            case "neofetch":
                CetakTeks("       .o+`                    rayhan@KopdOS\n      `ooo/                    -------------\n     `+oooo:                   OS: KopdOS 1.0.4 x86_64\n    `+oooooo:                  Kernel: 5.15.0-generic");
                break;

            case "": break;
            default: CetakTeks("KopdOS: command not found: " + cmd); break;
        }

        if (cmd != "clear" && cmd != "") CetakTeks(""); 
        
        isExecuting = false;
        terminalInput.interactable = true;
        terminalInput.text = "";
        terminalInput.ActivateInputField();
        StartCoroutine(ScrollKeBawah());
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

public class Network : MonoBehaviour
{

    public void Network_Run()
    {

        ProcessStartInfo cmd = new ProcessStartInfo();
        Process process = new Process();

        // 실행할 파일명 입력
        cmd.FileName = @"cmd";
        
        // cmd 창 띄우기 -- 
        cmd.CreateNoWindow = true;
        cmd.UseShellExecute = false;

        // cmd 데이터 받기
        cmd.RedirectStandardOutput = true;
        // cmd 데이터 보내기
        cmd.RedirectStandardInput = true;
        // cmd 오류내용 받기 
        cmd.RedirectStandardError = true;

        process.EnableRaisingEvents = false;
        process.StartInfo = cmd;
        
        process.Start();
        string query = "";
        //System.Diagnostics.Process.Start("explorer.exe", "http://www.naver.com");

        process.StandardInput.Write(@"mkdir bbb" + Environment.NewLine); // 명령어 수행
        //process.StandardInput.Write(@"C:\USERBASE\Application\Application.exe" + Environment.NewLine);

        process.StandardInput.Close();

        process.WaitForExit();
        process.Close();
    }
}


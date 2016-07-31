using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;

public class RenameCtrl : MonoBehaviour {

    public InputField OldNameInput;
    public InputField NewNameInput;
    public InputField FilePathInput;

    private string OldNameHead;
    private string OldNameFoot;
    private string NewNameHead;
    private string NewNameFoot;
    private string FilePath;

    public void OnStartRename() {
        if (OldNameInput != null) {
            SplitFileName(OldNameInput.text, ref OldNameHead, ref OldNameFoot);
        }
        if (NewNameInput != null) {
            SplitFileName(NewNameInput.text, ref NewNameHead, ref NewNameFoot);
        }
        if (FilePathInput != null) {
            FilePath = FilePathInput.text;
        }
        FileRename();
    }

    private void SplitFileName(string NewStr, ref string StrHead, ref string StrFoot) {
        string[] Strs = NewStr.Split(new string[] { "****" }, 2, System.StringSplitOptions.RemoveEmptyEntries);
        int StrsLength = Strs.GetLength(0);
        if (StrsLength >= 2) {
            StrHead = Strs[0];
            StrFoot = Strs[1];
        } else if (StrsLength == 1) {
            StrHead = Strs[0];
            StrFoot = null;
        } else {
            StrHead = null;
            StrFoot = null;
        }
    }

    private void FileRename() {
        if (Directory.Exists(FilePath)) {
            string[] FileNames = Directory.GetFiles(FilePath);
            int IndexLength = FileNames.Length.ToString().Length;
            foreach (string FileName in FileNames) {
                string OldFileName = Path.GetFileName(FileName);
                if ((OldNameHead != null) && (OldFileName.Contains(OldNameHead))) {
                    string[] FileClasss = OldFileName.Split(new char[] { '.' }, System.StringSplitOptions.RemoveEmptyEntries);
                    string FileClass = null;
                    if (FileClasss.Length > 1) {
                        FileClass = '.' + FileClasss[FileClasss.Length - 1];
                    }
                    string Index = SplitIndex(IndexLength, OldNameHead.Length, OldFileName);
                    string NewFileName = FilePath + NewNameHead + Index + NewNameFoot + FileClass;
                    File.Move(FileName, NewFileName);
                }
            }
        }
        Debug.Log("--------Rename End!");
    }

    private string SplitIndex(int IndexLength, int StartIndex, string FileName) {
        int i = StartIndex;
        string Index = null;
        while (((FileName[i] - '0') >= 0) && ((FileName[i] - '0') <= 9)) {
            if ((Index != null) || (FileName[i] != '0')) {
                Index = Index + FileName[i];
            }
            i++;
        }
        while (Index.Length < IndexLength) {
            Index = '0' + Index;
        }

        return Index;
    }
}

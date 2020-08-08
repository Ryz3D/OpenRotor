using System.Collections.Generic;
using System.IO;
using System.Linq;

public class FSSysIO : FileSystem
{
    public override void MakeDir(string path)
    {
        if (WhatIs(path) == FileType.Nonexistent) {
            DirectoryInfo info = Directory.CreateDirectory(path);
            status = info.Exists ? FSStatus.Success : FSStatus.UnknownError;
            CheckStatus();
        }
        else {
            status = FSStatus.AlreadyExists;
            CheckStatus();
        }
    }

    public override string Read(string path)
    {
        if (WhatIs(path) == FileType.File) {
            string data = File.ReadAllText(path);
            status = data.Length == 0 ? FSStatus.UnknownError : FSStatus.Success;
            CheckStatus();
            return data;
        }
        else {
            status = FSStatus.Nonexistent;
            CheckStatus();
            return "";
        }
    }

    public override byte[] ReadB(string path)
    {
        if (WhatIs(path) == FileType.File) {
            byte[] data = File.ReadAllBytes(path);
            status = data.Length == 0 ? FSStatus.UnknownError : FSStatus.Success;
            CheckStatus();
            return data;
        }
        else {
            status = FSStatus.Nonexistent;
            CheckStatus();
            return new byte[] {};
        }
    }

    public override FileType WhatIs(string path)
    {
        status = FSStatus.Success;

        if (File.Exists(path)) {
            return FileType.File;
        }
        if (Directory.Exists(path)) {
            return FileType.Directory;
        }
        return FileType.Nonexistent;
    }

    public override void Write(string path, string data)
    {
        if (WhatIs(path) == FileType.Directory) {
            status = FSStatus.DirectoryWriteAttempt;
            CheckStatus();
            return;
        }
        else if (WhatIs(path) == FileType.Nonexistent) {
            File.Create(path);
        }

        File.WriteAllText(path, data);
        status = FSStatus.Success;
    }

    public override void WriteB(string path, byte[] data)
    {
        if (WhatIs(path) == FileType.Directory) {
            status = FSStatus.DirectoryWriteAttempt;
            CheckStatus();
            return;
        }
        else if (WhatIs(path) == FileType.Nonexistent) {
            File.Create(path);
        }

        File.WriteAllBytes(path, data);
        status = FSStatus.Success;
    }

    public override List<string> ListFiles(string path) {
        if (WhatIs(path) == FileType.Directory) {
            return Directory.GetFiles(path).ToList();
        }
        else {
            status = FSStatus.Nonexistent;
            return new List<string>();
        }
    }

    public override List<string> ListDir(string path) {
        if (WhatIs(path) == FileType.Directory) {
            return Directory.GetDirectories(path).ToList();
        }
        else {
            status = FSStatus.Nonexistent;
            return new List<string>();
        }
    }

    public override FSStatus Remove(string path) {
        try {
            switch (WhatIs(path)) {
                case FileType.Directory:
                    Directory.Delete(path);
                    status = FSStatus.Success;
                    break;
                case FileType.File:
                    File.Delete(path);
                    status = FSStatus.Success;
                    break;
                case FileType.Nonexistent:
                    status = FSStatus.Nonexistent;
                    break;
                default:
                    status = FSStatus.UnknownError;
                    break;
            }
        }
        catch (System.UnauthorizedAccessException) {
            status = FSStatus.AccessDenied;
        }
        CheckStatus();
        return status;
    }
}

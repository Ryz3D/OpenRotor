using System.IO;

public class FSWindows : FileSystem
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
}

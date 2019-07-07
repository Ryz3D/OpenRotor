using UnityEngine;

public enum FSStatus {
    Unknown,
    Success,
    UnknownError,
    AccessDenied,
    Nonexistent,
    AlreadyExists,
    DirectoryWriteAttempt   // trying to write to a dir
}

public enum FileType {
    Unknown,
    Nonexistent,
    File,
    Directory
}

public abstract class FileSystem {
    public FSStatus status = FSStatus.Unknown;

    public abstract FileType WhatIs(string path);
    public abstract string Read(string path);
    public abstract byte[] ReadB(string path);
    public abstract void Write(string path, string data);
    public abstract void WriteB(string path, byte[] data);
    public abstract void MakeDir(string path);

    protected void CheckStatus() {
        if (status != FSStatus.Success) {
            Debug.LogError(status);
        }
    }
}

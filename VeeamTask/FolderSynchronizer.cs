using System.IO;
using System.Security.Cryptography;
using System.Threading;
class FolderSynchronizer
{
    private readonly string sourcePath;
    private readonly string replicaPath;
    private readonly int intervalLength;
    private readonly Logger logger;

    public FolderSynchronizer(string sourcePath, string replicaPath, int intervalLength, string loggerPath)
    {
        this.sourcePath = sourcePath;
        this.replicaPath = replicaPath;
        this.intervalLength = intervalLength;
        this.logger = new Logger(loggerPath);
    }

    public void StartSynchronizing()
    {
        if (!Directory.Exists(replicaPath))
        {
            Directory.CreateDirectory(replicaPath);
            logger.LogMessage($"Replica folder {replicaPath} created");
        }

        logger.LogMessage("Synchronization started");

        while (true)
        {
            try
            {
                SynchronizeContent(sourcePath, replicaPath);
                RemoveExtraFiles(replicaPath, sourcePath);
            }
            catch (Exception ex)
            {
                logger.LogMessage($"Error: {ex.Message}");
            }
            Thread.Sleep(intervalLength * 1000);
        }
    }

    private void SynchronizeContent(string sourceDir, string replicaDir)
    {
        foreach (string dirPath in Directory.GetDirectories(sourceDir))
        {
            string targetDirPath = Path.Combine(replicaDir, Path.GetFileName(dirPath));
            if (!Directory.Exists(targetDirPath))
            {
                Directory.CreateDirectory(targetDirPath);
                logger.LogMessage($"Folder {targetDirPath} created");
            }
            SynchronizeContent(dirPath, targetDirPath);
        }

        foreach (string filePath in Directory.GetFiles(sourceDir))
        {
            string targetFilePath = Path.Combine(replicaDir, Path.GetFileName(filePath));

            if (!File.Exists(targetFilePath))
            {
                File.Copy(filePath, targetFilePath, true);
                logger.LogMessage("File " + targetFilePath + " created");
            }
            else if (!CheckIfFilesEqual(filePath, targetFilePath))
            {
                File.Copy(filePath, targetFilePath, true);
                logger.LogMessage($"File {targetFilePath} modified");
            }
        }
    }

    private void RemoveExtraFiles(string replicaDir, string sourceDir)
    {
        foreach (string filePath in Directory.GetFiles(replicaDir))
        {
            string sourceFilePath = Path.Combine(sourceDir, Path.GetFileName(filePath));
            if (!File.Exists(sourceFilePath))
            {
                File.Delete(filePath);
                logger.LogMessage($"File {filePath} deleted");
            }
        }

        foreach (string dirPath in Directory.GetDirectories(replicaDir))
        {
            string sourceDirPath = Path.Combine(sourceDir, Path.GetFileName(dirPath));
            if (!Directory.Exists(sourceDirPath))
            {
                Directory.Delete(dirPath, true);
                logger.LogMessage($"Folder {dirPath} deleted");
            }
            else
            {
                RemoveExtraFiles(dirPath, sourceDirPath);
            }
        }
    }

    private static bool CheckIfFilesEqual(string file1, string file2)
    {
        using var md5 = MD5.Create();
        using var stream1 = File.OpenRead(file1);
        using var stream2 = File.OpenRead(file2);
        byte[] hash1 = md5.ComputeHash(stream1);
        byte[] hash2 = md5.ComputeHash(stream2);
        return hash1.SequenceEqual(hash2);
    }
}

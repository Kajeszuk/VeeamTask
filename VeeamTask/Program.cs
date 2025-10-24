class Program
{
    static void Main(string[] args)
    {
        FolderSynchronizer folderSynchronizer = new FolderSynchronizer(args[0], args[1], int.Parse(args[2]), args[3]);
        folderSynchronizer.StartSynchronizing();
    }
}
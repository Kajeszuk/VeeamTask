class FolderSynchronizer
{
    private readonly string sourcePath;
    private readonly string replicaPath;
    private readonly int intervalLength;
    private Logger logger;

    public FolderSynchronizer(string sourcePath, string replicaPath, int intervalLength)
    {
        this.sourcePath = sourcePath;
        this.replicaPath = replicaPath;
        this.intervalLength = intervalLength;
        this.logger = new Logger();
    }

    public void StartSynchronizing()
    {

    }

}

namespace DirectoriesComparer;

public class CompareProgressEventArg : EventArgs
{
    public float Progress { get; }
    public string StatusMessage { get; }

    public CompareProgressEventArg(float progress, string statusMessage = "")
    {
        Progress = progress;
        StatusMessage = statusMessage;
    }
}
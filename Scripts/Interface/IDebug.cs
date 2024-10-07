using CapturedGame;

public interface IDebug
{
    public string DebugName { get; }
    public bool IsDebugMode { get; }
    public void OnDebugChange(bool enable);
}

public interface IDebugText : IDebug
{
    public DraggableResizableTextBox TextBox { get; }
    public void DrawValuesContent();
}

public interface IDebugButton : IDebug
{
    public ResizableButtonManager DebugButton { get; }
}
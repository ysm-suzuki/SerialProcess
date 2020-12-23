using System.Collections;
using System.Collections.Generic;

public class SerialProcess
{
    public SerialProcess() { }
    public SerialProcess(bool autoFlush) { _autoFlush = autoFlush; }

    private bool _autoFlush = true;
    private Queue<System.Action<System.Action>> _processes = new Queue<System.Action<System.Action>>();

    public SerialProcess Add(System.Action<System.Action> process)
    {
        _processes.Enqueue(process);
        return this;
    }

    public void Flush()
    {
        Next();
    }

    public SerialProcess SetAutoFlush(bool autoFlush)
    {
        _autoFlush = autoFlush;
        return this;
    }

    private void Next()
    {
        if (_processes.Count == 0)
            return;

        var process = _processes.Dequeue();
        process(() =>
        {
            if (_autoFlush)
                Next();
        });
    }
}

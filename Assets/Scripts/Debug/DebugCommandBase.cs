using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DebugCommandBase
{
    private string _commandId;
    private string _commandDescription;
    private string _commandFormat;

    public string CommandId { get => _commandId; }
    public string CommandDescription { get => _commandDescription; }
    public string CommandFormat { get => _commandFormat; }

    public DebugCommandBase(string commandId, string commandDescription, string commandFormat)
    {
        _commandId = commandId;
        _commandDescription = commandDescription;
        _commandFormat = commandFormat;
    }
}

[System.Serializable]
public class DebugCommand : DebugCommandBase
{
    private Action command;
    public DebugCommand(string commandId, string commandDescription, string commandFormat, Action command) : base(commandId, commandDescription, commandFormat)
    {
        this.command = command;
    }
    public void Invoke (){
        command.Invoke();
    }
}

[System.Serializable]
public class DebugCommand<T> : DebugCommandBase
{
    private Action<T> command;
    public DebugCommand(string commandId, string commandDescription, string commandFormat, Action<T> command) : base(commandId, commandDescription, commandFormat)
    {
        this.command = command;
    }
    public void Invoke (T value){
        command.Invoke(value);
    }
}

public class DebugCommand<T,X> : DebugCommandBase
{
    private Action<T,X> command;
    public DebugCommand(string commandId, string commandDescription, string commandFormat, Action<T,X> command) : base(commandId, commandDescription, commandFormat)
    {
        this.command = command;
    }
    public void Invoke (T value, X xvalue){
        command.Invoke(value, xvalue);
    }
}

public class DebugCommand<T,X,Z> : DebugCommandBase {
    private Action<T,X,Z> command;
    
    public DebugCommand(string commandId, string commandDescription, string commandFormat, Action<T,X,Z> command) : base(commandId, commandDescription, commandFormat)
    {
        this.command = command;
    }
    public void Invoke (T value, X xvalue, Z zvalue){
        command.Invoke(value, xvalue, zvalue);
    }
}

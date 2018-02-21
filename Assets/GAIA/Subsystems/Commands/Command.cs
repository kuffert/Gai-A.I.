using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command : GAIAAction
{
    public enum CommandType { None, Peek, Shift, Infuse, Flourish }

    public class CommandResult
    {
        bool success = false;
        CommandType type = CommandType.None;
        float nodeCurrLife = -1;
        float nodeLifeRes = -1;
        float nodeLifeThresh = -1;
        Node node = null;

        public CommandResult(bool succ, CommandType ct, float cl, float lt, float lr, Node n)
        {
            success = succ;
            type = ct;
            nodeCurrLife = cl;
            nodeLifeThresh = lt;
            nodeLifeRes = lr;
            node = n;
        }
    }

    protected CommandResult result = null;

    private string commandDescriptor;

    public abstract CommandResult getResult();
}

public class CommandPeek : Command
{
    Node peekNode;


    CommandPeek(Node peekNode)
    {
        this.peekNode = peekNode;
    }

    public override void execute()
    {
        result = new CommandResult(true, CommandType.Peek, peekNode.currentLifeLevel, -1, -1, null);
    }

    public override void unexecute()
    {

    }

    public override CommandResult getResult()
    {
        return result;
    }
}

public class CommandShift : Command
{
    Node shiftNode;


    CommandShift(Node shiftNode)
    {
        this.shiftNode = shiftNode;
    }

    public override void execute()
    {
        result = new CommandResult(true, CommandType.Peek, shiftNode.currentLifeLevel, shiftNode.lifeResistance, shiftNode.lifeThreshhold, shiftNode);
    }

    public override void unexecute()
    {

    }

    public override CommandResult getResult()
    {
        return result;
    }
}


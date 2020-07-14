﻿using RussellLib.Assets;
using RussellLib.Base;
using System;
using System.Collections.Generic;

namespace RussellLib.Code
{
    public class GMAction
    {
        public enum ActionType
        {
            ACT_NORMAL,
            ACT_BEGIN,
            ACT_END,
            ACT_ELSE,
            ACT_EXIT,
            ACT_REPEAT,
            ACT_VARIABLE,
            ACT_CODE,

            // ??
            ACT_PLACEHOLDER,
            ACT_SEPARATOR,
            ACT_LABEL
        }

        public enum ActionExecuteType
        {
            EXE_NOTHING,
            EXE_FUNCTION,
            EXE_CODE
        }

        public enum ActionArgType
        {
            ARG_CONSTANT = -1,
            ARG_EXPRESSION,
            ARG_STRING,
            ARG_STRINGEXP,
            ARG_BOOLEAN,
            ARG_MENU,
            ARG_SPRITE,
            ARG_SOUND,
            ARG_BACKGROUND,
            ARG_PATH,
            ARG_SCRIPT,
            ARG_OBJECT,
            ARG_ROOM,
            ARG_FONTR,
            ARG_COLOR,
            ARG_TIMELINE,
            ARG_FONT
        }

        public int LibID;
        public int ID;
        public ActionType Kind;
        public bool UseRelative;
        public bool IsQuestion;
        public bool UseApplyTo;
        public ActionExecuteType ExeType;
        public string Name;
        public string Code;
        public int ArgumentCount;
        public List<ActionArgType> ArgumentTypes;
        public int Who; // since this may be -1 for self and -2 for other, it's better that you look it up yourself and rely on WhoObj
        public GMObject WhoObj;
        public bool Relative;
        public List<string> Arguments;
        public bool IsNot;

        public GMAction(ProjectReader reader)
        {
            int actionver = reader.ReadInt32(); // what?
            if (actionver != 440)
            {
                throw new Exception("Invalid Action version, got " + actionver);
            }
            LibID = reader.ReadInt32();
            ID = reader.ReadInt32();
            Kind = (ActionType)reader.ReadInt32();
            UseRelative = reader.ReadBoolean();
            IsQuestion = reader.ReadBoolean();
            UseApplyTo = reader.ReadBoolean();
            ExeType = (ActionExecuteType)reader.ReadInt32();
            Name = reader.ReadString();
            Code = reader.ReadString();
            ArgumentCount = reader.ReadInt32();
            int argc = reader.ReadInt32();
            ArgumentTypes = new List<ActionArgType>(argc);
            for (int i = 0; i < argc; i++)
            {
                ArgumentTypes.Add((ActionArgType)reader.ReadInt32());
            }
            Who = reader.ReadInt32();
            WhoObj = null;
            Relative = reader.ReadBoolean();
            int argc2 = reader.ReadInt32();
            Arguments = new List<string>(argc2);
            for (int i = 0; i < argc2; i++)
            {
                Arguments.Add(reader.ReadString());
            }
            IsNot = reader.ReadBoolean();
        }

        public void PostLoad(GMProject proj)
        {
            if (Who > -1)
            {
                WhoObj = proj.Objects[Who];
            }
        }
    }
}

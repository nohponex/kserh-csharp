using System;

using System.Runtime.InteropServices;

namespace Project_Kserh_Online {

    public enum Players : byte { None = 0, Player1, Player2 };

    public enum Suit : byte { Clubs = 1, Diamonds, Hearts, Spades };
    public enum Ranks : byte { Ace = 1, two, three, four, five, six, seven, eight, nine, ten, Jack, Queen, King };

    public enum NetworkCommands : byte { Start = 0, TakeCard, oppTakenCard, Play, oppPlayed, ClearDownCards, AddDownCard,UpdateInfo,newRound,ChatText, GameOver ,Disconnect};

    [Serializable]
    [StructLayout(LayoutKind.Sequential,Pack = 1)]
    public struct CommandStruct {
        public ushort CommandIndex;
        public uint ExtraDataSize;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential,Pack = 1)]
    public struct CardStr {
        public Suit suit;
        public Ranks rank;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Info
    {
        public Players turn;
        public ushort deckLeft;
        public ushort player1points;
        public ushort player1cards;
        public ushort player2points;
        public ushort player2cards;
    }
    public static class Common{
        public static ushort CommandStructSize = (ushort)Marshal.SizeOf(typeof(CommandStruct));

        public static ushort CardStructSize = (ushort)Marshal.SizeOf(typeof(CardStr));

        public static ushort InfoStructSize = (ushort)Marshal.SizeOf(typeof(Info));

        public static Byte[] SerializeStruct<T>(T obj) {
            int objsize = Marshal.SizeOf(typeof(T));
           
            Byte[] ret = new Byte[objsize];

            IntPtr buff = Marshal.AllocHGlobal(objsize);

            Marshal.StructureToPtr(obj, buff, true);

            Marshal.Copy(buff, ret, 0, objsize);

            Marshal.FreeHGlobal(buff);

            return ret;
        }

        public static T DeSerializeStruct<T>(Byte[] data)
        {
            int objsize = Marshal.SizeOf(typeof(T));
            IntPtr buff = Marshal.AllocHGlobal(objsize);

            Marshal.Copy(data, 0, buff, objsize);

            T retStruct = (T)Marshal.PtrToStructure(buff, typeof(T));

            Marshal.FreeHGlobal(buff);

            return retStruct;
        }

        public static T ParseEnumValue<T>(ushort index) {
            return (T)Enum.ToObject(typeof(T), index);
        }
       

        //public static Byte[] CommandSerialization(CommandStruct cmd) {
        //    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        //    MemoryStream stream = new MemoryStream();
        //    formatter.Serialize(stream, cmd);
        //    Byte[] buffer = stream.ToArray();
        //    return buffer;
        //}

        //public static CommandStruct CommandDeSerialization(Byte[] cmdbytes) {
        //    CommandStruct cmd;
        //    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        //    MemoryStream stream = new MemoryStream(cmdbytes);
        //    cmd = (CommandStruct)formatter.Deserialize(stream);
        //    stream.Close();
        //    return cmd;
        //}
    }
}

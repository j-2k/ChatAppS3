using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    class BinaryFormatterClass
    {

        public static byte[] ObjectToByteArray(Object obj)  // OBJECT TO BYTE BF
        {
            BinaryFormatter bf = new BinaryFormatter();     //Creating a new binary formatter
            using (MemoryStream ms = new MemoryStream())    //making a memory stream //using responsible for memory cleanup
            {
                bf.Serialize(ms, obj);                      //serialize the stream with the object we take in
                return ms.ToArray();                        //return the serialized stream
            }
        }

        public static Object ByteArrayToObject(Byte[] byteArray)    // BYTE TO OBJECT BF
        {
            using(MemoryStream ms = new MemoryStream())             //a new mem stream
            {
                BinaryFormatter bf = new BinaryFormatter();         //new bf & begin undoing the serialization (deserialize)
                ms.Write(byteArray, 0, byteArray.Length);           //write the entire byte array to memory with offset 0 to the size of the byte array
                ms.Seek(0, SeekOrigin.Begin);                       //setting the START of the memory stream we put 0 becaues we dont want an offset & we make it start at the start of the MS
                Object obj = bf.Deserialize(ms);                    //then finally the object will be equal to the deserialized MS (using BF to deserialize)
                return obj;                                         //return deserialized obj
            }
        }

    }
}

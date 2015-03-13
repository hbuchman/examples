using System;
using System.Runtime.Serialization;

namespace ToDoList
{
    /// <summary>
    /// This is the data format that is used to represent items in the ToDo list
    /// in both the server and the client.
    /// </summary>
    [DataContract]
    public class ToDoItem
    {
        [DataMember]
        public string Uid { get; set; }

        [DataMember]
        public string Owner { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool Completed { get; set; }
    }
}

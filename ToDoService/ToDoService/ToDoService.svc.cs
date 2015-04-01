using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace ToDoList
{
    /// <summary>
    /// Provides implementations of the operations belonging to the service.
    /// </summary>
    public class ToDoService : IToDoService
    {
        // This amounts to a poor man's database.  The state of the service is
        // maintained in items, which is a list of ToDoItems.  The next uid to
        // be issued is contained in the uid variable.  The sync object is used
        // for synchronization (because multiple threads can be running
        // simultaneously in the service).  The entire state is lost each time
        // the service shuts down, so eventually we'll need to port this to
        // a proper database.
        private readonly static IList<ToDoItem> items = new List<ToDoItem>();
        private static long uid = 0;
        private static readonly object sync = new object();

        static ToDoService ()
        {
            EventLog appLog = new System.Diagnostics.EventLog();
            appLog.Source = "Application";
            appLog.WriteEntry("ToDoServer: This is a test", EventLogEntryType.Information);
        }

        public IList<ToDoItem> GetAllItems(bool includeCompleted, string userName)
        {
            List<ToDoItem> result = new List<ToDoItem>();
            lock (sync)
            {
                foreach (ToDoItem item in items)
                {
                    if ((!item.Completed || includeCompleted) && ((userName == "") || (item.Owner == userName)))
                    {
                        result.Add(item);
                    }
                }
            }
            return result;
        }

        public string AddItem(ToDoItem item)
        {
            //WebOperationContext.Current.OutgoingResponse.StatusCode = (HttpStatusCode) 409;

            lock(sync)
            {
                item.Uid = "" + uid++;
                items.Add(item);
                return item.Uid;
            }
        }

        public void MarkCompleted(string uid)
        {
            lock (sync)
            {
                foreach (ToDoItem item in items)
                {
                    if (item.Uid == uid)
                    {
                        item.Completed = true;
                        return;
                    }
                }
            }
        }

        public void DeleteItem (string uid)
        {
            lock(sync)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].Uid.ToString() == uid)
                    {
                        items.RemoveAt(i);
                        return;
                    }
                }
            }
        }
    }
}

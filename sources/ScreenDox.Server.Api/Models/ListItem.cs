using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScreenDox.Server.Api.Models
{
    /// <summary>
    /// Model used for UI list bindings
    /// </summary>
    public class ListItem
    {
        /// <summary>
        /// Unique id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Label
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ListItem()
        {
        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public ListItem(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
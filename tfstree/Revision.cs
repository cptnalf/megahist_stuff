using System;
using System.Collections.Generic;

namespace TFSTree
{
    /// <summary>A revision in monotone.</summary>
    class Revision
    {
        /// <summary>Unique identifier.</summary>
        readonly string id;

        /// <summary>Branch the revision belongs to.</summary>
        readonly string branch;

        /// <summary>Author of the revision.</summary>
        string author;

        /// <summary>Date and time when the revision was created.</summary>
        DateTime date;

        /// <summary>Description of the revision as specified by the author.</summary>
        string log;

        /// <summary>All parent IDs of this revision.</summary>
        List<string> parents = new List<string>();

        /// <summary>Creates a new revision.</summary>
        /// <param name="id">Unique identifier.</param>
        /// <param name="branch">Branch.</param>
        public Revision(string id, string branch)
        {
            this.id = id;
            this.branch = branch;
        }

        /// <summary>Creates a new revision.</summary>
        /// <param name="id">Unique identifier.</param>
        /// <param name="branch">Branch.</param>
        /// <param name="author">Author.</param>
        /// <param name="date">Date.</param>
        /// <param name="log">Log.</param>
        public Revision(string id, string branch, string author, string date, string log)
        {
            this.id = id;
            this.branch = branch;
            this.author = author;
            this.date = DateTime.Parse(date);
            this.log = log;
        }

        /// <summary>Gets the unique identifier.</summary>
        /// <value>Gets the unique identifier.</value>
        public string ID
        {
            get
            {
                return id;
            }
        }

        /// <summary>Gets the branch.</summary>
        /// <value>Gets the branch.</value>
        public string Branch
        {
            get
            {
                return branch;
            }
        }

        /// <summary>Gets or sets the author.</summary>
        /// <value>Gets or sets the author.</value>
        public string Author
        {
            get
            {
                return author;
            }

            set
            {
                author = value;
            }
        }

        /// <summary>Gets or sets the date and time.</summary>
        /// <value>Gets or sets the date and time.</value>
        public DateTime Date
        {
            get
            {
                return date;
            }

            set
            {
                date = value;
            }
        }

        /// <summary>Gets or sets the description.</summary>
        /// <value>Gets or sets the description.</value>
        public string Log
        {
            get
            {
                return log;
            }

            set
            {
                log = value;
            }
        }

        /// <summary>Gets the parent IDs.</summary>
        /// <value>Gets the parent IDs.</value>
        public List<string> Parents
        {
            get
            {
                return parents;
            }
        }
    }
}

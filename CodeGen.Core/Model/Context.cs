//////using System;
//////using System.Collections.Generic;
//////using System.Linq;
//////using System.Text;
//////using System.Threading.Tasks;

//////namespace CodeGen.Core.Model
//////{
//////    public class Context
//////    {
//////        private static string _startDelimeter = "{";
//////        private static string _endingDelimiter = "}";

//////        public static string StartDelimeter
//////        {
//////            get { return _startDelimeter; }
//////            set { _startDelimeter = value; }
//////        }

//////        public static string EndingDelimiter
//////        {
//////            get { return _endingDelimiter; }
//////            set { _endingDelimiter = value; }
//////        }

//////        public string Input { get; set; }

//////        public string Output { get; set; }

//////        internal object Extra { get; set; }
//////    }

//////}
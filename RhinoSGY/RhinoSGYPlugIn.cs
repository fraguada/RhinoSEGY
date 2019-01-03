using System;
using Rhino;
using Unplugged.Segy;

namespace RhinoSGY
{
    ///<summary>
    /// <para>Every RhinoCommon .rhp assembly must have one and only one PlugIn-derived
    /// class. DO NOT create instances of this class yourself. It is the
    /// responsibility of Rhino to create an instance of this class.</para>
    /// <para>To complete plug-in information, please also see all PlugInDescription
    /// attributes in AssemblyInfo.cs (you might need to click "Project" ->
    /// "Show All Files" to see it in the "Solution Explorer" window).</para>
    ///</summary>
    public class RhinoSGYPlugIn : Rhino.PlugIns.FileImportPlugIn

    {
        public RhinoSGYPlugIn()
        {
            Instance = this;
        }

        ///<summary>Gets the only instance of the RhinoSGYPlugIn plug-in.</summary>
        public static RhinoSGYPlugIn Instance
        {
            get; private set;
        }

        ///<summary>Defines file extensions that this import plug-in is designed to read.</summary>
        /// <param name="options">Options that specify how to read files.</param>
        /// <returns>A list of file types that can be imported.</returns>
        protected override Rhino.PlugIns.FileTypeList AddFileTypes(Rhino.FileIO.FileReadOptions options)
        {
            var result = new Rhino.PlugIns.FileTypeList();
            result.AddFileType("SEG-Y File (*.sgy)", "sgy");
            return result;
        }

        /// <summary>
        /// Is called when a user requests to import a ."sgy file.
        /// It is actually up to this method to read the file itself.
        /// </summary>
        /// <param name="filename">The complete path to the new file.</param>
        /// <param name="index">The index of the file type as it had been specified by the AddFileTypes method.</param>
        /// <param name="doc">The document to be written.</param>
        /// <param name="options">Options that specify how to write file.</param>
        /// <returns>A value that defines success or a specific failure.</returns>
        protected override bool ReadFile(string filename, int index, RhinoDoc doc, Rhino.FileIO.FileReadOptions options)
        {
            bool read_success = false;

            var reader = new SegyReader();
            ISegyFile file = reader.Read(filename);

            RhinoApp.WriteLine("SEGY: Header Text: {0}",file.Header.Text);
            RhinoApp.WriteLine("SEGY: Nº of Traces: {0}", file.Traces.Count);
            for(int i = 0; i < file.Traces.Count; i++)
            {
                var trace = file.Traces[0];
                RhinoApp.WriteLine("SEGY: Trace #" + trace.Header.TraceNumber + " Count #: " + i + " Sample Count: {0}", trace.Header.SampleCount);
                RhinoApp.WriteLine("SEGY: Trace #" + trace.Header.TraceNumber + " Count #: " + i + " Crossline Nº: {0}", trace.Header.CrosslineNumber);
                RhinoApp.WriteLine("SEGY: Trace #" + trace.Header.TraceNumber + " Count #: " + i + " Inline Nº: {0}", trace.Header.InlineNumber);

                for (int j = 0; j < trace.Values.Count; j++)
                {
                    RhinoApp.WriteLine("SEGY: Trace #" + trace.Header.TraceNumber + " Count #: " + i + " Value Nº " + j + ": {0}", trace.Values[j]);
                }
            }

            read_success = true;

            return read_success;
        }

        // You can override methods here to change the plug-in behavior on
        // loading and shut down, add options pages to the Rhino _Option command
        // and maintain plug-in wide options in a document.
    }
}
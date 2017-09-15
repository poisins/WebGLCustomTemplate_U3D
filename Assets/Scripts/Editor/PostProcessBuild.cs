// Author: Salvis Poišs (poisins92@gmail.com)
// Feel free to use and modify (and leave some credits :) )!

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

using System;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Core;

namespace Poisins
{
    public class PostProcessBuild
    {
        // Unzip compressed WebGL files for GZIP compression enabled server upload
        [PostProcessBuildAttribute(1)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target == BuildTarget.WebGL )
            {
                if (!Debug.isDebugBuild)
                {
                    string pureBuildPath = pathToBuiltProject;
                    pureBuildPath = Path.Combine(pureBuildPath, "Release");
					if (Directory.Exists(pureBuildPath))
					{
						foreach (string file in Directory.GetFiles(pureBuildPath,"*.*gz"))
						{
							if (UnGzipFiles(pureBuildPath, file))
								File.Delete(file);
						}
					}
                }
            }
        }

        /// <summary>
        /// Extracts the file contained within a GZip to the target dir.
        /// A GZip can contain only one file, which by default is named the same
        /// as the GZip except without the extension.
        /// </summary>
        public static bool UnGzipFiles(string targetDir, string gzipFileName)
        {
            try
            {
                // Use a 4K buffer. Any larger is a waste.    
                byte[] dataBuffer = new byte[4096];

                using (Stream fs = new FileStream(gzipFileName, FileMode.Open, FileAccess.Read))
                {
                    using (GZipInputStream gzipStream = new GZipInputStream(fs))
                    {
                        // Change this to your needs - just remove GZ from extension
                        string fnOut = Path.Combine(targetDir, gzipFileName.Remove(gzipFileName.Length-2,2));

                        using (FileStream fsOut = File.Create(fnOut))
                        {
                            StreamUtils.Copy(gzipStream, fsOut, dataBuffer);
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

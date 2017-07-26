using System;
using System.IO.Pipes;
using System.IO;
using System.Security.Principal;

namespace functionspipehack
{
  class Program
  {
    static void Main(string[] args)
    {
      var fileStream = File.OpenRead("named-pipes-test.csproj");

      Console.WriteLine("Setting up named pipe");

      NamedPipeClientStream pipeClient =
          new NamedPipeClientStream(".", "\\.\\testpipe",
              PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Anonymous);

      Console.WriteLine("Waiting for connection...\n");
      pipeClient.Connect();
      Console.WriteLine("Connected...\n");

      int chunkSize = 255;

      using (StreamReader sr = new StreamReader(fileStream))
      using (StreamWriter sw = new StreamWriter(pipeClient))
      {
        while (!sr.EndOfStream)
        {
          var chunk = new char[255];
          sr.ReadBlock(chunk, 0, chunkSize);

          sw.Write(chunk);
          Console.WriteLine(DateTime.Now.ToString() + "Write chuck...\n");
          pipeClient.WaitForPipeDrain();
        }
      }
    }
  }
}
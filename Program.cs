using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using System.IO.Pipes;
using System;
using System.Drawing;
using System.IO;
//using static System.Net.Mime.MediaTypeNames;

String win1 = "Test Window (Press any key to close)"; //The name of the window

CvInvoke.NamedWindow(win1); //Create the window using the specific name

Mat frame = new Mat();       //getting an instance of Mat 

//VideoCapture capture = new VideoCapture("rtsp://admin:vee22cam@192.168.20.56:554/MediaInput/mpeg4");

VideoCapture capture = new VideoCapture("http://217.24.53.18:80/cgi-bin/faststream.jpg?stream=half&fps=15&rand=COUNTER");

var server = new NamedPipeServerStream("PipesOfPiece");

Console.WriteLine("Wait for client...");

server.WaitForConnection();
Console.WriteLine("connected");



StreamWriter writer = new StreamWriter(server);




while (CvInvoke.WaitKey(1) == -1)
{
    capture.Read(frame);

    if (!(frame.IsEmpty))
    {
        CvInvoke.Imshow(win1, frame);
        string s = ConvertImageToBase64(frame);
       // Console.WriteLine(s);
        writer.WriteLine(s);
        writer.Flush();


    }
}

string ConvertImageToBase64(Mat frame)
{
    Image<Bgr, byte> afterImage = frame.ToImage<Bgr, byte>();

    byte[] afterbytes = afterImage.ToJpegData();
    string base64String = Convert.ToBase64String(afterbytes);
    return base64String;
}
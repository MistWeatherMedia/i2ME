using System.Net;
using System.Net.Sockets;
using TWC.I2.MsgEncode;
using TWC.I2.MsgEncode.FEC;
using TWC.I2.MsgEncode.ProcessingSteps;
using TWC.Msg;
using System.Diagnostics;


namespace MistWX_i2Me.Communication;

public class UdpSender
{
    private Config _config = Config.config;
    
    private string _tempDirectory = Path.Combine(AppContext.BaseDirectory, "temp");
    private readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private IPAddress _ipAddress;
    private IPEndPoint _endPoint;
    private UdpClient _udpClient;

    public UdpSender(string destination, int port, string interfaceIp)
    {
        _ipAddress = IPAddress.Parse(destination);
        _endPoint = new IPEndPoint(_ipAddress, port);
        
        _udpClient = new UdpClient();
        _udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        _udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, port));
        _udpClient.JoinMulticastGroup(_ipAddress, 64);
        
        Log.Info($"UDP Socket established at port {port}.");
    }

    public void SendFile(string fileName, string command, bool gZipEncode = true, string? headendId = null)
    {
        
        if (!Config.config.UnitConfig.UseExecInstead)
        {

            string tempFile = Path.Combine(_tempDirectory, Guid.NewGuid().ToString() + ".i2m");
            string fecTempFile = Path.Combine(_tempDirectory, Guid.NewGuid().ToString() + ".i2m");
        
            Log.Info($"Sending new file\nCommand: {command}\nFileName: {tempFile}");

            File.Copy(fileName, tempFile);

            List<IMsgEncodeStep> steps = new List<IMsgEncodeStep>();
            ExecMsgEncodeStep execMsgEncodeStep = new ExecMsgEncodeStep(command);
            steps.Add(execMsgEncodeStep);

            if (gZipEncode)
            {
                steps.Add(new GzipMsgEncoderDecoder());
            }

            if (headendId != null)
            {
                steps.Add(new CheckHeadendIdMsgEncodeStep(headendId));
            }

            MsgEncoder encoder = new MsgEncoder(steps);
            encoder.Encode(tempFile);

            FecEncoder fecEncoder = FecEncoder.Create(FecEncoding.None, (ushort)DgPacket.MAX_PAYLOAD_SIZE, 1, 2);
            Stream inputStream = (Stream)File.OpenRead(tempFile);

            using (Stream oStream = (Stream)File.OpenWrite(fecTempFile))
            {
                fecEncoder.Encode(inputStream, oStream);
            }


            I2Msg msg = new I2Msg(fecTempFile);
            msg.Id = (uint)GetUnixTimeStampMillis();
            msg.Start();

            uint count = msg.CalcMsgPacketCount();

            uint packets = 0;
            while (packets < count)
            {
                byte[] bytes = msg.GetNextPacket();
                _udpClient.Send(bytes, bytes.Length, _endPoint);
                packets++;
                Thread.Sleep(2);
            }
            // Clean up
            msg.Dispose();
            inputStream.Close();
            File.Delete(fecTempFile);
            File.Delete(tempFile);
        } else
        {
            ProcessStartInfo procStartInfo = new ProcessStartInfo("C:/Program Files (x86)/TWC/i2/exec.exe");
            procStartInfo.Arguments = $"-async {$"{command.Remove(command.Length - 1, 1)},File={fileName})"}";
            Log.Info($"Sending new file\nCommand: {command}\nFileName: {fileName}");
            Log.Debug($"Exec arguments: {procStartInfo.Arguments}");
            Process.Start(procStartInfo);
        }
        
    }

    public void SendCommand(string command, string? headendId = null)
    {
        Log.Info($"New command received.\nCommand: {command}");
        string tempFile = Path.Combine(_tempDirectory, Guid.NewGuid().ToString() + ".i2m");
        File.WriteAllText(tempFile, command);
        SendFile(tempFile, command, false, headendId);
        File.Delete(tempFile);
    }

    public long GetUnixTimeStampMillis()
    {
        return (long)(DateTime.UtcNow - _unixEpoch).TotalMilliseconds;
    }
}

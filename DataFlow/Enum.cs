using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFlow
{
    public enum ConnectType
    {
        [System.ComponentModel.Description("Hermes(Upstream)")]
        Upstream,

        [System.ComponentModel.Description("Hermes(Downstream)")]
        Downstream,

        [System.ComponentModel.Description("NXT(Upstream)")]
        NXT_Upstream,

        [System.ComponentModel.Description("NXT(Downstream)")]
        NXT_Downstream,

        [System.ComponentModel.Description("PLC")]
        PLC
    }

    public enum MessageType
    {
        Read,
        Write
    }

    public enum RepositoryType
    {
        ADD,
        DEL,
        SEARCH,
        DELALL,
        UPDATE
    }

    public enum Command : byte
    {
        DLE   = 0x10,
        ACK   = 0x06,
        ETX   = 0x03,
        STX   = 0x02,
        STN   = 0x00,
        CMP   = 0xA0,
        READ  = 0x20,
        WRITE = 0x28
    }

    public enum Direction
    {
        SerialPort,
        Repository,
        LogicController,
        TEST
    }

    public enum Check
    { 
       OK = 1,
       NG
    }

    public enum Status
    {
        UnTrack,
        Track,
        Change,
        Modify,
        Submit,
        Finish,
        Poll,
        Search,
        Error,
        Check,
        Reconnect,
        Disconnect
    }

    public enum MachineStatus
    {
        [System.ComponentModel.Description("NotConnected")]
        NotConnectedStatus,

        [System.ComponentModel.Description("ServiceDescription")]
        ServiceDescriptionStatus,

        [System.ComponentModel.Description("NotAvailableNotReady")]
        NotAvailableNotReadyStatus,

        [System.ComponentModel.Description("BoardAvailable")]
        BoardAvailableStatus,

        [System.ComponentModel.Description("MachineReady")]
        MachineReadyStatus,

        [System.ComponentModel.Description("AvailableAndReady")]
        AvailableAndReadyStatus,

        [System.ComponentModel.Description("Transporting")]
        TransportingStatus,

        [System.ComponentModel.Description("TransportFinished")]
        TransportFinishedStatus,

        [System.ComponentModel.Description("TransportStopped")]
        TransportStoppedStatus
    }
}

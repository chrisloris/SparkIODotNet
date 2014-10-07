using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparkIO.WebServices;

namespace DrivEmTinkerAPI
{
    public class Helper
    {
        private TinkerAPI tinker;
        private const string coreID = "abcdefabcdefabcdefabcdef";
        private const string accessToken = "abcdefabcdefabcdefabcdefabcdefabcdefabcd";

        private const TinkerAPI.Pins rightDirPin = TinkerAPI.Pins.D2;
        private const TinkerAPI.Pins leftDirPin = TinkerAPI.Pins.D3;
        private const TinkerAPI.Pins rightMotorPin = TinkerAPI.Pins.D0;
        private const TinkerAPI.Pins leftMotorPin = TinkerAPI.Pins.D1;

        private enum Speed
        {
            speedStop = 0,
            speedHalf = 50,
            speedFull = 250
        }

        public enum DriveCommands
        {
            Forward,
            ForwardLeft,
            ForwardRight,
            Left,
            Right,
            Rear,
            RearLeft,
            RearRight,
            Stop
        }

        public Helper()
        {
            tinker = new TinkerAPI(coreID, accessToken);
        }

        private void driveCommand(TinkerAPI.SetStates? LeftDir, TinkerAPI.SetStates? RightDir, Speed LeftSpeed, Speed RightSpeed)
        {
            TinkerAPI.DigitalWriteDelegate dwdL = new TinkerAPI.DigitalWriteDelegate(tinker.DigitalWrite);
            TinkerAPI.DigitalWriteDelegate dwdR = new TinkerAPI.DigitalWriteDelegate(tinker.DigitalWrite);

            TinkerAPI.AnalogWriteDelegate awdL = new TinkerAPI.AnalogWriteDelegate(tinker.AnalogWrite);
            TinkerAPI.AnalogWriteDelegate awdR = new TinkerAPI.AnalogWriteDelegate(tinker.AnalogWrite);

            IAsyncResult resultL;
            IAsyncResult resultR;

            if (LeftDir.HasValue && RightDir.HasValue)
            {
                resultL = dwdL.BeginInvoke(leftDirPin, LeftDir.Value, null, null);
                resultR = dwdR.BeginInvoke(rightDirPin, RightDir.Value, null, null);

                TinkerAPI.GetStates gsresultL = dwdL.EndInvoke(resultL);
                TinkerAPI.GetStates gsresultR = dwdR.EndInvoke(resultR);
            }

            resultL = awdL.BeginInvoke(leftMotorPin, (short)LeftSpeed, null, null);
            resultR = awdR.BeginInvoke(rightMotorPin, (short)RightSpeed, null, null);

            bool bresultL = awdL.EndInvoke(resultL);
            bool bresultR = awdR.EndInvoke(resultR);
        }

        //private void driveCommand(TinkerAPI.SetStates? LeftDir, TinkerAPI.SetStates? RightDir, Speed LeftSpeed, Speed RightSpeed)
        //{
        //    if (LeftDir.HasValue && RightDir.HasValue)
        //    {
        //        tinker.DigitalWrite(leftDirPin, LeftDir.Value);
        //        tinker.DigitalWrite(rightDirPin, RightDir.Value);
        //    }

        //    tinker.AnalogWrite(leftMotorPin, (short)LeftSpeed);
        //    tinker.AnalogWrite(rightMotorPin, (short)RightSpeed);
        //}

        public void GiveCommand(DriveCommands command)
        {
            switch (command)
            {
                case DriveCommands.Forward:
                    driveCommand(TinkerAPI.SetStates.HIGH, TinkerAPI.SetStates.HIGH, Speed.speedFull, Speed.speedFull);
                    break;
                case DriveCommands.ForwardLeft:
                    driveCommand(TinkerAPI.SetStates.HIGH, TinkerAPI.SetStates.HIGH, Speed.speedHalf, Speed.speedFull);
                    break;
                case DriveCommands.ForwardRight:
                    driveCommand(TinkerAPI.SetStates.HIGH, TinkerAPI.SetStates.HIGH, Speed.speedFull, Speed.speedHalf);
                    break;
                case DriveCommands.Left:
                    driveCommand(TinkerAPI.SetStates.LOW, TinkerAPI.SetStates.HIGH, Speed.speedFull, Speed.speedFull);
                    break;
                case DriveCommands.Right:
                    driveCommand(TinkerAPI.SetStates.HIGH, TinkerAPI.SetStates.LOW, Speed.speedFull, Speed.speedFull);
                    break;
                case DriveCommands.Rear:
                    driveCommand(TinkerAPI.SetStates.LOW, TinkerAPI.SetStates.LOW, Speed.speedFull, Speed.speedFull);
                    break;
                case DriveCommands.RearLeft:
                    driveCommand(TinkerAPI.SetStates.LOW, TinkerAPI.SetStates.LOW, Speed.speedHalf, Speed.speedFull);
                    break;
                case DriveCommands.RearRight:
                    driveCommand(TinkerAPI.SetStates.LOW, TinkerAPI.SetStates.LOW, Speed.speedFull, Speed.speedHalf);
                    break;
                case DriveCommands.Stop:
                    driveCommand(null, null, Speed.speedStop, Speed.speedStop);
                    break;
            }
        }
    }
}

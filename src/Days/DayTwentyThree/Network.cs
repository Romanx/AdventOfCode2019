using Helpers;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace DayTwentyThree
{
    public class Network
    {
        private readonly ImmutableDictionary<int, IntcodeComputer> _interfaces;
        private readonly ImmutableDictionary<int, Queue<Packet>> _packetBuffer;

        private readonly TextWriter? _output;
        private Packet _previousNat;
        private Packet _natPacket;

        public Network(int machineCount, ImmutableArray<long> program, TextWriter? output = null)
        {
            var builder = ImmutableDictionary.CreateBuilder<int, IntcodeComputer>();
            var buffer = ImmutableDictionary.CreateBuilder<int, Queue<Packet>>();
            for (var i = 0; i < machineCount; i++)
            {
                builder[i] = BuildComputer(i, program);
                buffer[i] = new Queue<Packet>();
            }

            _interfaces = builder.ToImmutable();
            _packetBuffer = buffer.ToImmutable();
            _output = output;

            static IntcodeComputer BuildComputer(int address, ImmutableArray<long> program)
            {
                var computer = new IntcodeComputer(program);
                computer.Input.Enqueue(address);

                return computer;
            }
        }

        public Packet Simulate()
        {
            while(true)
            {
                foreach (var (i, computer) in _interfaces)
                {
                    if (_packetBuffer[i].Count > 0)
                    {
                        WritePendingPackets(computer, _packetBuffer[i]);
                    }
                    else
                    {
                        computer.Input.Enqueue(-1);
                    }

                    computer.Run();

                    while (computer.Output.Count > 0)
                    {
                        var addr = (int)computer.Output.Dequeue();
                        var x = computer.Output.Dequeue();
                        var y = computer.Output.Dequeue();
                        var packet = new Packet(x, y);

                        if (addr == 255)
                        {
                            return packet;
                        }

                        _output?.WriteLine($"Packet Sent to {addr}: X = {packet.X}, Y = {packet.Y}");

                        _packetBuffer[addr].Enqueue(packet);
                    }
                }

            }

            static void WritePendingPackets(IntcodeComputer computer, Queue<Packet> queue)
            {
                while (queue.TryDequeue(out var p))
                {
                    computer.Input.Enqueue(p.X);
                    computer.Input.Enqueue(p.Y);
                }
            }
        }

        public Packet SimulateWithNat()
        {
            while (true)
            {
                var idle = true;
                foreach (var (i, computer) in _interfaces)
                {
                    if (_packetBuffer[i].Count > 0)
                    {
                        idle = false;
                        WritePendingPackets(computer, _packetBuffer[i]);
                    }
                    else
                    {
                        computer.Input.Enqueue(-1);
                    }

                    computer.Run();

                    while (computer.Output.Count > 0)
                    {
                        idle = false;
                        var addr = (int)computer.Output.Dequeue();
                        var x = computer.Output.Dequeue();
                        var y = computer.Output.Dequeue();
                        var packet = new Packet(x, y);

                        if (addr == 255)
                        {
                            _previousNat = _natPacket;
                            _natPacket = packet;
                            continue;
                        }

                        _output?.WriteLine($"Packet Sent to {addr}: X = {packet.X}, Y = {packet.Y}");

                        _packetBuffer[addr].Enqueue(packet);
                    }
                }

                if (idle)
                {
                    if (_previousNat.Y == _natPacket.Y)
                    {
                        return _natPacket;
                    }

                    _output?.WriteLine($"NAT Packet Sent to 0: X = {_natPacket.X}, Y = {_natPacket.Y}");
                    _packetBuffer[0].Enqueue(_natPacket);
                }
            }

            static void WritePendingPackets(IntcodeComputer computer, Queue<Packet> queue)
            {
                while (queue.TryDequeue(out var p))
                {
                    computer.Input.Enqueue(p.X);
                    computer.Input.Enqueue(p.Y);
                }
            }
        }
    }
}

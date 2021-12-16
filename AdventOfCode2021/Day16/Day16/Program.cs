using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day16
{
    public class Packet
    {
        enum PacketType
        {
            Sum = 0,
            Product = 1,
            Minimum = 2,
            Maximum = 3,
            Literal = 4,
            GreaterThan = 5,
            LessThan = 6,
            EqualTo = 7,
        }

        int version;
        PacketType type_id;
        string literal_value;
        bool length_type_id;
        List<Packet> sub_packets;

        Packet()
        {
            sub_packets = new List<Packet>();
        }

        static string extract(StringReader str, int num_chars)
        {
            char[] buffer = new char[num_chars];
            Array.Fill(buffer, '0');
            str.Read(buffer, 0, num_chars);

            return new string(buffer);
        }

        static int extractInt(StringReader str, int num_chars)
        {
            return Convert.ToInt32(extract(str, num_chars), 2);
        }

        static string parseLiteral(StringReader binary_string)
        {
            StringBuilder sb = new StringBuilder();
            for(bool keep_going = true; keep_going;)
            {
                keep_going = (extract(binary_string, 1) == "1");
                sb.Append(extract(binary_string, 4));
            }
            return sb.ToString();
        }
        static void parseHeader(Packet packet, StringReader binary_string)
        {
            packet.version = extractInt(binary_string, 3);
            packet.type_id = (PacketType)extractInt(binary_string, 3);
        }

        static void parseNumSubpackets(Packet packet, StringReader binary_string)
        {
            int num_sub_packets = extractInt(binary_string, 11);
            for (int i = 0; i < num_sub_packets; ++i)
            {
                packet.sub_packets.Add(parse(binary_string));
            }
        }

        static void parseSubPacketWithLength(Packet packet, StringReader binary_string)
        {
            int sub_packet_length = extractInt(binary_string, 15);
            string sub_packet_string = extract(binary_string, sub_packet_length);

            StringReader sr = new StringReader(sub_packet_string);
            while (sr.Peek() != -1)
            {
                packet.sub_packets.Add(parse(sr));
            }
        }

        static void parseOperator(Packet packet, StringReader binary_string)
        {
            packet.length_type_id = (extract(binary_string, 1) == "1");
            if (packet.length_type_id)
            {
                parseNumSubpackets(packet, binary_string);
            }
            else
            {
                parseSubPacketWithLength(packet, binary_string);
            }
        }

        static public Packet parse(StringReader binary_string)
        {
            Packet packet = new Packet();
            parseHeader(packet, binary_string);
            if (packet.type_id == PacketType.Literal)
            {
                packet.literal_value = parseLiteral(binary_string);
            }
            else
            {
                parseOperator(packet, binary_string);
            }
            return packet;
        }

        public long getVersionSum()
        {
            long sum = sub_packets.Select(p => p.getVersionSum()).Sum();
            return sum + version;
        }

        public long execute()
        {
            return type_id switch
            {
                PacketType.Sum => sub_packets.Select(pkt => pkt.execute()).Sum(),
                PacketType.Product=> sub_packets.Select(pkt => pkt.execute()).Aggregate(1L, (next, total) => total * next),
                PacketType.Minimum=> sub_packets.Select(pkt => pkt.execute()).Min(),
                PacketType.Maximum=> sub_packets.Select(pkt => pkt.execute()).Max(),
                PacketType.GreaterThan=> (sub_packets[0].execute() > sub_packets[1].execute()) ? 1 : 0,
                PacketType.LessThan=> (sub_packets[0].execute() < sub_packets[1].execute()) ? 1 : 0,
                PacketType.EqualTo=> (sub_packets[0].execute() == sub_packets[1].execute()) ? 1 : 0,
                PacketType.Literal=> Convert.ToInt64(literal_value, 2),
                _ => throw new ArgumentOutOfRangeException(nameof(type_id), $"Unexpected type_id value: {type_id}"),
            };
        }
    }
    public class Program
    {
        static string readInput(string file_name)
        {
            return File.ReadLines(file_name).First();
        }

        static string hexToBinary(string hex_string)
        {
            return string.Join(null, hex_string.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
        }

        public static long part1(string transmission)
        {
            transmission = hexToBinary(transmission).TrimEnd('0');
            Packet packet = Packet.parse(new StringReader(transmission));

            return packet.getVersionSum();
        }

        public static long part2(string transmission)
        {
            transmission = hexToBinary(transmission).TrimEnd('0');
            Packet packet = Packet.parse(new StringReader(transmission));

            return packet.execute();
        }

        static void Main(string[] args)
        {
            Console.WriteLine(part1(readInput("input.txt")));
            Console.WriteLine(part2(readInput("input.txt")));
        }
    }
}

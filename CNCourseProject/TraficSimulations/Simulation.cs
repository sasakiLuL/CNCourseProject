using ComputerNetworksCourseWork.Networks.Nods;

namespace ComputerNetworksCourseWork.TraficSimulations;

public record Simulation(
    Protocol Protocol,
    Node Source,
    Node Destination,
    int ServicePacketLenght,
    int InfoPacketLenght,
    int InfoPacketsCount,
    int MessageSize,
    int Time);

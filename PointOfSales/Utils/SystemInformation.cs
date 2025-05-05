using DeviceId;
using PointOfSales.Core.Utils;

namespace PointOfSales.Utils;

public class SystemInformation: ISystemInformation
{
    public string GetMachineUniqueCodeAsync()
    {
        string deviceId = new DeviceIdBuilder()
            .AddMachineName()
            .AddOsVersion()
            .OnWindows(windows => windows
                .AddProcessorId()
                .AddMotherboardSerialNumber()
                .AddSystemDriveSerialNumber())
            .OnLinux(linux => linux
                .AddMotherboardSerialNumber()
                .AddSystemDriveSerialNumber())
            .OnMac(mac => mac
                .AddSystemDriveSerialNumber()
                .AddPlatformSerialNumber())
            .ToString();
        return deviceId;
    }
}